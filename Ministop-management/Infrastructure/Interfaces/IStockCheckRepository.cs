using Domain.Entity;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IStockCheckRepository : IGenericRepository<StockCheck>
    {
        IReadOnlyList<StockCheckDto> GetPagedResponse(Expression<Func<StockCheck, bool>> predicated, int pageNumber, int pageSize);
    }
}
