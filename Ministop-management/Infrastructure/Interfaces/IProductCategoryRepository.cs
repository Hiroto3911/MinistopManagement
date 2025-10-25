using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IProductCategoryRepository : IGenericRepository<ProductCategory>
    {
        IReadOnlyList<ProductCategory> GetAllIsDelete();
        void SoftDelete(ProductCategory entity, bool hasTransaction = false);
        void SoftDeleteRange(IList<ProductCategory> entities, bool hasTransaction = false);
    }
}
