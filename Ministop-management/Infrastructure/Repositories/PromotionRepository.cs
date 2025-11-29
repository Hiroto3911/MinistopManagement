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
    public class PromotionRepository : GenericRepository<Promotion>, IPromotionRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public PromotionRepository(MinistopDataContextDataContext context) : base(context) 
        {
            _context = context;
        }
        public override IReadOnlyList<Promotion> GetPagedResponse(int pageNumber, int pageSize)
        {
            return _context.GetTable<Promotion>()
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber -1) * pageSize )
                .Take(pageSize)
                .OrderByDescending(x => x.Created)
                .ToList();
        }
        public override bool All(Expression<Func<Promotion, bool>> predicate)
        {
            return _context.GetTable<Promotion>().Where(x => !x.IsDeleted).All(predicate);
        }
        public override bool Any(Expression<Func<Promotion, bool>> predicate)
        {
            return _context.GetTable<Promotion>().Where(x => !x.IsDeleted).Any(predicate);
        }
        public override int GetCount(Expression<Func<Promotion, bool>> predicate)
        {
            return _context.GetTable<Promotion>().Where(x => !x.IsDeleted).Count(predicate);
        }

        public void SoftDelete(Promotion entity, bool hasTransaction = false)
        {
            var table = _context.GetTable<Promotion>();


            var existing = table.SingleOrDefault(s => s.PromotionID == entity.PromotionID);
            if (existing == null) return;

            existing.IsDeleted = true;
            existing.LastModified = entity.LastModified;
            existing.LastModifiedBy = entity.LastModifiedBy;

            if (!hasTransaction)
            {
                _context.SubmitChanges();
            }
        }

        public void SoftDeleteRange(IList<Promotion> entities, bool hasTransaction = false)
        {
            var table = _context.GetTable<Promotion>();
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

        public IReadOnlyList<Promotion> GetAllIsDelete()
        {
            return _context.GetTable<Promotion>().Where(x => x.IsDeleted).ToList();
        }
    }
}
