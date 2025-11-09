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
    public class ReturnProductRepository : GenericRepository<ReturnProduct>, IReturnProductRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public ReturnProductRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public IReadOnlyList<ReturnProduct> GetPagedResponse(Expression<Func<ReturnProduct, bool>> predicate, int pageNumber, int pageSize)
        {
            return _context.GetTable<ReturnProduct>().Where(predicate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        }
    }
}
