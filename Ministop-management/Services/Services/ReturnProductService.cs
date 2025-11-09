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
using Unity;

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
                var InvoiceEntity = _ministopUnitOfWork.ReturnProductRepository.Find(x => x.InvoiceID == id);
                if (InvoiceEntity == null)
                {
                    return new Result<ReturnProductDto>(ErrorCodeEnum.IVD_ERR_001);//chua sua error
                }
                var result = _mapper.Map<ReturnProductDto>(InvoiceEntity);
                return new Result<ReturnProductDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedResult<IReadOnlyList<ReturnProductDto>> GetReturnProduct(string storeID, int pageNumber, int pageSize)
        {
            // Lọc theo invoiceID nếu truyền, ngược lại trả tất cả phân trang
            var totalCount = _ministopUnitOfWork.ReturnProductRepository.GetCount(x => x.StoreID == storeID);
            var returnEntity = _ministopUnitOfWork.ReturnProductRepository.GetPagedResponse((x => x.StoreID == storeID), pageNumber, pageSize);
            var dto = _mapper.Map<IReadOnlyList<ReturnProductDto>>(returnEntity);
            return new PagedResult<IReadOnlyList<ReturnProductDto>>(dto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreateReturnProduct(ReturnProductDto returnProductDto)
        {
            // Lưu ý: mình giả định returnProductDto bao gồm một collection ReturnDetails
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                // 1. Kiểm tra hóa đơn gốc có tồn tại và ở trạng thái được phép (giống InvoiceDetailService logic)
                var invoice = _ministopUnitOfWork.InvoiceRepository.Any(x => x.InvoiceID == returnProductDto.InvoiceId && x.InvoiceDate.Day == returnProductDto.ReturnDate.Day && x.InvoiceDate.Month == returnProductDto.ReturnDate.Month && x.InvoiceDate.Year == returnProductDto.ReturnDate.Year);
                if (!invoice)
                {
                    return new Result<bool>(ErrorCodeEnum.RTP_ERR_003);
                }

                // 2. Sinh ReturnID
                var returnId = IdGenerator.CreateID("RTP");
                // Set ID vào DTO (nếu property tên ReturnId)
                returnProductDto.ReturnId = returnId;

                // 3. Map ReturnProductDto -> ReturnProduct entity
                var returnEntity = _mapper.Map<ReturnProduct>(returnProductDto);
                if (returnEntity == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.RTP_ERR_003);
                }

                var addedReturn = _ministopUnitOfWork.ReturnProductRepository.Add(returnEntity);
                if (addedReturn == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.RTP_ERR_003);
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
        public Result<bool> UpdateReturnProduct(string returnID)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var invoice = _ministopUnitOfWork.InvoiceRepository.Find(x => x.InvoiceID == returnID);
                if (invoice == null)
                {
                    return new Result<bool>(ErrorCodeEnum.RTP_ERR_004);
                }
                var returnProductDto = _ministopUnitOfWork.ReturnProductRepository.Find(x=> x.ReturnID == returnID);
                if (returnProductDto == null)
                {
                    return new Result<bool>(ErrorCodeEnum.RTP_ERR_004);
                }
                decimal invoiceDecreaseAmount = 0m;
                var returnDetailsDto = _ministopUnitOfWork.ReturnDetailRepository.GetAll(x => x.ReturnID == returnID);
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
                            return new Result<bool>(ErrorCodeEnum.RTP_ERR_004);
                        }

           

                        // Lấy stockDetail của cửa hàng và product
                        var stockDetail = _ministopUnitOfWork.StockDetailRepository.Find(
                            x => x.StoreID == _userSession.IdStore && x.ProductID == returnDetailEntity.ProductID
                        );

                        if (stockDetail == null)
                        {
                            // Nếu kho không có thì tạo lỗi (hoặc có thể tạo dòng kho mới tùy nghiệp vụ)
                            _ministopUnitOfWork.Rollback();
                            return new Result<bool>(ErrorCodeEnum.RTP_ERR_004);
                        }

                        // 6.a Cập nhật kho: tăng lại số lượng trả về kho
                        stockDetail.Quantity += returnDetailEntity.Quantity;
                        stockDetail.LastUpdate = DateTime.UtcNow.ToLocalTime();
                        var sd = _mapper.Map<StockDetail>(stockDetail);
                        _ministopUnitOfWork.StockDetailRepository.Update(sd);

                        // 6.b Cập nhật InvoiceDetail: trừ quantity
                        var invoiceDetail = _ministopUnitOfWork.InvoiceDetailRepository
                            .Find(x => x.InvoiceID == returnProductDto.InvoiceID && x.ProductID == returnDetailEntity.ProductID);

                        if (invoiceDetail != null)
                        {
                            // Nếu trả nhiều hơn tồn tại trong invoiceDetail => lỗi
                            if (returnDetailEntity.Quantity > invoiceDetail.Quantity)
                            {
                                _ministopUnitOfWork.Rollback();
                                return new Result<bool>(ErrorCodeEnum.RTP_ERR_004, "So luong trả nhiều hơn tồn tại trong invoiceDetail");
                            }

                            // Giảm quantity trong InvoiceDetail
                            invoiceDetail.Quantity -= returnDetailEntity.Quantity;

                            // Cập nhật các giá liên quan nếu cần (unit price, discount, final price)
                            // Ở đây mình giả định UnitPrice vẫn giữ nguyên; nếu bạn muốn recalc discount, cần logic thêm.
                            invoiceDetail.FinalUnitPrice = invoiceDetail.UnitPrice - invoiceDetail.DiscountAmount;

                            _ministopUnitOfWork.InvoiceDetailRepository.Update(invoiceDetail, true);

                            // Tính tiền giảm của invoice để cập nhật Invoice.FinalAmount
                            // Sử dụng UnitPrice * quantity (bạn có thể sửa nếu muốn dùng RefundAmount)
                            invoiceDecreaseAmount += (invoiceDetail.UnitPrice * returnDetailEntity.Quantity  ) - invoiceDetail.DiscountAmount;
                        }
                        else
                        {
                            // Nếu không tìm thấy invoiceDetail (sản phẩm không có trong hóa đơn) => rollback
                            _ministopUnitOfWork.Rollback();
                            return new Result<bool>(ErrorCodeEnum.RTP_ERR_004, "không tìm thấy invoiceDetail");
                        }
                        // 7. Cập nhật Invoice.FinalAmount giảm theo invoiceDecreaseAmount
                        if (invoiceDecreaseAmount != 0m)
                        {
                            invoice.FinalAmount = (invoice.FinalAmount ?? 0m) - invoiceDecreaseAmount;
                            _ministopUnitOfWork.InvoiceRepository.Update(invoice, true);
                        }
                        // 6.c Lưu ReturnDetail
                        _ministopUnitOfWork.ReturnDetailRepository.Update(returnDetailEntity);
                       
                    }
                }

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch
            {
                _ministopUnitOfWork.Rollback();
                throw;
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
                    return new Result<bool>(ErrorCodeEnum.RTP_ERR_001);
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
