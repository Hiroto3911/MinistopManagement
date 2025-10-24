using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        IReadOnlyList<Supplier> GetAllIsDelete();
        void SoftDelete(Supplier entity, bool hasTransaction = false);
        void SoftDeleteRange(IList<Supplier> entities, bool hasTransaction = false);
    }
}
