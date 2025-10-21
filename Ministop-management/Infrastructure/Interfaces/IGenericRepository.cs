using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity, bool hasTransaction = false);
        bool AddRange(IList<TEntity> entities, bool hasTransaction = false);
        bool All(Expression<Func<TEntity, bool>> predicate);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> AsQueryable();
        void Delete(TEntity entity, bool hasTransaction = false);
        void DeleteRange(IList<TEntity> entities, bool hasTransaction = false);
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        IReadOnlyList<TEntity> GetAll();
        IReadOnlyList<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        int GetCount(Expression<Func<TEntity, bool>> predicate);
        IReadOnlyList<TEntity> GetPagedResponse(int pageNumber, int pageSize);
        void Update(TEntity entity, bool hasTransaction = false);
        void UpdateRange(IList<TEntity> entities, bool hasTransaction = false);
    }
}
