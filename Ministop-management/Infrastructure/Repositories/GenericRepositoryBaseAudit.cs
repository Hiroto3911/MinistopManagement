using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    
    public class GenericRepositoryBaseAudit<TEntity> : GenericRepository<TEntity>, IGenericRepositoryBaseAudit<TEntity> where TEntity : BaseAuditLog
    {
        private readonly MinistopDataContextDataContext _context;
        public GenericRepositoryBaseAudit(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }

        public override  IReadOnlyList<TEntity> GetAll()
        {
            return  _context.GetTable<TEntity>().Where(x => !x.IsDeleted).ToList();
        }
        public override  IReadOnlyList<TEntity> GetPagedResponse(int pageNumber, int pageSize)
        {
            return  _context.GetTable<TEntity>()
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Created)
                .ToList();
        }
        public override  bool All(Expression<Func<TEntity, bool>> predicate)
        {
            return  _context.GetTable<TEntity>().Where(x => !x.IsDeleted).All(predicate);
        }
        public override  bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return  _context.GetTable<TEntity>().Where(x => !x.IsDeleted).Any(predicate);
        }
        public override  int GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            return  _context.GetTable<TEntity>().Where(x => !x.IsDeleted).Count(predicate);
        }

        public  void SoftDelete(TEntity entity, bool hasTransaction = false)
        {
            var table = _context.GetTable<TEntity>();
            entity.IsDeleted = true;
            table.Attach(entity, true);
          
            if (!hasTransaction)
            {
                 _context.SubmitChanges();
            }
        }

        public  void SoftDeleteRange(IList<TEntity> entities, bool hasTransaction = false)
        {
            var table = _context.GetTable<TEntity>();
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                table.Attach(entity, true);
            }
            if (!hasTransaction)
            {
                 _context.SubmitChanges();
            }
        }

        public  IReadOnlyList<TEntity> GetAllIsDelete()
        {
            return  _context.GetTable<TEntity>().Where(x => x.IsDeleted).ToList();
        }
    }
}
