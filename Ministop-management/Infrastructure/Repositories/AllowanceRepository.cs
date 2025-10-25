using Domain.DTO;
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
    public class AllowanceRepository : GenericRepository<Allowance>, IAllowanceRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public AllowanceRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }

        public override IReadOnlyList<Allowance> GetPagedResponse(int pageNumber, int pageSize)
        {
            return _context.GetTable<Allowance>()
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Created)
                .ToList();
        }
        public override bool All(Expression<Func<Allowance, bool>> predicate)
        {
            return _context.GetTable<Allowance>().Where(x => !x.IsDeleted).All(predicate);
        }
        public override bool Any(Expression<Func<Allowance, bool>> predicate)
        {
            return _context.GetTable<Allowance>().Where(x => !x.IsDeleted).Any(predicate);
        }
        public override int GetCount(Expression<Func<Allowance, bool>> predicate)
        {
            return _context.GetTable<Allowance>().Where(x => !x.IsDeleted).Count(predicate);
        }

        public void SoftDelete(Allowance entity, bool hasTransaction = false)
        {
            var table = _context.GetTable<Allowance>();


            var existing = table.SingleOrDefault(s => s.AllowanceID == entity.AllowanceID);
            if (existing == null) return;

            existing.IsDeleted = true;
            existing.LastModified = entity.LastModified;
            existing.LastModifiedBy = entity.LastModifiedBy;

            if (!hasTransaction)
            {
                _context.SubmitChanges();
            }
        }

        public void SoftDeleteRange(IList<Allowance> entities, bool hasTransaction = false)
        {
            var table = _context.GetTable<Allowance>();
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

        public IReadOnlyList<Allowance> GetAllIsDelete()
        {
            return _context.GetTable<Allowance>().Where(x => x.IsDeleted).ToList();
        }
    }
}
