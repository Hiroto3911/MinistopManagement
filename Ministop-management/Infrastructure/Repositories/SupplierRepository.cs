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
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public SupplierRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public override IReadOnlyList<Supplier> GetPagedResponse(int pageNumber, int pageSize)
        {
            return _context.GetTable<Supplier>()
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Created)
                .ToList();
        }
        public override bool All(Expression<Func<Supplier, bool>> predicate)
        {
            return _context.GetTable<Supplier>().Where(x => !x.IsDeleted).All(predicate);
        }
        public override bool Any(Expression<Func<Supplier, bool>> predicate)
        {
            return _context.GetTable<Supplier>().Where(x => !x.IsDeleted).Any(predicate);
        }
        public override int GetCount(Expression<Func<Supplier, bool>> predicate)
        {
            return _context.GetTable<Supplier>().Where(x => !x.IsDeleted).Count(predicate);
        }

        public void SoftDelete(Supplier entity, bool hasTransaction = false)
        {
            var table = _context.GetTable<Supplier>();


            var existing = table.SingleOrDefault(s => s.SupplierID == entity.SupplierID);
            if (existing == null) return;

            existing.IsDeleted = true;
            existing.LastModified = entity.LastModified;
            existing.LastModifiedBy = entity.LastModifiedBy;

            if (!hasTransaction)
            {
                _context.SubmitChanges();
            }
        }

        public void SoftDeleteRange(IList<Supplier> entities, bool hasTransaction = false)
        {
            var table = _context.GetTable<Supplier>();
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

        public IReadOnlyList<Supplier> GetAllIsDelete()
        {
            return _context.GetTable<Supplier>().Where(x => x.IsDeleted).ToList();
        }

    }
}
