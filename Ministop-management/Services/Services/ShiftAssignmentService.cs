using AutoMapper;
using Domain.Entity;
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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ShiftAssignmentService : IShiftAssignmentService
    {
        private readonly IMinistopUnitOfWork _unitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public ShiftAssignmentService(
            IMinistopUnitOfWork unitOfWork,
            IDateTimeService dateTimeService,
            IUserSession userSession,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public Result<IReadOnlyList<ShiftAssignmentDto>> GetAll()
        {
            var entities = _unitOfWork.ShiftAssignmentRepository.GetAll();
            var dtos = _mapper.Map<IReadOnlyList<ShiftAssignmentDto>>(entities);
            return new Result<IReadOnlyList<ShiftAssignmentDto>>(dtos);
        }

        public Result<IReadOnlyList<ShiftAssignmentDto>> GetAllIsDeleted()
        {
            var entities = _unitOfWork.ShiftAssignmentRepository.GetAllIsDelete();
            var dtos = _mapper.Map<IReadOnlyList<ShiftAssignmentDto>>(entities);
            return new Result<IReadOnlyList<ShiftAssignmentDto>>(dtos);
        }

        public Result<ShiftAssignmentDto> GetById(string id)
        {
            try
            {
                var entity = _unitOfWork.ShiftAssignmentRepository.Find(x => x.Id == id && !x.IsDeleted);
                if (entity == null)
                    return new Result<ShiftAssignmentDto>(ErrorCodeEnum.SA_ERR_001); // Không tìm thấy

                var dto = _mapper.Map<ShiftAssignmentDto>(entity);
                return new Result<ShiftAssignmentDto>(dto);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy ShiftAssignment theo ID", ex);
            }
        }

        public PagedResult<IReadOnlyList<ShiftAssignmentDto>> GetPaged(int pageNumber, int pageSize)
        {
            var totalCount = _unitOfWork.ShiftAssignmentRepository.GetCount(x => !x.IsDeleted);
            var entities = _unitOfWork.ShiftAssignmentRepository.GetPagedResponse(pageNumber, pageSize);
            var dtos = _mapper.Map<IReadOnlyList<ShiftAssignmentDto>>(entities);

            return new PagedResult<IReadOnlyList<ShiftAssignmentDto>>(dtos, pageNumber, pageSize, totalCount);
        }

        public Result<bool> Create(ShiftAssignmentDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                // Kiểm tra trùng lặp theo UNIQUE constraint: (EmployeeID, ShiftID, WorkDate)
                var duplicateCheck = CheckDuplicate(dto.EmployeeId, dto.ShiftId, dto.WorkDate);
                if (duplicateCheck.Data)
                    return new Result<bool>(ErrorCodeEnum.SA_ERR_006);

                var id = IdGenerator.CreateID("SA");
                var currentUserId = _userSession.UserId;

                dto.Id = id;
                dto.Created = _dateTimeService.NowUtc;
                dto.CreatedBy = currentUserId;

                var entity = _mapper.Map<ShiftAssignment>(dto);
                var result = _unitOfWork.ShiftAssignmentRepository.Add(entity);

                if (result == null)
                    return new Result<bool>(ErrorCodeEnum.SA_ERR_003); // Thêm thất bại

                _unitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception("Lỗi khi tạo ShiftAssignment", ex);
            }
        }

        public Result<bool> Update(ShiftAssignmentDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var entity = _unitOfWork.ShiftAssignmentRepository.Find(x => x.Id == dto.Id && !x.IsDeleted);

                if (entity == null)
                    return new Result<bool>(ErrorCodeEnum.SA_ERR_001);

                var duplicateCheck = CheckDuplicate(dto.EmployeeId, dto.ShiftId, dto.WorkDate, dto.Id);
                if (duplicateCheck.Data)
                    return new Result<bool>(ErrorCodeEnum.SA_ERR_006);

                // Cập nhật các trường
                entity.EmployeeID = dto.EmployeeId;
                entity.ShiftID = dto.Id;
                entity.WorkDate = dto.WorkDate;
                entity.Note = dto.Note;
                entity.LastModified = _dateTimeService.NowUtc;
                entity.LastModifiedBy = currentUserId;

                _unitOfWork.ShiftAssignmentRepository.Update(entity, true);
                _unitOfWork.Commit();

                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception("Lỗi khi cập nhật ShiftAssignment", ex);
            }
        }

        public Result<bool> SoftDelete(string id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var entity = _unitOfWork.ShiftAssignmentRepository.Find(x => x.Id == id && !x.IsDeleted);

                if (entity == null)
                    return new Result<bool>(ErrorCodeEnum.SA_ERR_001);

                entity.LastModified = _dateTimeService.NowUtc;
                entity.LastModifiedBy = currentUserId;

                _unitOfWork.ShiftAssignmentRepository.SoftDelete(entity, true);
                _unitOfWork.Commit();

                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception("Lỗi khi xóa mềm ShiftAssignment", ex);
            }
        }

        public Result<bool> Restore(List<string> ids)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var entities = _unitOfWork.ShiftAssignmentRepository.GetAllIsDelete()
                    .Where(x => ids.Contains(x.Id))
                    .ToList();

                if (!entities.Any())
                    return new Result<bool>(ErrorCodeEnum.SA_ERR_001);

                foreach (var entity in entities)
                {
                    entity.IsDeleted = false;
                    entity.LastModified = _dateTimeService.NowUtc;
                    entity.LastModifiedBy = currentUserId;
                }

                _unitOfWork.ShiftAssignmentRepository.UpdateRange(entities, true);
                _unitOfWork.Commit();

                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new Exception("Lỗi khi khôi phục ShiftAssignment", ex);
            }
        }

        public PagedResult<IReadOnlyList<ShiftAssignmentDto>> GetByEmployee(string employeeId, int pageNumber, int pageSize)
        {
            try
            {
                var totalCount = _unitOfWork.ShiftAssignmentRepository.GetCount(x => !x.IsDeleted && x.EmployeeID == employeeId);
                var entities = _unitOfWork.ShiftAssignmentRepository.GetPagedResponse(pageNumber, pageSize)
                    .Where(x => x.EmployeeID == employeeId)
                    .ToList();

                var dtos = _mapper.Map<IReadOnlyList<ShiftAssignmentDto>>(entities);
                return new PagedResult<IReadOnlyList<ShiftAssignmentDto>>(dtos, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy ca theo nhân viên", ex);
            }
        }

        public PagedResult<IReadOnlyList<ShiftAssignmentDto>> GetByShift(string shiftId, int pageNumber, int pageSize)
        {
            try
            {
                var totalCount = _unitOfWork.ShiftAssignmentRepository.GetCount(x => !x.IsDeleted && x.ShiftID == shiftId);
                var entities = _unitOfWork.ShiftAssignmentRepository.GetPagedResponse(pageNumber, pageSize)
                    .Where(x => x.ShiftID == shiftId)
                    .ToList();

                var dtos = _mapper.Map<IReadOnlyList<ShiftAssignmentDto>>(entities);
                return new PagedResult<IReadOnlyList<ShiftAssignmentDto>>(dtos, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy ca theo ca làm việc", ex);
            }
        }

        public PagedResult<IReadOnlyList<ShiftAssignmentDto>> GetByDateRange(DateTime startDate, DateTime endDate, int pageNumber, int pageSize)
        {
            try
            {
                var totalCount = _unitOfWork.ShiftAssignmentRepository.GetCount(x =>
                    !x.IsDeleted &&
                    x.WorkDate >= startDate.Date &&
                    x.WorkDate <= endDate.Date);

                var entities = _unitOfWork.ShiftAssignmentRepository.GetPagedResponse(pageNumber, pageSize)
                    .Where(x => x.WorkDate >= startDate.Date && x.WorkDate <= endDate.Date)
                    .ToList();

                var dtos = _mapper.Map<IReadOnlyList<ShiftAssignmentDto>>(entities);
                return new PagedResult<IReadOnlyList<ShiftAssignmentDto>>(dtos, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy ca theo khoảng ngày", ex);
            }
        }

        public Result<bool> Any(Expression<Func<ShiftAssignment, bool>> predicate)
        {
            try
            {
                var exists = _unitOfWork.ShiftAssignmentRepository.Any(predicate);
                return new Result<bool>(exists);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra tồn tại ShiftAssignment", ex);
            }
        }

        public Result<bool> CheckDuplicate(string employeeId, string shiftId, DateTime workDate, string excludeId = null)
        {
            try
            {
                var exists = _unitOfWork.ShiftAssignmentRepository.Any(x =>
                    x.EmployeeID == employeeId &&
                    x.ShiftID == shiftId &&
                    x.WorkDate == workDate &&
                    !x.IsDeleted &&
                    (string.IsNullOrEmpty(excludeId) || x.Id != excludeId));

                return new Result<bool>(exists);
            }
            catch (Exception ex)
            {
                return new Result<bool>(false, message: ex.Message);
            }
        }

        public Result<IReadOnlyList<ShiftAssignmentDto>> GetByEmployeeAndMonth(string employeeId, string monthYear)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeId) || string.IsNullOrWhiteSpace(monthYear))
                    return new Result<IReadOnlyList<ShiftAssignmentDto>>(ErrorCodeEnum.SA_ERR_002); // Thiếu tham số

                // Parse tháng năm
                if (!DateTime.TryParseExact(monthYear, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                    return new Result<IReadOnlyList<ShiftAssignmentDto>>(ErrorCodeEnum.SA_ERR_007);

                var startDate = new DateTime(parsedDate.Year, parsedDate.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1); // Ngày cuối tháng

                var entities = _unitOfWork.ShiftAssignmentRepository.GetAll()
                    .Where(x => !x.IsDeleted &&
                                x.EmployeeID == employeeId &&
                                x.WorkDate >= startDate &&
                                x.WorkDate <= endDate)
                    .OrderBy(x => x.WorkDate)
                    .ToList();

                var dtos = _mapper.Map<IReadOnlyList<ShiftAssignmentDto>>(entities);
                return new Result<IReadOnlyList<ShiftAssignmentDto>>(dtos);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy bằng nhân viên và tháng", ex);
            }
        }

    }
}
