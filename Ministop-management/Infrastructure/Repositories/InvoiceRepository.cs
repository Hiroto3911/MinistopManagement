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
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public InvoiceRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public IReadOnlyList<Invoice> GetPagedResponse(Expression<Func<Invoice, bool>> predicate, int pageNumber, int pageSize)
        {
            return _context.GetTable<Invoice>().Where(predicate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        }
    }
}
