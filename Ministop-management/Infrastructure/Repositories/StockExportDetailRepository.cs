using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StockExportDetailRepository : GenericRepository<StockExportDetail>, IStockExportDetailRepository
    {
        public StockExportDetailRepository(MinistopDataContextDataContext context) : base(context)
        {
        }
    }
}
