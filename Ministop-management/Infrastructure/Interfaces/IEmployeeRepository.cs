using Domain.DTO;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IReadOnlyList<Employee> GetAllIsDelete();
        void SoftDelete(Employee entity, bool hasTransaction = false);
        void SoftDeleteRange(IList<Employee> entities, bool hasTransaction = false);
    }
}
