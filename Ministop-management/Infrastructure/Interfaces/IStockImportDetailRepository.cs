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
    public interface IStockImportDetailRepository : IGenericRepository<StockImportDetail>
    {
        IList<StockImportDetail> GetAll(Expression<Func<StockImportDetail, bool>> predicate);
        IReadOnlyList<StockImportDetailDto> GetPagedResponse(Expression<Func<StockImportDetail, bool>> predicated, int pageNumber, int pageSize);
    }
}
