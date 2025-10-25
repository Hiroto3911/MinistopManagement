using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IAllowanceRepository : IGenericRepository<Allowance>
    {
        IReadOnlyList<Allowance> GetAllIsDelete();
        void SoftDelete(Allowance entity, bool hasTransaction = false);
        void SoftDeleteRange(IList<Allowance> entities, bool hasTransaction = false);
    }
}
