using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IShiftService
    {
        Result<IReadOnlyList<ShiftDto>> GetAll();
        Result<ShiftDto> GetShiftByID(string id);
        PagedResult<IReadOnlyList<ShiftDto>> GetShift(int pageNumber, int pageSize);
        Result<bool> CreateShift(ShiftDto shiftDto);
        Result<bool> UpdateShift(ShiftDto shiftEdit);
        Result<bool> RemoveShift(string shiftId);
    }
}
