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
    public class StockImportRepository : GenericRepository<StockImport>, IStockImportRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public StockImportRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public virtual IReadOnlyList<StockImportDto> GetPagedResponse(Expression<Func<StockImport, bool>> predicated, int pageNumber, int pageSize)
        {
            var query = from nh in _context.GetTable<StockImport>().Where(predicated)
                        join nv in _context.GetTable<Employee>() on nh.EmployeeID equals nv.EmployeeID
                        join ncc in _context.GetTable<Supplier>() on nh.SupplierID equals ncc.SupplierID
                        select new StockImportDto
                        {
                            ImportID = nh.ImportID,
                            StoreId = nh.StoreID,
                            SupplierId  = nh.SupplierID,
                            SupplierName = ncc.SupplierName,
                            EmployeeId = nv.EmployeeID,
                            EmployeeName = nv.FullName,
                            ImportDate = nh.ImportDate,
                            Status = nh.Status,
                            Note = nh.Note

                        };
            return query.Skip((int)((pageNumber - 1) * pageSize)).Take(pageSize).OrderByDescending(x => x.ImportDate).ToList();
        }
    }
}
