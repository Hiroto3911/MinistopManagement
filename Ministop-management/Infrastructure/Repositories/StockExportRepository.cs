using Domain.DTO;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StockExportRepository : GenericRepository<StockExport>, IStockExportRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public StockExportRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public virtual IReadOnlyList<StockExportDto> GetPagedResponse(Expression<Func<StockExport, bool>> predicated, int pageNumber, int pageSize)
        {
            var query = from xh in _context.GetTable<StockExport>().Where(predicated)
                        join nv in _context.GetTable<Employee>() on xh.EmployeeID equals nv.EmployeeID
                        select new StockExportDto
                        {
                         
                            ExportId = xh.ExportID,
                            StoreId = xh.StoreID,
                            EmployeeId = nv.EmployeeID,
                            EmployeeName = nv.FullName,
                            TypeExport = xh.TypeExport,
                            ExportDate = xh.ExportDate,
                            Reason = xh.Reason,
                            Status = xh.Status

                        };
            return query.Skip((int)((pageNumber - 1) * pageSize)).Take(pageSize).OrderByDescending(x => x.ExportDate).ToList();
        }
    }
}
