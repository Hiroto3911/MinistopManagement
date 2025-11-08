using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IShiftAssignmentRepository : IGenericRepository<ShiftAssignment>
    {
        IReadOnlyList<ShiftAssignment> GetAllIsDelete();
        void SoftDelete(ShiftAssignment entity, bool hasTransaction = false);
        void SoftDeleteRange(IList<ShiftAssignment> entities, bool hasTransaction = false);
    }
}
