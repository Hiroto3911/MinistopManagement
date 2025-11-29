using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SalaryContractAllowanceRepository : GenericRepository<SalaryContract_Allowance>, ISalaryContractAllowanceRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public SalaryContractAllowanceRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
    }
}
