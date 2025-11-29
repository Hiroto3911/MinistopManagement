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
    public class PromotionProductService : IPromotionProductService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public PromotionProductService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public Result<IReadOnlyList<PromotionProductDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.PromotionProductRepository.GetAll();

            // Map sang Domain.Entity.Shift
            var list = _mapper.Map<IReadOnlyList<PromotionProductDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<PromotionProductDto>>(list);
        }


        public Result<PromotionProductDto> GetPromotionProductByID(string id)
        {
            try
            {
                var PromotionProductEntity = _ministopUnitOfWork.PromotionProductRepository.Find(x => x.Id == id);
                if (PromotionProductEntity == null)
                {
                    return new Result<PromotionProductDto>(ErrorCodeEnum.PRD_ERR_001);
                }
                var result = _mapper.Map<PromotionProductDto>(PromotionProductEntity);
                return new Result<PromotionProductDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<PromotionProductDto>> GetPromotionProduct(string promotionId, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.PromotionProductRepository 
                .GetCount(x => x.PromotionID == promotionId);
            var promotionProductEntities = _ministopUnitOfWork.PromotionProductRepository
                .GetPagedResponse(x => x.PromotionID == promotionId, pageNumber, pageSize);
            return new PagedResult<IReadOnlyList<PromotionProductDto>>(
                promotionProductEntities,
                pageNumber,
                pageSize,
                totalCount
            );
        }

        public Result<bool> CreatePromotionProduct(PromotionProductDto PromotionProductDto)
        {
            try
            {
                //var promotion = _ministopUnitOfWork.PromotionRepository.Find(x => x.PromotionID == PromotionProductDto.PromotionId);
                //var checkPriority = _ministopUnitOfWork.PromotionRepository.Any(x =>  x.PromotionID != promotion.PromotionID && !x.IsDeleted && x.Status == false && x.Priority == promotion.Priority);
                //if (checkPriority)
                //{
                //    return new Result<bool>(ErrorCodeEnum.PRD_ERR_006, $"San pham nay đã được áp dụng ưu tiên {promotion.Priority} ");
                //}
                var promotion = _ministopUnitOfWork.PromotionRepository
    .Find(x => x.PromotionID == PromotionProductDto.PromotionId);

                var checkPriority = (
                    from p in _ministopUnitOfWork.PromotionRepository.GetAll()
                    join pp in _ministopUnitOfWork.PromotionProductRepository.GetAll()
                        on p.PromotionID equals pp.PromotionID
                    where
                        pp.ProductID == PromotionProductDto.ProductId &&  
                        p.PromotionID != promotion.PromotionID &&         
                        !p.IsDeleted &&                                   
                        p.Status == false &&                              
                        p.Priority == promotion.Priority                  
                    select p
                ).Any();

                if (checkPriority)
                {
                    return new Result<bool>(
                        ErrorCodeEnum.PRD_ERR_006,
                        $"Sản phẩm này đã nằm trong chương trình khuyến mãi khác đang hoạt động có ưu tiên {promotion.Priority}"
                    );
                }

                var isduplicate = _ministopUnitOfWork.PromotionProductRepository.Any(x => x.ProductID == PromotionProductDto.ProductId && x.PromotionID == PromotionProductDto.PromotionId);
                if (isduplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.PRD_ERR_006);
                }
                var PromotionProductId = IdGenerator.CreateID("PRP");
                var currentUserId = _userSession.UserId;
                PromotionProductDto.Id = PromotionProductId;

                var PromotionProductEntity = _mapper.Map<Promotion_Product>(PromotionProductDto);
                var succeeded = _ministopUnitOfWork.PromotionProductRepository.Add(PromotionProductEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PRD_ERR_003);
                }
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Result<bool> UpdatePromotionProduct(PromotionProductDto PromotionProductEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var promotionProductEntity = _ministopUnitOfWork.PromotionProductRepository.Find(x => x.Id == PromotionProductEdit.Id);
                if (promotionProductEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PRD_ERR_001);
                }
                promotionProductEntity.DiscountAmount = PromotionProductEdit.DiscountAmount;
                promotionProductEntity.MinQuantity = PromotionProductEdit.MinQuantity;
                promotionProductEntity.Note = PromotionProductEdit.Note;

                _ministopUnitOfWork.PromotionProductRepository.Update(promotionProductEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemovePromotionProduct(string PromotionProductId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var PromotionProductEntity = _ministopUnitOfWork.PromotionProductRepository.Find(x => x.Id == PromotionProductId);
                if (PromotionProductEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PRD_ERR_001);
                }
                _ministopUnitOfWork.PromotionProductRepository.Delete(PromotionProductEntity, true);
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
