using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISalaryContractService
    {
        Result<IReadOnlyList<SalaryContractDto>> GetAll();
        Result<IReadOnlyList<SalaryContractDto>> GetAllIsDeleted();
        Result<SalaryContractDto> GetById(string contractId);
        Result<SalaryContractDto> GetCurrentContractByEmployeeId(string employeeId);
        PagedResult<IReadOnlyList<SalaryContractDto>> GetPaged(int pageNumber, int pageSize);
        PagedResult<IReadOnlyList<SalaryContractDto>> GetByEmployeeId(string employeeId, int pageNumber, int pageSize);
        Result<bool> Create(SalaryContractDto dto);
        Result<bool> Update(SalaryContractDto dto);
        Result<bool> SoftDelete(string contractId);
        Result<bool> Restore(List<string> contractIds);
        Result<bool> AnyContractForEmployee(string employeeId);
    }
}
