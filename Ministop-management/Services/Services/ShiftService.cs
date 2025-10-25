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
    public class ShiftService : IShiftService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public ShiftService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public Result<IReadOnlyList<ShiftDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.ShiftRepository.GetAll();

            // Map sang Domain.Entity.Shift
            var list = _mapper.Map<IReadOnlyList<ShiftDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<ShiftDto>>(list);
        }

       
        public Result<ShiftDto> GetShiftByID(string id)
        {
            try
            {
                var ShiftEntity = _ministopUnitOfWork.ShiftRepository.Find(x => x.ShiftID == id);
                if (ShiftEntity == null)
                {
                    return new Result<ShiftDto>(ErrorCodeEnum.STR_ERR_001);
                }
                var result = _mapper.Map<ShiftDto>(ShiftEntity);
                return new Result<ShiftDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<ShiftDto>> GetShift(int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.ShiftRepository.GetCount();
            var ShiftsEntity = _ministopUnitOfWork.ShiftRepository.GetPagedResponse(pageNumber, pageSize);
            var ShiftsDto = _mapper.Map<IReadOnlyList<ShiftDto>>(ShiftsEntity);

            return new PagedResult<IReadOnlyList<ShiftDto>>(ShiftsDto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreateShift(ShiftDto ShiftDto)
        {
            try
            {
                var isDuplicate = _ministopUnitOfWork.ShiftRepository.Any(x => x.ShiftName == ShiftDto.ShiftName);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_006);
                }
                var ShiftId = IdGenerator.CreateID("SFT");
                var currentUserId = _userSession.UserId;
                ShiftDto.ShiftId = ShiftId;
               
                var ShiftEntity = _mapper.Map<Shift>(ShiftDto);
                var succeeded = _ministopUnitOfWork.ShiftRepository.Add(ShiftEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      
        public Result<bool> UpdateShift(ShiftDto ShiftEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var isDuplicate = _ministopUnitOfWork.ShiftRepository.Any(x => x.ShiftName == ShiftEdit.ShiftName && x.ShiftID != ShiftEdit.ShiftId);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_006);
                }
                var currentUserId = _userSession.UserId;
                var ShiftEntity = _ministopUnitOfWork.ShiftRepository.Find(x => x.ShiftID == ShiftEdit.ShiftId);
                if (ShiftEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_001);
                }
                ShiftEntity.ShiftName = ShiftEdit.ShiftName;
                ShiftEntity.StartTime = ShiftEdit.StartTime;
                ShiftEntity.EndTime = ShiftEdit.EndTime;

                _ministopUnitOfWork.ShiftRepository.Update(ShiftEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemoveShift(string ShiftId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;
                var ShiftEntity = _ministopUnitOfWork.ShiftRepository.Find(x => x.ShiftID == ShiftId);
                if (ShiftEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_001);
                }
                _ministopUnitOfWork.ShiftRepository.Delete(ShiftEntity, true);
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
