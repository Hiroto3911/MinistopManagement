using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AbsenceRepository : GenericRepository<Absence>, IAbsenceRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public AbsenceRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
    }
}
