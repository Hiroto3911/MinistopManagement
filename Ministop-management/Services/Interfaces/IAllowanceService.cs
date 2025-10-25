using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAllowanceService
    {
        Result<IReadOnlyList<AllowanceDto>> GetAll();
        Result<IReadOnlyList<AllowanceDto>> GetAllAllowanceIsDelete();
        Result<AllowanceDto> GetAllowanceByID(string id);
        PagedResult<IReadOnlyList<AllowanceDto>> GetAllowance(int pageNumber, int pageSize);
        Result<bool> CreateAllowance(AllowanceDto allowanceDto);
        Result<bool> RestoreAllowance(List<string> listRestoreId);
        Result<bool> UpdateAllowance(AllowanceDto allowanceEdit);
        Result<bool> RemoveAllowance(string allowanceId);
    }
}
