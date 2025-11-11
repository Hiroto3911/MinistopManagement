using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IReturnProductRepository : IGenericRepository<ReturnProduct>
    {
        IReadOnlyList<ReturnProduct> GetPagedResponse(Expression<Func<ReturnProduct, bool>> predicate, int pageNumber, int pageSize);
    }
}
