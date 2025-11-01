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
    public class PromotionService : IPromotionService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public PromotionService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<PromotionDto> GetPromotionByID(string id)
        {
            try
            {
                var PromotionEntity = _ministopUnitOfWork.PromotionRepository.Find(x => x.PromotionID == id);
                if (PromotionEntity == null)
                {
                    return new Result<PromotionDto>(ErrorCodeEnum.PCT_ERR_001);
                }
                var result = _mapper.Map<PromotionDto>(PromotionEntity);
                return new Result<PromotionDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Result<IReadOnlyList<PromotionDto>> GetAllPromotionIsDelete()
        {
            var dbList = _ministopUnitOfWork.PromotionRepository.GetAllIsDelete();
            var list = _mapper.Map<IReadOnlyList<PromotionDto>>(dbList);
            return new Result<IReadOnlyList<PromotionDto>>(list);
        }
        public Result<IReadOnlyList<PromotionDto>> GetAll()
        {
            var dbList = _ministopUnitOfWork.PromotionRepository.GetAll(x => !x.IsDeleted);
            var list = _mapper.Map<IReadOnlyList<PromotionDto>>(dbList);
            return new Result<IReadOnlyList<PromotionDto>>(list);
        }
        public Result<bool> RestorePromotion(List<string> listRestoreId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;

                var promotions = _ministopUnitOfWork.PromotionRepository.GetAllIsDelete();
                var filter = promotions.Where(x => listRestoreId.Contains(x.PromotionID)).ToList();
                if (filter == null || filter.Count == 0)
                {
                    return new Result<bool>(ErrorCodeEnum.PCT_ERR_001);
                }
                foreach (var item in filter)
                {
                    item.IsDeleted = false;
                    item.LastModified = _dateTimeService.NowUtc;
                    item.LastModifiedBy = currentUserId;
                }
                _ministopUnitOfWork.PromotionRepository.UpdateRange(filter, true);

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<PromotionDto>> GetPromotion(int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.PromotionRepository.GetCount(x => !x.IsDeleted);
            var PromotionsEntity = _ministopUnitOfWork.PromotionRepository.GetPagedResponse(pageNumber, pageSize);
            var PromotionDto = _mapper.Map<IReadOnlyList<PromotionDto>>(PromotionsEntity);
            return new PagedResult<IReadOnlyList<PromotionDto>>(PromotionDto, pageNumber, pageSize, totalCount);
        }
        public Result<bool> CreatePromotion(PromotionDto promotionDto)
        {
            try
            {
                var isDuplicate = _ministopUnitOfWork.PromotionRepository.Any(x => x.PromotionID == promotionDto.PromotionId);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.PCT_ERR_006);
                }
                var PromotionId = IdGenerator.CreateID("PRM");
                var currenUserId = _userSession.UserId;
                promotionDto.PromotionId = PromotionId;
                promotionDto.Created = _dateTimeService.NowUtc;
                promotionDto.CreatedBy = currenUserId;
                var PromotionEntity = _mapper.Map<Promotion>(promotionDto);
                var succeeded = _ministopUnitOfWork.PromotionRepository.Add(PromotionEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PCT_ERR_003);
                }
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Result<bool> UpdatePromotion(PromotionDto PromotionEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var isDuplicate = _ministopUnitOfWork.PromotionRepository.Any(x => x.PromotionID != PromotionEdit.PromotionId && x.PromotionName == PromotionEdit.PromotionName);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.PCT_ERR_004);
                }
                var currentUserID = _userSession.UserId;
                var PromotionEntity = _ministopUnitOfWork.PromotionRepository.Find(x => x.PromotionID == PromotionEdit.PromotionId);
                if (PromotionEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.STR_ERR_001);
                }
                PromotionEntity.PromotionName = PromotionEdit.PromotionName;
                PromotionEntity.StartDate = PromotionEdit.StartDate;
                PromotionEntity.EndDate = PromotionEdit.EndDate;
                PromotionEntity.Priority = PromotionEdit.Priority;
                PromotionEntity.Status = PromotionEdit.Status;
                PromotionEntity.LastModified = _dateTimeService.NowUtc;
                PromotionEntity.LastModifiedBy = currentUserID;
                _ministopUnitOfWork.PromotionRepository.Update(PromotionEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public Result<bool> RemovePromotion(string PromotionId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var PromotionEntity = _ministopUnitOfWork.PromotionRepository.Find(x => x.PromotionID == PromotionId);
                if (PromotionEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PCT_ERR_005);
                }
                PromotionEntity.LastModified = _dateTimeService.NowUtc;
                PromotionEntity.LastModifiedBy = currentUserId;
                _ministopUnitOfWork.PromotionRepository.SoftDelete(PromotionEntity, true);
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
