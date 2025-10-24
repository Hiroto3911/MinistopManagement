using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductCategoryService
    {
        Result<bool> CreateProductCategory(ProductCategoryDto ProductCategoryDto);
        Result<IReadOnlyList<ProductCategoryDto>> GetAll();
        Result<IReadOnlyList<ProductCategoryDto>> GetAllProductCategoryIsDelete();
        PagedResult<IReadOnlyList<ProductCategoryDto>> GetProductCategory(int pageNumber, int pageSize);
        Result<ProductCategoryDto> GetProductCategoryByID(string id);
        Result<bool> RemoveProductCategory(string ProductCategoryId);
        Result<bool> RestoreProductCategory(List<string> listRestoreId);
        Result<bool> UpdateProductCategory(ProductCategoryDto ProductCategoryEdit);
    }
}
