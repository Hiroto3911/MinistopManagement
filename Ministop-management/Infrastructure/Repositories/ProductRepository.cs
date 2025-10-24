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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public ProductRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }

        public override IReadOnlyList<Product> GetPagedResponse(int pageNumber, int pageSize)
        {
            return _context.GetTable<Product>()
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Created)
                .ToList();
        }
        public override bool All(Expression<Func<Product, bool>> predicate)
        {
            return _context.GetTable<Product>().Where(x => !x.IsDeleted).All(predicate);
        }
        public override bool Any(Expression<Func<Product, bool>> predicate)
        {
            return _context.GetTable<Product>().Where(x => !x.IsDeleted).Any(predicate);
        }
        public override int GetCount(Expression<Func<Product, bool>> predicate)
        {
            return _context.GetTable<Product>().Where(x => !x.IsDeleted).Count(predicate);
        }

        public void SoftDelete(Product entity, bool hasTransaction = false)
        {
            var table = _context.GetTable<Product>();


            var existing = table.SingleOrDefault(s => s.ProductID == entity.ProductID);
            if (existing == null) return;

            existing.IsDeleted = true;
            existing.LastModified = entity.LastModified;
            existing.LastModifiedBy = entity.LastModifiedBy;

            if (!hasTransaction)
            {
                _context.SubmitChanges();
            }
        }

        public void SoftDeleteRange(IList<Product> entities, bool hasTransaction = false)
        {
            var table = _context.GetTable<Product>();
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

        public IReadOnlyList<Product> GetAllIsDelete()
        {
            return _context.GetTable<Product>().Where(x => x.IsDeleted).ToList();
        }
    }
}
