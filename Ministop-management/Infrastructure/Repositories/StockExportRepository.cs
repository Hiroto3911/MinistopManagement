using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StockExportRepository : GenericRepository<StockExport>, IStockExportRepository
    {
        public StockExportRepository(MinistopDataContextDataContext context) : base(context)
        {
        }
    }
}
