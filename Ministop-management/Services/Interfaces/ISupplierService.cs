using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISupplierService
    {
        Result<bool> CreateSupplier(SupplierDto SupplierDto);
        Result<IReadOnlyList<SupplierDto>> GetAllSupplier();
        Result<IReadOnlyList<SupplierDto>> GetAllSupplierIsDelete();
        PagedResult<IReadOnlyList<SupplierDto>> GetSupplier(int pageNumber, int pageSize);
        Result<SupplierDto> GetSupplierByID(string id);
        Result<bool> RemoveSupplier(string SupplierId);
        Result<bool> RestoreSupplier(List<string> listRestoreId);
        Result<bool> UpdateSupplier(SupplierDto SupplierDto);
    }
}
