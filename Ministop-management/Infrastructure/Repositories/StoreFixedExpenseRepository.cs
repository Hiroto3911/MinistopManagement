using Domain.DTO;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StoreFixedExpenseRepository : GenericRepositoryBaseAudit<StoreFixedExpenseDto>, IStoreFixedExpenseRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public StoreFixedExpenseRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
    }
}
