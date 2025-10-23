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
    public class AllowanceService : IAllowanceService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public AllowanceService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public Result<IReadOnlyList<AllowanceDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.AllowanceRepository.GetAll();

            // Map sang Domain.Entity.Allowance
            var list = _mapper.Map<IReadOnlyList<AllowanceDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<AllowanceDto>>(list);
        }

        public Result<IReadOnlyList<AllowanceDto>> GetAllAllowanceIsDelete()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.AllowanceRepository.GetAllIsDelete();

            // Map sang Domain.Entity.Allowance
            var list = _mapper.Map<IReadOnlyList<AllowanceDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<AllowanceDto>>(list);
        }
        public Result<AllowanceDto> GetAllowanceByID(string id)
        {
            try
            {
                var AllowanceEntity = _ministopUnitOfWork.AllowanceRepository.Find(x => x.AllowanceID == id);
                if (AllowanceEntity == null)
                {
                    return new Result<AllowanceDto>(ErrorCodeEnum.STR_ERR_001);
                }
                var result = _mapper.Map<AllowanceDto>(AllowanceEntity);
                return new Result<AllowanceDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<AllowanceDto>> GetAllowance(int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.AllowanceRepository.GetCount(x => !x.IsDeleted);
            var AllowancesEntity = _ministopUnitOfWork.AllowanceRepository.GetPagedResponse(pageNumber, pageSize);
            var AllowancesDto = _mapper.Map<IReadOnlyList<AllowanceDto>>(AllowancesEntity);

            return new PagedResult<IReadOnlyList<AllowanceDto>>(AllowancesDto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreateAllowance(AllowanceDto AllowanceDto)
        {
            try
            {
                var isDuplicate = _ministopUnitOfWork.AllowanceRepository.Any(x => x.AllowanceName == AllowanceDto.AllowanceName);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.ALL_ERR_006);
                }
                var AllowanceId = IdGenerator.CreateID("ALL");
                var currentUserId = _userSession.UserId;
                AllowanceDto.AllowanceId = AllowanceId;
                AllowanceDto.Created = _dateTimeService.NowUtc;
                AllowanceDto.CreatedBy = currentUserId;
                var AllowanceEntity = _mapper.Map<Allowance>(AllowanceDto);
                var succeeded = _ministopUnitOfWork.AllowanceRepository.Add(AllowanceEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.ALL_ERR_003);
                }
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result<bool> RestoreAllowance(List<string> listRestoreId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;

                var allowances = _ministopUnitOfWork.AllowanceRepository.GetAllIsDelete();
                var filter = allowances.Where(x => listRestoreId.Contains(x.AllowanceID)).ToList();
                if (filter == null || filter.Count == 0)
                {
                    return new Result<bool>(ErrorCodeEnum.ALL_ERR_001);
                }
                foreach (var item in filter)
                {
                    item.IsDeleted = false;
                    item.LastModified = _dateTimeService.NowUtc;
                    item.LastModifiedBy = currentUserId;
                }
                _ministopUnitOfWork.AllowanceRepository.UpdateRange(filter, true);

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> UpdateAllowance(AllowanceDto allowanceEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var isDuplicate = _ministopUnitOfWork.AllowanceRepository.Any(x => x.AllowanceName == allowanceEdit.AllowanceName && x.AllowanceID != allowanceEdit.AllowanceId);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.ALL_ERR_006);
                }
                var currentUserId = _userSession.UserId;
                var allowanceEntity = _ministopUnitOfWork.AllowanceRepository.Find(x => x.AllowanceID == allowanceEdit.AllowanceId);
                if (allowanceEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.ALL_ERR_001);
                }
                allowanceEntity.AllowanceName = allowanceEdit.AllowanceName;
                allowanceEntity.DefaultAmount = allowanceEdit.DefaultAmount;
                allowanceEntity.LastModified = _dateTimeService.NowUtc;
                allowanceEntity.LastModifiedBy = currentUserId;
                _ministopUnitOfWork.AllowanceRepository.Update(allowanceEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemoveAllowance(string allowanceId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;
                var allowanceEntity = _ministopUnitOfWork.AllowanceRepository.Find(x => x.AllowanceID == allowanceId);
                if (allowanceEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.ALL_ERR_001);
                }
                allowanceEntity.LastModified = _dateTimeService.NowUtc;
                allowanceEntity.LastModifiedBy = currentUserId;
                _ministopUnitOfWork.AllowanceRepository.SoftDelete(allowanceEntity, true);
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
