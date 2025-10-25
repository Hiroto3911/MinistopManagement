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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public EmployeeRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }

        public override IReadOnlyList<Employee> GetPagedResponse(int pageNumber, int pageSize)
        {
            return _context.GetTable<Employee>()
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Created)
                .ToList();
        }
        public override bool All(Expression<Func<Employee, bool>> predicate)
        {
            return _context.GetTable<Employee>().Where(x => !x.IsDeleted).All(predicate);
        }
        public override bool Any(Expression<Func<Employee, bool>> predicate)
        {
            return _context.GetTable<Employee>().Where(x => !x.IsDeleted).Any(predicate);
        }
        public override int GetCount(Expression<Func<Employee, bool>> predicate)
        {
            return _context.GetTable<Employee>().Where(x => !x.IsDeleted).Count(predicate);
        }

        public void SoftDelete(Employee entity, bool hasTransaction = false)
        {
            var table = _context.GetTable<Employee>();


            var existing = table.SingleOrDefault(s => s.EmployeeID == entity.EmployeeID);
            if (existing == null) return;

            existing.IsDeleted = true;
            existing.LastModified = entity.LastModified;
            existing.LastModifiedBy = entity.LastModifiedBy;

            if (!hasTransaction)
            {
                _context.SubmitChanges();
            }
        }

        public void SoftDeleteRange(IList<Employee> entities, bool hasTransaction = false)
        {
           
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                
            }
            if (!hasTransaction)
            {
                _context.SubmitChanges();
            }
        }

        public IReadOnlyList<Employee> GetAllIsDelete()
        {
            return _context.GetTable<Employee>().Where(x => x.IsDeleted).ToList();
        }

    }
}
