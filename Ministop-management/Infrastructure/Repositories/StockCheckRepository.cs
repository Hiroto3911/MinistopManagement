using Domain.DTO;
using Domain.Entity;
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
    public class StockCheckRepository : GenericRepository<StockCheck>, IStockCheckRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public StockCheckRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public virtual IReadOnlyList<StockCheckDto> GetPagedResponse(Expression<Func<StockCheck, bool>> predicated, int pageNumber, int pageSize)
        {
            var query = from nh in _context.GetTable<StockCheck>().Where(predicated)
                        join nv in _context.GetTable<Employee>() on nh.EmployeeID equals nv.EmployeeID
                        select new StockCheckDto
                        {
                            CheckId = nh.CheckID,
                            StoreId = nh.StoreID,
                            EmployeeId = nv.EmployeeID,
                            EmployeeName = nv.FullName,
                            CheckDate = nh.CheckDate,
                            Status = nh.Status

                        };
            return query.Skip((int)((pageNumber - 1) * pageSize)).Take(pageSize).OrderByDescending(x => x.CheckDate).ToList();
        }
    }
}
