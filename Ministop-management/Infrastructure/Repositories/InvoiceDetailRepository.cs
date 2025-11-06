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
    public class InvoiceDetailRepository : GenericRepository<InvoiceDetail>, IInvoiceDetailRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public InvoiceDetailRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public IReadOnlyList<InvoiceDetail> GetPagedResponse(Expression<Func<InvoiceDetail, bool>> predicate, int pageNumber, int pageSize)
        {
            return _context.GetTable<InvoiceDetail>().Where(predicate).Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();


        }
    }
}
