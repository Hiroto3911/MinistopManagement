using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StockImportDetailRepository : GenericRepository<StockImportDetail>, IStockImportDetailRepository
    {
        public StockImportDetailRepository(MinistopDataContextDataContext context) : base(context)
        {
        }
    }
}
