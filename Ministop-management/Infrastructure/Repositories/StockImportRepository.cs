using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StockImportRepository : GenericRepository<StockImport>, IStockImportRepository
    {
        public StockImportRepository(MinistopDataContextDataContext context) : base(context)
        {
        }
    }
}
