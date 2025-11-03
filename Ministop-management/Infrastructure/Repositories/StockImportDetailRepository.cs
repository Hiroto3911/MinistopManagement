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
    public class StockImportDetailRepository : GenericRepository<StockImportDetail>, IStockImportDetailRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public StockImportDetailRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public virtual IList<StockImportDetail> GetAll(Expression<Func<StockImportDetail, bool>> predicate)
        {
            return _context.GetTable<StockImportDetail>().Where(predicate).ToList();
        }
        public virtual IReadOnlyList<StockImportDetailDto> GetPagedResponse(Expression<Func<StockImportDetail, bool>> predicated, int pageNumber, int pageSize)
        {
            var query = from nh in _context.GetTable<StockImportDetail>().Where(predicated)
                        join sp in _context.GetTable<Product>() on nh.ProductID equals sp.ProductID
                        select new StockImportDetailDto
                        {
                            Id = nh.Id,
                            ImportId = nh.ImportID,
                            ProductName = sp.ProductName,
                            ProductId = sp.ProductID,
                            Quantity = nh.Quantity,
                            UnitPrice = nh.UnitPrice,
                            Total = nh.Quantity * nh.UnitPrice,
                        };
            return query.Skip((int)((pageNumber - 1) * pageSize)).Take(pageSize).OrderByDescending(x => x.Total).ToList();
        }
    }
}
