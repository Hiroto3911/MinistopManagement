using Domain.DTO;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
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
        public IReadOnlyList<SupplierProduct> GetPagedResponse(string supplierId, int pageNumber, int pageSize)
        {
            return _context.GetTable<SupplierProduct>()
                .Where(x => x.SupplierID == supplierId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public IReadOnlyList<SupplierProductDto> GetPagedResponse(Expression<Func<SupplierProduct, bool>> predicate, int pageNumber, int pageSize)
        {
            var query = from e in _context.GetTable<SupplierProduct>().Where(predicate)
                        join s in _context.GetTable<Product>() on e.ProductID equals s.ProductID
                        select new SupplierProductDto
                        {
                            Id = e.Id,
                            SupplierId = e.SupplierID,
                            ProductId = e.ProductID,
                            ProductName = s.ProductName,
                            SupplyPrice = e.SupplyPrice,
                            Status = e.Status,
                        };
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        }
    }

}
