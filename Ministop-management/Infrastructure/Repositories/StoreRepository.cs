using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public StoreRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }

        public override IReadOnlyList<Store> GetPagedResponse(int pageNumber, int pageSize)
        {
            return _context.GetTable<Store>()
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Created)
                .ToList();
        }
        public override bool All(Expression<Func<Store, bool>> predicate)
        {
            return _context.GetTable<Store>().Where(x => !x.IsDeleted).All(predicate);
        }
        public override bool Any(Expression<Func<Store, bool>> predicate)
        {
            return _context.GetTable<Store>().Where(x => !x.IsDeleted).Any(predicate);
        }
        public override int GetCount(Expression<Func<Store, bool>> predicate)
        {
            return _context.GetTable<Store>().Where(x => !x.IsDeleted).Count(predicate);
        }

        public void SoftDelete(Store entity, bool hasTransaction = false)
        {
            var table = _context.GetTable<Store>();


            var existing = table.SingleOrDefault(s => s.StoreID == entity.StoreID);
            if (existing == null) return;

            existing.IsDeleted = true;
            existing.LastModified = entity.LastModified;
            existing.LastModifiedBy = entity.LastModifiedBy;

            if (!hasTransaction)
            {
                _context.SubmitChanges();
            }
        }

        public void SoftDeleteRange(IList<Store> entities, bool hasTransaction = false)
        {
            var table = _context.GetTable<Store>();
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

        public IReadOnlyList<Store> GetAllIsDelete()
        {
            return _context.GetTable<Store>().Where(x => x.IsDeleted).ToList();
        }
    }
}
