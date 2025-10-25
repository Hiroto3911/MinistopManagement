using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Result<bool> CreateProduct(ProductDto ProductDto);
        Result<IReadOnlyList<ProductDto>> GetAll();
        Result<IReadOnlyList<ProductDto>> GetAllProductIsDelete();
        PagedResult<IReadOnlyList<ProductDto>> GetProduct(int pageNumber, int pageSize);
        Result<ProductDto> GetProductByID(string id);
        Result<bool> RemoveProduct(string ProductId);
        Result<bool> RestoreProduct(List<string> listRestoreId);
        Result<bool> UpdateProduct(ProductDto ProductDto);
    }
}
