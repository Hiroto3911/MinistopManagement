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
    public interface IStockExportDetailRepository : IGenericRepository<StockExportDetail>
    {
        IList<StockExportDetail> GetAll(Expression<Func<StockExportDetail, bool>> predicate);
        IReadOnlyList<StockExportDetailDto> GetPagedResponse(Expression<Func<StockExportDetail, bool>> predicated, int pageNumber, int pageSize);
    }
}
