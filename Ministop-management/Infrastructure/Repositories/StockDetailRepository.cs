using Domain.DTO;
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
    public class StockDetailRepository : GenericRepository<StockDetail>, IStockDetailRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public StockDetailRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public virtual IReadOnlyList<StockDetailDto> GetPagedResponse(Expression<Func<StockDetail, bool>> predicated, int pageNumber, int pageSize)
        {
            var query = from ct in _context.GetTable<StockDetail>().Where(predicated)
                        join sp in _context.GetTable<Product>() on ct.ProductID equals sp.ProductID
                        select new StockDetailDto
                        {
                            StockDetailId = ct.StockDetailID,
                            StoreId = ct.StoreID,
                            ProductName = sp.ProductName,
                            ProductId = ct.ProductID,
                            LastUpdate = ct.LastUpdate,
                            Price = ct.Price,
                            Quantity = ct.Quantity
                        };
            return query.Skip((int)((pageNumber - 1) * pageSize)).Take(pageSize).OrderByDescending(x => x.LastUpdate).ToList();
        }
    }
}
