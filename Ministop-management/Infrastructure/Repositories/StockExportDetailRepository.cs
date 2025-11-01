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
    public class StockExportDetailRepository : GenericRepository<StockExportDetail>, IStockExportDetailRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public StockExportDetailRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public virtual IReadOnlyList<StockExportDetailDto> GetPagedResponse(Expression<Func<StockExportDetail, bool>> predicated, int pageNumber, int pageSize)
        {
            var query = from xh in _context.GetTable<StockExportDetail>().Where(predicated)
                        join sp in _context.GetTable<Product>() on xh.ProductID equals sp.ProductID
                        select new StockExportDetailDto
                        {
                            Id = xh.Id,
                            ExportId = xh.ExportID,
                            ProductName = sp.ProductName,
                            ProductId = sp.ProductID,
                            Quantity = xh.Quantity,
                            UnitPrice = xh.UnitPrice,
                            Total = xh.Quantity * xh.UnitPrice,
                        };
            return query.Skip((int)((pageNumber - 1) * pageSize)).Take(pageSize).OrderByDescending(x => x.Total).ToList();
        }
    }
}
