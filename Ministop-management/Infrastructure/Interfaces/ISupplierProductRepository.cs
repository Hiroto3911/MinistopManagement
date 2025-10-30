using Domain.DTO;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ISupplierProductRepository : IGenericRepository<SupplierProduct>
    {
        IReadOnlyList<SupplierProduct> GetPagedResponse(string supplierId, int pageNumber, int pageSize);
        IReadOnlyList<SupplierProductDto> GetPagedResponse(Expression<Func<SupplierProduct, bool>> predicate, int pageNumber, int pageSize);
    }
}
