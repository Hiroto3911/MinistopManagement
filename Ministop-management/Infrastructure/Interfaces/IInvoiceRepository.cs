using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        IReadOnlyList<Invoice> GetPagedResponse(Expression<Func<Invoice, bool>> predicate, int pageNumber, int pageSize);
    }
}
