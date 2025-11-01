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
    public interface IStockImportRepository : IGenericRepository<StockImport>
    {
        IReadOnlyList<StockImportDto> GetPagedResponse(Expression<Func<StockImport, bool>> predicated, int pageNumber, int pageSize);
    }
}
