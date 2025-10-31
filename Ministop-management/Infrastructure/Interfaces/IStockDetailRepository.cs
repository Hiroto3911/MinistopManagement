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
    public interface IStockDetailRepository : IGenericRepository<StockDetail>
    {
        IReadOnlyList<StockDetailDto> GetPagedResponse(Expression<Func<StockDetail, bool>> predicated, int pageNumber, int pageSize);
    }
}
