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
    public class StockCheckDetailRepository : GenericRepository<StockCheckDetail>, IStockCheckDetailRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public StockCheckDetailRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public virtual IReadOnlyList<StockCheckDetailDto> GetPagedResponse(Expression<Func<StockCheckDetail, bool>> predicated, int pageNumber, int pageSize)
        {
            var query = from nh in _context.GetTable<StockCheckDetail>().Where(predicated)
                        join sp in _context.GetTable<Product>() on nh.ProductID equals sp.ProductID
                        select new StockCheckDetailDto
                        {
                            Id = nh.Id,
                            CheckId = nh.CheckID,
                            ProductName = sp.ProductName,
                            ProductId = sp.ProductID,
                            QuantityActual = nh.QuantityActual,
                            QuantitySystem = nh.QuantitySystem,
                            QuantityVariance = (int)(nh.QuantitySystem - nh.QuantityActual),
                            Note = nh.Note,
                        };
            return query.Skip((int)((pageNumber - 1) * pageSize)).Take(pageSize).OrderByDescending(x => x.QuantityVariance).ToList();
        }
    }
}
