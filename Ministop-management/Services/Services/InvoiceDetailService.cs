using AutoMapper;
using Domain.DTO;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Services.Interfaces;
using Shared.ErrorCode;
using Shared.Helpers;
using Shared.Security;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class InvoiceDetailService : IInvoiceDetailService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;
        public InvoiceDetailService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<IReadOnlyList<InvoiceDetailDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.InvoiceDetailRepository.GetAll();

            // Map sang Domain.Entity.Shift
            var list = _mapper.Map<IReadOnlyList<InvoiceDetailDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<InvoiceDetailDto>>(list);
        }


        public Result<InvoiceDetailDto> GetInvoiceDetailByID(string id)
        {
            try
            {
                var InvoiceEntity = _ministopUnitOfWork.InvoiceDetailRepository.Find(x => x.Id == id);
                if (InvoiceEntity == null)
                {
                    return new Result<InvoiceDetailDto>(ErrorCodeEnum.STR_ERR_001);//chua sua error
                }
                var result = _mapper.Map<InvoiceDetailDto>(InvoiceEntity);
                return new Result<InvoiceDetailDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<InvoiceDetailDto>> GetInvoiceDetail(int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.InvoiceDetailRepository.GetCount();
            var InvoiceDetailEntity = _ministopUnitOfWork.InvoiceDetailRepository.GetPagedResponse(pageNumber, pageSize);
            var InvoiceDetailsDto = _mapper.Map<IReadOnlyList<InvoiceDetailDto>>(InvoiceDetailEntity);

            return new PagedResult<IReadOnlyList<InvoiceDetailDto>>(InvoiceDetailsDto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreateInvoiceDetail(InvoiceDetailDto InvoiceDetailDto)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var isAvailable = _ministopUnitOfWork.StockDetailRepository.Find(x => x.StoreID == _userSession.IdStore && x.ProductID == InvoiceDetailDto.ProductId);
                if (isAvailable.Quantity <= 0)
                {
                    //thong bao da het hang san pham
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }
                decimal subTotal = 0;
                decimal discountTotal = 0;
                var productPrice = isAvailable.Price;
                decimal discountAmt = 0;
                decimal finalPrice = productPrice;
                var isDuplicate = _ministopUnitOfWork.InvoiceDetailRepository.Find(x => x.ProductID == InvoiceDetailDto.ProductId && x.InvoiceID == InvoiceDetailDto.InvoiceId);
                if (isDuplicate != null)
                {
                    isDuplicate.Quantity += InvoiceDetailDto.Quantity;
                    var promotion = GetActivePromotion(isDuplicate.ProductID, isDuplicate.Quantity);
                    if (promotion != null)
                    {
                        discountAmt = promotion.DiscountAmount;
                        finalPrice = productPrice - discountAmt;

                    }
                    subTotal += productPrice * isDuplicate.Quantity;
                    discountTotal += discountAmt * isDuplicate.Quantity;
                    isDuplicate.DiscountAmount = discountAmt;
                    _ministopUnitOfWork.InvoiceDetailRepository.Update(isDuplicate);
                    isAvailable.Quantity -= isDuplicate.Quantity;
                    isAvailable.LastUpdate = DateTime.UtcNow.ToLocalTime();
                    var stockDetail = _mapper.Map<StockDetail>(isAvailable);
                    _ministopUnitOfWork.StockDetailRepository.Update(stockDetail);
                    _ministopUnitOfWork.Commit();
                    return new Result<bool>(true);
                }
                else
                {
                    var InvoiceDetailId = IdGenerator.CreateID("IVD");
                    InvoiceDetailDto.Id = InvoiceDetailId;
                    var promotion = GetActivePromotion(InvoiceDetailDto.ProductId, InvoiceDetailDto.Quantity);
                    if (promotion != null)
                    {
                        discountAmt = promotion.DiscountAmount;
                        finalPrice = productPrice - discountAmt;
                    }
                    InvoiceDetailDto.DiscountAmount = discountAmt;
                    var InvoiceDetailEntity = _mapper.Map<InvoiceDetail>(InvoiceDetailDto);
                    var succeeded = _ministopUnitOfWork.InvoiceDetailRepository.Add(InvoiceDetailEntity);
                    if (succeeded == null)
                    {
                        return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                    }
                    isAvailable.Quantity -= InvoiceDetailDto.Quantity;
                    isAvailable.LastUpdate = DateTime.UtcNow.ToLocalTime();
                    var stockDetail = _mapper.Map<StockDetail>(isAvailable);
                    _ministopUnitOfWork.StockDetailRepository.Update(stockDetail);
                }

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        private PromotionProductDto GetActivePromotion(string productId, int qty)
        {
            var promotion = _ministopUnitOfWork.PromotionProductRepository.GetActivePromotionForProduct(productId, qty);
            var entity = _mapper.Map<PromotionProductDto>(promotion);
            return entity;
        }

        public Result<bool> UpdateInvoiceDetail(InvoiceDetailDto invoiceDetailDto)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                // 1. Kiểm tra InvoiceDetail có tồn tại không
                var existingDetail = _ministopUnitOfWork.InvoiceDetailRepository.Find(x => x.Id == invoiceDetailDto.Id);
                if (existingDetail == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }

                // 2. Kiểm tra Invoice có đang ở trạng thái cho phép sửa không
                var invoice = _ministopUnitOfWork.InvoiceRepository.Find(x => x.InvoiceID == existingDetail.InvoiceID);
                if (invoice == null || invoice.Status != 0)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }

                // 3. Kiểm tra kho
                var stockDetail = _ministopUnitOfWork.StockDetailRepository.Find(
                    x => x.StoreID == _userSession.IdStore && x.ProductID == existingDetail.ProductID
                );

                if (stockDetail == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }

                // 4. Tính toán sự thay đổi số lượng
                int oldQuantity = existingDetail.Quantity;
                int newQuantity = invoiceDetailDto.Quantity;
                int quantityDiff = newQuantity - oldQuantity;

                // 5. Kiểm tra số lượng trong kho
                // Nếu tăng số lượng (quantityDiff > 0) thì phải kiểm tra kho có đủ không
                if (quantityDiff > 0)
                {
                    if (stockDetail.Quantity < quantityDiff)
                    {
                        return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                    }
                }

                // 6. Cập nhật số lượng trong kho
                // - Nếu tăng số lượng: trừ thêm từ kho
                // - Nếu giảm số lượng: hoàn lại vào kho
                stockDetail.Quantity -= quantityDiff;
                stockDetail.LastUpdate = DateTime.UtcNow.ToLocalTime();
                var entity = _mapper.Map<StockDetail>(stockDetail);
                _ministopUnitOfWork.StockDetailRepository.Update(entity);

                // 7. Tính lại discount dựa trên số lượng mới
                decimal productPrice = stockDetail.Price;
                decimal discountAmt = 0;

                var promotion = GetActivePromotion(existingDetail.ProductID, newQuantity);
                if (promotion != null)
                {
                    discountAmt = promotion.DiscountAmount;
                }

                // 8. Cập nhật InvoiceDetail
                existingDetail.Quantity = newQuantity;
                existingDetail.UnitPrice = productPrice;
                existingDetail.DiscountAmount = discountAmt;
                existingDetail.FinalUnitPrice = productPrice - discountAmt;

                _ministopUnitOfWork.InvoiceDetailRepository.Update(existingDetail, true);

                // 9. Cập nhật lại Invoice
                RecalculateInvoiceTotals(invoice.InvoiceID);

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        // ===== HÀM XÓA InvoiceDetail =====
        public Result<bool> RemoveInvoiceDetail(string invoiceDetailId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                // 1. Kiểm tra InvoiceDetail có tồn tại không
                var invoiceDetail = _ministopUnitOfWork.InvoiceDetailRepository.Find(x => x.Id == invoiceDetailId);
                if (invoiceDetail == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }

                // 2. Kiểm tra Invoice có đang ở trạng thái cho phép xóa không
                var invoice = _ministopUnitOfWork.InvoiceRepository.Find(x => x.InvoiceID == invoiceDetail.InvoiceID);
                if (invoice == null || invoice.Status != 0)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }

                // 3. Hoàn lại số lượng vào kho
                var stockDetail = _ministopUnitOfWork.StockDetailRepository.Find(
                    x => x.StoreID == _userSession.IdStore && x.ProductID == invoiceDetail.ProductID
                );

                if (stockDetail != null)
                {
                    // Cộng lại số lượng đã trừ
                    stockDetail.Quantity += invoiceDetail.Quantity;
                    stockDetail.LastUpdate = DateTime.UtcNow.ToLocalTime();
                    var entity = _mapper.Map<StockDetail>(stockDetail);
                    _ministopUnitOfWork.StockDetailRepository.Update(entity);
                }

                // 4. Xóa InvoiceDetail
                _ministopUnitOfWork.InvoiceDetailRepository.Delete(invoiceDetail, true);

                // 5. Cập nhật lại Invoice
                RecalculateInvoiceTotals(invoice.InvoiceID);

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        // ===== HÀM TÍNH LẠI TỔNG TIỀN CHO INVOICE =====
        private void RecalculateInvoiceTotals(string invoiceId)
        {
            var invoice = _ministopUnitOfWork.InvoiceRepository.Find(x => x.InvoiceID == invoiceId);
            if (invoice == null) return;

            // Lấy tất cả InvoiceDetails của Invoice này
            var invoiceDetails = _ministopUnitOfWork.InvoiceDetailRepository
                .GetAll()
                .Where(x => x.InvoiceID == invoiceId)
                .ToList();

            // Tính SubTotal và DiscountTotal
            decimal subTotal = 0;
            decimal discountTotal = 0;

            foreach (var detail in invoiceDetails)
            {
                subTotal += detail.UnitPrice * detail.Quantity;
                discountTotal += detail.DiscountAmount * detail.Quantity;
            }

            // Cập nhật Invoice
            invoice.DiscountTotal = discountTotal;
            invoice.FinalAmount = subTotal - discountTotal;

            _ministopUnitOfWork.InvoiceRepository.Update(invoice, true);
        }

        //private PromotionProductDto GetActivePromotion(string productId, int qty)
        //{
        //    var promotion = _ministopUnitOfWork.PromotionProductRepository.GetActivePromotionForProduct(productId, qty);
        //    var entity = _mapper.Map<PromotionProductDto>(promotion);
        //    return entity;
        //}
    }
}
