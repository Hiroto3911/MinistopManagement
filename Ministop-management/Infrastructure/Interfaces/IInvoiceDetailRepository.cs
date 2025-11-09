using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IInvoiceDetailRepository : IGenericRepository<InvoiceDetail>
    {
        IReadOnlyList<InvoiceDetail> GetPagedResponse(Expression<Func<InvoiceDetail, bool>> predicate, int pageNumber, int pageSize);
    }
}
