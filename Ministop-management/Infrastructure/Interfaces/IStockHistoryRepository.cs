using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IStockHistoryRepository : IGenericRepository<StockHistory>
    {
        IReadOnlyList<StockHistory> GetPagedResponse(Expression<Func<StockHistory, bool>> predicated, int pageNumber, int pageSize);
    }
}
