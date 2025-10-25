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
    public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public ProductCategoryRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }

        public override IReadOnlyList<ProductCategory> GetPagedResponse(int pageNumber, int pageSize)
        {
            return _context.GetTable<ProductCategory>()
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Created)
                .ToList();
        }
        public override bool All(Expression<Func<ProductCategory, bool>> predicate)
        {
            return _context.GetTable<ProductCategory>().Where(x => !x.IsDeleted).All(predicate);
        }
        public override bool Any(Expression<Func<ProductCategory, bool>> predicate)
        {
            return _context.GetTable<ProductCategory>().Where(x => !x.IsDeleted).Any(predicate);
        }
        public override int GetCount(Expression<Func<ProductCategory, bool>> predicate)
        {
            return _context.GetTable<ProductCategory>().Where(x => !x.IsDeleted).Count(predicate);
        }

        public void SoftDelete(ProductCategory entity, bool hasTransaction = false)
        {
            var table = _context.GetTable<ProductCategory>();


            var existing = table.SingleOrDefault(s => s.CategoryID == entity.CategoryID);
            if (existing == null) return;

            existing.IsDeleted = true;
            existing.LastModified = entity.LastModified;
            existing.LastModifiedBy = entity.LastModifiedBy;

            if (!hasTransaction)
            {
                _context.SubmitChanges();
            }
        }

        public void SoftDeleteRange(IList<ProductCategory> entities, bool hasTransaction = false)
        {
            var table = _context.GetTable<ProductCategory>();
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

        public IReadOnlyList<ProductCategory> GetAllIsDelete()
        {
            return _context.GetTable<ProductCategory>().Where(x => x.IsDeleted).ToList();
        }
    }
}
