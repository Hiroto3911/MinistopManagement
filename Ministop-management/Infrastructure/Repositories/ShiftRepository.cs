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
    public class ShiftRepository : GenericRepository<Shift>, IShiftRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public ShiftRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
    }
}
