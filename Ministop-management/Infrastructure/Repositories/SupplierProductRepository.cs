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
    public class SupplierProductRepository : GenericRepository<SupplierProduct>, ISupplierProductRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public SupplierProductRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public  IReadOnlyList<SupplierProduct> GetPagedResponse(string supplierId,int pageNumber, int pageSize)
        {
            return _context.GetTable<SupplierProduct>()
                .Where(x =>x.SupplierID == supplierId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)              
                .ToList();
        }
    }

}
