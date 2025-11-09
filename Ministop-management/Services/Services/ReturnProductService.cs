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
    public class ReturnProductService : IReturnProductService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public ReturnProductService(
            IMinistopUnitOfWork ministopUnitOfWork,
            IDateTimeService dateTimeService,
            IUserSession userSession,
            IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public Result<IReadOnlyList<ReturnProductDto>> GetAll()
        {
            var dbList = _ministopUnitOfWork.ReturnProductRepository.GetAll();
            var list = _mapper.Map<IReadOnlyList<ReturnProductDto>>(dbList);
            return new Result<IReadOnlyList<ReturnProductDto>>(list);
        }

        public Result<ReturnProductDto> GetReturnProductByID(string id)
        {
            try
            {
                var entity = _ministopUnitOfWork.ReturnProductRepository.Find(x => x.ReturnID == id);
                if (entity == null)
                {
                    return new Result<ReturnProductDto>(ErrorCodeEnum.STR_ERR_001);
                }

                var dto = _mapper.Map<ReturnProductDto>(entity);

                // Lấy chi tiết trả (nếu cần hiển thị)
                var details = _ministopUnitOfWork.ReturnDetailRepository
                    .GetAll()
                    .Where(x => x.ReturnID == id)
                    .ToList();

                // Nếu DTO có property ReturnDetails, map vào
                try
                {
                    var detailsDto = _mapper.Map<IReadOnlyList<object>>(details);
                    // dùng reflection-safe: nếu DTO có ReturnDetails property thì set
                    var prop = typeof(ReturnProductDto).GetProperty("ReturnDetails");
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(dto, detailsDto);
                    }
                }
                catch
                {
                    // Nếu mapping không khớp, không quan trọng — trả về dto chính
                }

                return new Result<ReturnProductDto>(dto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedResult<IReadOnlyList<ReturnProductDto>> GetReturnProduct(string invoiceID, int pageNumber, int pageSize)
        {
            // Lọc theo invoiceID nếu truyền, ngược lại trả tất cả phân trang
            var query = _ministopUnitOfWork.ReturnProductRepository.GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(invoiceID))
            {
                query = query.Where(x => x.InvoiceID == invoiceID);
            }

            var totalCount = query.Count();
            var paged = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var dto = _mapper.Map<IReadOnlyList<ReturnProductDto>>(paged);

            return new PagedResult<IReadOnlyList<ReturnProductDto>>(dto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreateReturnProduct(ReturnProductDto returnProductDto)
        {
            // Lưu ý: mình giả định returnProductDto bao gồm một collection ReturnDetails
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                // 1. Kiểm tra hóa đơn gốc có tồn tại và ở trạng thái được phép (giống InvoiceDetailService logic)
                var invoice = _ministopUnitOfWork.InvoiceRepository.Find(x => x.InvoiceID == returnProductDto.InvoiceId);
                if (invoice == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }

                // 2. Sinh ReturnID
                var returnId = IdGenerator.CreateID("RTP");
                // Set ID vào DTO (nếu property tên ReturnId)
                try { returnProductDto.ReturnId = returnId; } catch { }

                // 3. Map ReturnProductDto -> ReturnProduct entity
                var returnEntity = _mapper.Map<ReturnProduct>(returnProductDto);
                if (returnEntity == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }
                returnEntity.ReturnID = returnId;
                returnEntity.ReturnDate = _dateTimeService.NowUtc; // hoặc returnProductDto.ReturnDate nếu bạn muốn dùng client time

                // 4. Lấy chi tiết trả từ DTO (giả định property ReturnDetails)
                var returnDetailsProp = returnProductDto.GetType().GetProperty("ReturnDetails");
                var returnDetailsDto = returnDetailsProp != null ? returnDetailsProp.GetValue(returnProductDto) as IEnumerable<object> : null;

                // 5. Add ReturnProduct
                var addedReturn = _ministopUnitOfWork.ReturnProductRepository.Add(returnEntity);
                if (addedReturn == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }

                decimal invoiceDecreaseAmount = 0m;

                // 6. Xử lý từng ReturnDetail nếu có
                if (returnDetailsDto != null)
                {
                    foreach (var detailObj in returnDetailsDto)
                    {
                        // Map dynamic object -> ReturnDetail entity
                        var detailDto = detailObj; // giữ nguyên
                        // cố gắng map sang ReturnDetail entity
                        var returnDetailEntity = _mapper.Map<ReturnDetail>(detailDto);
                        if (returnDetailEntity == null)
                        {
                            _ministopUnitOfWork.Rollback();
                            return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                        }

                        // Gán ID cho detail nếu cần
                        if (string.IsNullOrEmpty(returnDetailEntity.Id))
                        {
                            returnDetailEntity.Id = IdGenerator.CreateID("RTD");
                        }
                        returnDetailEntity.ReturnID = returnId;

                        // Lấy stockDetail của cửa hàng và product
                        var stockDetail = _ministopUnitOfWork.StockDetailRepository.Find(
                            x => x.StoreID == _userSession.IdStore && x.ProductID == returnDetailEntity.ProductID
                        );

                        if (stockDetail == null)
                        {
                            // Nếu kho không có thì tạo lỗi (hoặc có thể tạo dòng kho mới tùy nghiệp vụ)
                            _ministopUnitOfWork.Rollback();
                            return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                        }

                        // 6.a Cập nhật kho: tăng lại số lượng trả về kho
                        stockDetail.Quantity += returnDetailEntity.Quantity;
                        stockDetail.LastUpdate = DateTime.UtcNow.ToLocalTime();
                        var sd = _mapper.Map<StockDetail>(stockDetail);
                        _ministopUnitOfWork.StockDetailRepository.Update(sd);

                        // 6.b Cập nhật InvoiceDetail: trừ quantity
                        var invoiceDetail = _ministopUnitOfWork.InvoiceDetailRepository
                            .Find(x => x.InvoiceID == returnProductDto.InvoiceId && x.ProductID == returnDetailEntity.ProductID);

                        if (invoiceDetail != null)
                        {
                            // Nếu trả nhiều hơn tồn tại trong invoiceDetail => lỗi
                            if (returnDetailEntity.Quantity > invoiceDetail.Quantity)
                            {
                                _ministopUnitOfWork.Rollback();
                                return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                            }

                            // Giảm quantity trong InvoiceDetail
                            invoiceDetail.Quantity -= returnDetailEntity.Quantity;

                            // Cập nhật các giá liên quan nếu cần (unit price, discount, final price)
                            // Ở đây mình giả định UnitPrice vẫn giữ nguyên; nếu bạn muốn recalc discount, cần logic thêm.
                            invoiceDetail.FinalUnitPrice = invoiceDetail.UnitPrice - invoiceDetail.DiscountAmount;

                            _ministopUnitOfWork.InvoiceDetailRepository.Update(invoiceDetail, true);

                            // Tính tiền giảm của invoice để cập nhật Invoice.FinalAmount
                            // Sử dụng UnitPrice * quantity (bạn có thể sửa nếu muốn dùng RefundAmount)
                            invoiceDecreaseAmount += (invoiceDetail.UnitPrice - invoiceDetail.DiscountAmount) * returnDetailEntity.Quantity;
                        }
                        else
                        {
                            // Nếu không tìm thấy invoiceDetail (sản phẩm không có trong hóa đơn) => rollback
                            _ministopUnitOfWork.Rollback();
                            return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                        }

                        // 6.c Lưu ReturnDetail
                        var addedDetail = _ministopUnitOfWork.ReturnDetailRepository.Add(returnDetailEntity);
                        if (addedDetail == null)
                        {
                            _ministopUnitOfWork.Rollback();
                            return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                        }
                    }
                }

                // 7. Cập nhật Invoice.FinalAmount giảm theo invoiceDecreaseAmount
                if (invoiceDecreaseAmount != 0m)
                {
                    invoice.FinalAmount = (invoice.FinalAmount ?? 0m) - invoiceDecreaseAmount;
                    _ministopUnitOfWork.InvoiceRepository.Update(invoice, true);
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

        public Result<bool> RemoveReturnProduct(string returnProductID)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                // 1. Lấy ReturnProduct
                var returnEntity = _ministopUnitOfWork.ReturnProductRepository.Find(x => x.ReturnID == returnProductID);
                if (returnEntity == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }

                // 2. Lấy tất cả ReturnDetails
                var details = _ministopUnitOfWork.ReturnDetailRepository.GetAll()
                    .Where(x => x.ReturnID == returnProductID)
                    .ToList();

                // 3. Với mỗi detail: hoàn tác các thay đổi lên kho và invoice
                foreach (var d in details)
                {
                    // a) Cập nhật kho: trừ lại số lượng đã trả
                    var stockDetail = _ministopUnitOfWork.StockDetailRepository.Find(
                        x => x.StoreID == _userSession.IdStore && x.ProductID == d.ProductID
                    );
                    if (stockDetail != null)
                    {
                        // Trừ đi vì khi tạo phiếu trả, đã cộng vào kho
                        stockDetail.Quantity -= d.Quantity;
                        stockDetail.LastUpdate = DateTime.UtcNow.ToLocalTime();
                        var sd = _mapper.Map<StockDetail>(stockDetail);
                        _ministopUnitOfWork.StockDetailRepository.Update(sd);
                    }

                    // b) Cập nhật InvoiceDetail: cộng lại số lượng (hoàn trả lại cho invoice)
                    var invoiceDetail = _ministopUnitOfWork.InvoiceDetailRepository
                        .Find(x => x.InvoiceID == returnEntity.InvoiceID && x.ProductID == d.ProductID);

                    if (invoiceDetail != null)
                    {
                        invoiceDetail.Quantity += d.Quantity;
                        invoiceDetail.FinalUnitPrice = invoiceDetail.UnitPrice - invoiceDetail.DiscountAmount;
                        _ministopUnitOfWork.InvoiceDetailRepository.Update(invoiceDetail, true);

                        // Cập nhật Invoice.FinalAmount tăng lại
                        var invoice = _ministopUnitOfWork.InvoiceRepository.Find(x => x.InvoiceID == returnEntity.InvoiceID);
                        if (invoice != null)
                        {
                            decimal increase = (invoiceDetail.UnitPrice - invoiceDetail.DiscountAmount) * d.Quantity;
                            invoice.FinalAmount = (invoice.FinalAmount ?? 0m) + increase;
                            _ministopUnitOfWork.InvoiceRepository.Update(invoice, true);
                        }
                    }

                    // c) Xóa ReturnDetail
                    _ministopUnitOfWork.ReturnDetailRepository.Delete(d, true);
                }

                // 4. Xóa ReturnProduct
                _ministopUnitOfWork.ReturnProductRepository.Delete(returnEntity, true);

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
    }
}
