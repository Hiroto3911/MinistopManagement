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

        public Result<bool> Any(string invoiceID)
        {
            try
            {
                bool isChecked = _ministopUnitOfWork.InvoiceDetailRepository.Any((x => x.InvoiceID == invoiceID));
                return new Result<bool>(isChecked);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public PagedResult<IReadOnlyList<InvoiceDetailDto>> GetInvoiceDetail(string invoiceID, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.InvoiceDetailRepository.GetCount(x => x.InvoiceID == invoiceID);
            var InvoiceDetailEntity = _ministopUnitOfWork.InvoiceDetailRepository.GetPagedResponse((x => x.InvoiceID == invoiceID), pageNumber, pageSize);
            var InvoiceDetailsDto = _mapper.Map<IReadOnlyList<InvoiceDetailDto>>(InvoiceDetailEntity);

            return new PagedResult<IReadOnlyList<InvoiceDetailDto>>(InvoiceDetailsDto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreateInvoiceDetail(InvoiceDetailDto invoiceDetailDto)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var isAvailable = _ministopUnitOfWork.StockDetailRepository.Find(x => x.StoreID == _userSession.IdStore && x.ProductID == invoiceDetailDto.ProductId);
                if (isAvailable.Quantity <= 0)
                {
                    //thong bao da het hang san pham
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }
                var productPrice = isAvailable.Price;
                var isDuplicate = _ministopUnitOfWork.InvoiceDetailRepository.Find(x => x.ProductID == invoiceDetailDto.ProductId && x.InvoiceID == invoiceDetailDto.InvoiceId);
                if (isDuplicate != null)
                {
                    isDuplicate.Quantity += invoiceDetailDto.Quantity;
                    isDuplicate.FinalUnitPrice = productPrice * isDuplicate.Quantity;
                    _ministopUnitOfWork.InvoiceDetailRepository.Update(isDuplicate);
                    _ministopUnitOfWork.Commit();
                    return new Result<bool>(true);
                }
                else
                {
                    var InvoiceDetailId = IdGenerator.CreateID("IVD");
                    invoiceDetailDto.Id = InvoiceDetailId;
                    invoiceDetailDto.FinalUnitPrice = productPrice * invoiceDetailDto.Quantity;
                    var InvoiceDetailEntity = _mapper.Map<InvoiceDetail>(invoiceDetailDto);
                    var succeeded = _ministopUnitOfWork.InvoiceDetailRepository.Add(InvoiceDetailEntity);
                    if (succeeded == null)
                    {
                        return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                    }
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

                // 3. Kiểm tra kho
                var stockDetail = _ministopUnitOfWork.StockDetailRepository.Find(
                    x => x.StoreID == _userSession.IdStore && x.ProductID == existingDetail.ProductID
                );

                if (stockDetail == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }

                // 8. Cập nhật InvoiceDetail
                existingDetail.Quantity = invoiceDetailDto.Quantity;
                existingDetail.UnitPrice = stockDetail.Price;
                existingDetail.FinalUnitPrice = stockDetail.Price * invoiceDetailDto.Quantity;
                _ministopUnitOfWork.InvoiceDetailRepository.Update(existingDetail, true);
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
                // 4. Xóa InvoiceDetail
                _ministopUnitOfWork.InvoiceDetailRepository.Delete(invoiceDetail, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }



        //private PromotionProductDto GetActivePromotion(string productId, int qty)
        //{
        //    var promotion = _ministopUnitOfWork.PromotionProductRepository.GetActivePromotionForProduct(productId, qty);
        //    var entity = _mapper.Map<PromotionProductDto>(promotion);
        //    return entity;
        //}

        public Result<bool> RemoveRangeInvoiceDetailByInvoiceID(string invoiceID)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var invoiceDetailEntity = _ministopUnitOfWork.InvoiceDetailRepository.GetAll((x => x.InvoiceID == invoiceID));
                if (invoiceDetailEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SID_ERR_001);
                }
                _ministopUnitOfWork.InvoiceDetailRepository.DeleteRange(invoiceDetailEntity.ToList(), true);
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
