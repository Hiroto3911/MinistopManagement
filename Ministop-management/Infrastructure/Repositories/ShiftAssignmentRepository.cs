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
    public class ShiftAssignmentRepository : GenericRepository<ShiftAssignment>, IShiftAssignmentRepository
    {
        private readonly MinistopDataContextDataContext _context;

        public ShiftAssignmentRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }

        public override IReadOnlyList<ShiftAssignment> GetPagedResponse(int pageNumber, int pageSize)
        {
            return _context.GetTable<ShiftAssignment>()
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Created)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public override bool All(Expression<Func<ShiftAssignment, bool>> predicate)
        {
            return _context.GetTable<ShiftAssignment>()
                .Where(x => !x.IsDeleted)
                .All(predicate);
        }

        public override bool Any(Expression<Func<ShiftAssignment, bool>> predicate)
        {
            return _context.GetTable<ShiftAssignment>()
                .Where(x => !x.IsDeleted)
                .Any(predicate);
        }

        public override int GetCount(Expression<Func<ShiftAssignment, bool>> predicate)
        {
            return _context.GetTable<ShiftAssignment>()
                .Where(x => !x.IsDeleted)
                .Count(predicate);
        }

        public void SoftDelete(ShiftAssignment entity, bool hasTransaction = false)
        {
            var table = _context.GetTable<ShiftAssignment>();
            var existing = table.SingleOrDefault(s => s.Id == entity.Id && !s.IsDeleted);

            if (existing == null) return;

            existing.IsDeleted = true;
            existing.LastModified = entity.LastModified ?? DateTime.Now;
            existing.LastModifiedBy = entity.LastModifiedBy;

            if (!hasTransaction)
            {
                _context.SubmitChanges();
            }
        }

        public void SoftDeleteRange(IList<ShiftAssignment> entities, bool hasTransaction = false)
        {
            var table = _context.GetTable<ShiftAssignment>();
            var ids = entities.Select(e => e.Id).ToList();

            var existingRecords = table.Where(s => ids.Contains(s.Id) && !s.IsDeleted).ToList();

            foreach (var record in existingRecords)
            {
                var input = entities.First(e => e.Id == record.Id);
                record.IsDeleted = true;
                record.LastModified = input.LastModified ?? DateTime.Now;
                record.LastModifiedBy = input.LastModifiedBy;
            }

            if (!hasTransaction)
            {
                _context.SubmitChanges();
            }
        }

        public IReadOnlyList<ShiftAssignment> GetAllIsDelete()
        {
            return _context.GetTable<ShiftAssignment>()
                .Where(x => x.IsDeleted)
                .ToList();
        }
    }
}
