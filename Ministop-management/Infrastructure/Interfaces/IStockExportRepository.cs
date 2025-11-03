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
    public interface IStockExportRepository : IGenericRepository<StockExport>
    {
        IReadOnlyList<StockExportDto> GetPagedResponse(Expression<Func<StockExport, bool>> predicated, int pageNumber, int pageSize);
    }
}
