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

namespace Services.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public AbsenceService(
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

        public Result<IReadOnlyList<AbsenceDto>> GetAll()
        {
            var dbList = _ministopUnitOfWork.AbsenceRepository.GetAll();
            var list = _mapper.Map<IReadOnlyList<AbsenceDto>>(dbList);
            return new Result<IReadOnlyList<AbsenceDto>>(list);
        }

        public Result<AbsenceDto> GetAbsenceByID(string id)
        {
            try
            {
                var absenceEntity = _ministopUnitOfWork.AbsenceRepository.Find(x => x.AbsenceID == id);
                if (absenceEntity == null)
                    return new Result<AbsenceDto>(ErrorCodeEnum.ABS_ERR_001);

                var dto = _mapper.Map<AbsenceDto>(absenceEntity);
                return new Result<AbsenceDto>(dto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedResult<IReadOnlyList<AbsenceDto>> GetAbsence(int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.AbsenceRepository.GetCount();
            var entities = _ministopUnitOfWork.AbsenceRepository.GetPagedResponse(pageNumber, pageSize);
            var dtos = _mapper.Map<IReadOnlyList<AbsenceDto>>(entities);
            return new PagedResult<IReadOnlyList<AbsenceDto>>(dtos, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreateAbsence(AbsenceDto absenceDto)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                // Kiểm tra ShiftAssignment tồn tại
                var assignmentExists = _ministopUnitOfWork.ShiftAssignmentRepository.Any(x =>
                    x.EmployeeID == absenceDto.EmployeeId &&
                    x.ShiftID == absenceDto.ShiftId &&
                    x.WorkDate.Date == absenceDto.WorkDate.Date);

                if (!assignmentExists)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.ABS_ERR_002); // Không tồn tại phân công
                }

                // Kiểm tra đã tồn tại nghỉ phép cho phân công này chưa (UNIQUE constraint)
                var isDuplicate = _ministopUnitOfWork.AbsenceRepository.Any(x =>
                    x.EmployeeID == absenceDto.EmployeeId &&
                    x.ShiftID == absenceDto.ShiftId &&
                    x.WorkDate.Date == absenceDto.WorkDate.Date);

                if (isDuplicate)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.ABS_ERR_006); // Đã tồn tại nghỉ phép
                }

                // Tạo ID
                var absenceId = IdGenerator.CreateID("ABS");
                absenceDto.AbsenceId = absenceId;

                var entity = _mapper.Map<Absence>(absenceDto);
                var succeeded = _ministopUnitOfWork.AbsenceRepository.Add(entity);

                if (succeeded == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.ABS_ERR_003); // Tạo thất bại
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

        public Result<bool> UpdateAbsence(AbsenceDto absenceEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var entity = _ministopUnitOfWork.AbsenceRepository.Find(x => x.AbsenceID == absenceEdit.AbsenceId);
                if (entity == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.ABS_ERR_001);
                }

                // Kiểm tra ShiftAssignment (nếu thay đổi)
                if (entity.EmployeeID != absenceEdit.EmployeeId ||
                    entity.ShiftID != absenceEdit.ShiftId ||
                    entity.WorkDate.Date != absenceEdit.WorkDate.Date)
                {
                    var assignmentExists = _ministopUnitOfWork.ShiftAssignmentRepository.Any(x =>
                        x.EmployeeID == absenceEdit.EmployeeId &&
                        x.ShiftID == absenceEdit.ShiftId &&
                        x.WorkDate.Date == absenceEdit.WorkDate.Date);

                    if (!assignmentExists)
                    {
                        _ministopUnitOfWork.Rollback();
                        return new Result<bool>(ErrorCodeEnum.ABS_ERR_002);
                    }

                    // Kiểm tra trùng (trừ bản thân)
                    var isDuplicate = _ministopUnitOfWork.AbsenceRepository.Any(x =>
                        x.EmployeeID == absenceEdit.EmployeeId &&
                        x.ShiftID == absenceEdit.ShiftId &&
                        x.WorkDate.Date == absenceEdit.WorkDate.Date &&
                        x.AbsenceID != absenceEdit.AbsenceId);

                    if (isDuplicate)
                    {
                        _ministopUnitOfWork.Rollback();
                        return new Result<bool>(ErrorCodeEnum.ABS_ERR_006);
                    }
                }

                // Cập nhật
                entity.EmployeeID = absenceEdit.EmployeeId;
                entity.ShiftID = absenceEdit.ShiftId;
                entity.WorkDate = absenceEdit.WorkDate;
                entity.IsLeaveOfAbsence = absenceEdit.IsLeaveOfAbsence;
                entity.Reason = absenceEdit.Reason;
                entity.IsPaid = absenceEdit.IsPaid;

                _ministopUnitOfWork.AbsenceRepository.Update(entity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemoveAbsence(string absenceId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var entity = _ministopUnitOfWork.AbsenceRepository.Find(x => x.AbsenceID == absenceId);
                if (entity == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.ABS_ERR_001);
                }

                _ministopUnitOfWork.AbsenceRepository.Delete(entity, true);
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