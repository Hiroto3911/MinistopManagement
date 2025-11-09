using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISalaryService
    {
        Result<IReadOnlyList<SalaryDto>> GetAll();
        Result<SalaryDto> GetSalaryByID(string id);
        PagedResult<IReadOnlyList<SalaryDto>> GetPaged(int pageNumber, int pageSize);
        Result<bool> CreateSalary(SalaryDto dto);
        Result<bool> UpdateSalary(SalaryDto dto);
        Result<bool> RemoveSalary(string salaryId);
        Result<IReadOnlyList<SalaryDto>> GetByContract(string contractId);
        Result<IReadOnlyList<SalaryDto>> GetByMonthYear(string monthYear); // Ví dụ: "2025-03"
    }
}
