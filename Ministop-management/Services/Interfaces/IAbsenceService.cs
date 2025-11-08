using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAbsenceService
    {
        Result<IReadOnlyList<AbsenceDto>> GetAll();
        Result<AbsenceDto> GetAbsenceByID(string id);
        PagedResult<IReadOnlyList<AbsenceDto>> GetAbsence(int pageNumber, int pageSize);
        Result<bool> CreateAbsence(AbsenceDto absenceDto);
        Result<bool> UpdateAbsence(AbsenceDto absenceEdit);
        Result<bool> RemoveAbsence(string absenceId);
    }
}
