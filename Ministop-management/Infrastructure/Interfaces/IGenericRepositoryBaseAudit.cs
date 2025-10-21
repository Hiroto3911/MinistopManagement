using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IGenericRepositoryBaseAudit<TEntity> : IGenericRepository<TEntity> where TEntity : BaseAuditLog
    {
        IReadOnlyList<TEntity> GetAllIsDelete();
        void SoftDelete(TEntity entity, bool hasTransaction = false);
        void SoftDeleteRange(IList<TEntity> entities, bool hasTransaction = false);
    }
}
