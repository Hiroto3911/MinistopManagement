using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StockHistoryRepository : GenericRepository<StockHistory>, IStockHistoryRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public StockHistoryRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public virtual IReadOnlyList<StockHistory> GetPagedResponse(Expression<Func<StockHistory, bool>> predicated, int pageNumber, int pageSize)
        {
            return _context.GetTable<StockHistory>()
                           .Where(predicated)
                           .Skip((int)((pageNumber - 1) * pageSize))
                           .Take(pageSize).
                            OrderByDescending(x => x.ChangeType)
                           .ToList();

        }
    }
}
