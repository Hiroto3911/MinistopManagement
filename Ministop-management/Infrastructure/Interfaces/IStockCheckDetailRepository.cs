using Domain.DTO;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IStockCheckDetailRepository : IGenericRepository<StockCheckDetail>
    {
        IList<StockCheckDetail> GetAll(Expression<Func<StockCheckDetail, bool>> predicate);
        IReadOnlyList<StockCheckDetailDto> GetPagedResponse(Expression<Func<StockCheckDetail, bool>> predicated, int pageNumber, int pageSize);
    }
}
