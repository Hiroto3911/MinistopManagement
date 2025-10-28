using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        bool All(Expression<Func<Product, bool>> predicate);
        IReadOnlyList<Product> GetAllIsDelete();
        void SoftDelete(Product entity, bool hasTransaction = false);
        void SoftDeleteRange(IList<Product> entities, bool hasTransaction = false);
    }
}
