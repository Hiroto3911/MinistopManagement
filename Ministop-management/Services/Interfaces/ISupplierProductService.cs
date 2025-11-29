using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISupplierProductService
    {
        Result<IReadOnlyList<SupplierProductDto>> GetAll();
        Result<SupplierProductDto> GetSupplierProductByID(string id);
        PagedResult<IReadOnlyList<SupplierProductDto>> GetSupplierProduct(string supplierId ,int pageNumber, int pageSize);
        Result<bool> CreateSupplierProduct(SupplierProductDto shiftDto);
        Result<bool> UpdateSupplierProduct(SupplierProductDto shiftEdit);
        Result<bool> RemoveSupplierProduct(string shiftId);
    }
}
