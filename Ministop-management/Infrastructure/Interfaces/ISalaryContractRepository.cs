using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ISalaryContractRepository : IGenericRepository<SalaryContract>
    {
        IReadOnlyList<SalaryContract> GetAllIsDelete();
        void SoftDelete(SalaryContract entity, bool hasTransaction = false);
        void SoftDeleteRange(IList<SalaryContract> entities, bool hasTransaction = false);
        SalaryContract GetCurrentContract(string employeeId);
    }
}
