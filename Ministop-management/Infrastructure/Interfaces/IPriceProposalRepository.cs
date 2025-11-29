using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IPriceProposalRepository : IGenericRepository<PriceProposal>
    {
        IReadOnlyList<PriceProposal> GetPagedResponse(Expression<Func<PriceProposal, bool>> predicate, int pageNumber, int pageSize);
    }
}
