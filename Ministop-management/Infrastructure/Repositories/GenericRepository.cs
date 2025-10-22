using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly MinistopDataContextDataContext _context;

        public GenericRepository(MinistopDataContextDataContext context)
        {
            _context = context;
        }
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.GetTable<TEntity>().FirstOrDefault(predicate);
        }

        public virtual  IReadOnlyList<TEntity> GetPagedResponse(int pageNumber, int pageSize)
        {
            return _context.GetTable<TEntity>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public virtual  TEntity Add(TEntity entity, bool hasTransaction = false)
        {
             _context.GetTable<TEntity>().InsertOnSubmit(entity);
            if (!hasTransaction)
            {
                 _context.SubmitChanges();
            }
            return entity;
        }
        public virtual bool AddRange(IList<TEntity> entities, bool hasTransaction = false)
        {
             _context.GetTable<TEntity>().InsertAllOnSubmit(entities);
            if (!hasTransaction)
            {
                 _context.SubmitChanges();
            }
            return true;
        }
        public virtual void Update(TEntity entity, bool hasTransaction = false)
        {
            //var table = _context.GetTable<TEntity>();
            //table.Attach(entity, false);
            if (!hasTransaction)
            {
                 _context.SubmitChanges();
            }
        }
        public virtual  void UpdateRange(IList<TEntity> entities, bool hasTransaction = false)
        {
            //var table = _context.GetTable<TEntity>();
            //foreach (var entity in entities)
            //{
            //    table.Attach(entity, true);
            //}

            if (!hasTransaction)
            {
                 _context.SubmitChanges();
            }
        }
        public virtual  void Delete(TEntity entity, bool hasTransaction = false)
        {
            _context.GetTable<TEntity>().DeleteOnSubmit(entity);
            if (!hasTransaction)
            {
                 _context.SubmitChanges();
            }
        }
        public virtual  void DeleteRange(IList<TEntity> entities, bool hasTransaction = false)
        {
            _context.GetTable<TEntity>().DeleteAllOnSubmit(entities);
            if (!hasTransaction)
            {
                 _context.SubmitChanges();
            }
        }
        public virtual  IReadOnlyList<TEntity> GetAll()
        {
            return  _context.GetTable<TEntity>().ToList();
        }
        public virtual  IReadOnlyList<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return  _context.GetTable<TEntity>().Where(predicate).ToList();
        }
        public virtual  bool All(Expression<Func<TEntity, bool>> predicate)
        {
            return  _context.GetTable<TEntity>().All(predicate);
        }
        public virtual  bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return  _context.GetTable<TEntity>().Any(predicate);
        }
        public virtual  int GetCount(Expression<Func<TEntity, bool>> predicate = null)
        {
            var table = _context.GetTable<TEntity>();
            if (predicate == null)
            {
                return table.Count();
            }
            return table.Count(predicate);
        }
        public IQueryable<TEntity> AsQueryable()
        {
            throw new NotImplementedException();
        }
    }
}
