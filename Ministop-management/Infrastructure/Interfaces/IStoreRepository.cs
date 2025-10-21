
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
        IReadOnlyList<Store> GetAllIsDelete();
        void SoftDelete(Store entity, bool hasTransaction = false);
        void SoftDeleteRange(IList<Store> entities, bool hasTransaction = false);
    }
}
