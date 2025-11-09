using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReturnDetailRepository : GenericRepository<ReturnDetail>, IReturnDetailRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public ReturnDetailRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
    }
}
