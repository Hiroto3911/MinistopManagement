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
    public class PriceProposalRepository : GenericRepository<PriceProposal>, IPriceProposalRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public PriceProposalRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public IReadOnlyList<PriceProposal> GetPagedResponse(Expression<Func<PriceProposal, bool>> predicate, int pageNumber, int pageSize)
        {
            return _context.GetTable<PriceProposal>().Where(predicate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        }
    }
}
