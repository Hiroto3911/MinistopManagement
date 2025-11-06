using Domain.Entity;
using Infrastructure.Interfaces;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISalaryContractAllowanceService
    {
        Result<IReadOnlyList<SalaryContractAllowanceDto>> GetAll();
        Result<SalaryContractAllowanceDto> GetById(string id);
        PagedResult<IReadOnlyList<SalaryContractAllowanceDto>> GetPaged(int pageNumber, int pageSize);

        // Lấy danh sách phụ cấp theo hợp đồng lương
        Result<IReadOnlyList<SalaryContractAllowanceDto>> GetByContractId(string contractId);

        Result<bool> Create(SalaryContractAllowanceDto dto);
        Result<bool> Update(SalaryContractAllowanceDto dto);
        Result<bool> Remove(string id);

        // Xóa tất cả phụ cấp của một hợp đồng (khi xóa hợp đồng)
        Result<bool> RemoveByContractId(string contractId);

        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
