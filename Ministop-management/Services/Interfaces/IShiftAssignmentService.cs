using Domain.Entity;
using Infrastructure.Data;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IShiftAssignmentService
    {
        Result<IReadOnlyList<ShiftAssignmentDto>> GetAll();
        Result<IReadOnlyList<ShiftAssignmentDto>> GetAllIsDeleted();
        Result<ShiftAssignmentDto> GetById(string id);
        PagedResult<IReadOnlyList<ShiftAssignmentDto>> GetPaged(int pageNumber, int pageSize);
        Result<bool> Create(ShiftAssignmentDto dto);
        Result<bool> Update(ShiftAssignmentDto dto);
        Result<bool> SoftDelete(string id);
        Result<bool> Restore(List<string> ids);
        PagedResult<IReadOnlyList<ShiftAssignmentDto>> GetByEmployee(string employeeId, int pageNumber, int pageSize);
        PagedResult<IReadOnlyList<ShiftAssignmentDto>> GetByShift(string shiftId, int pageNumber, int pageSize);
        PagedResult<IReadOnlyList<ShiftAssignmentDto>> GetByDateRange(DateTime startDate, DateTime endDate, int pageNumber, int pageSize);
        Result<bool> Any(Expression<Func<ShiftAssignment, bool>> predicate);
        Result<bool> CheckDuplicate(string employeeId, string shiftId, DateTime workDate, string excludeId = null);
        Result<IReadOnlyList<ShiftAssignmentDto>> GetByEmployeeAndMonth(string employeeId, string monthYear);

        Result<bool> CreateBatch(List<ShiftAssignmentDto> dtos);
    }
}
