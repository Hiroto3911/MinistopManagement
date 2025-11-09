using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IReturnDetailRepository : IGenericRepository<ReturnDetail>
    {
        IReadOnlyList<ReturnDetail> GetPagedResponse(Expression<Func<ReturnDetail, bool>> predicate, int pageNumber, int pageSize);
    }
}
