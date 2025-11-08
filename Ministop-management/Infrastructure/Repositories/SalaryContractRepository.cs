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
    public class SalaryContractRepository : GenericRepository<SalaryContract>, ISalaryContractRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public SalaryContractRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }

        public override IReadOnlyList<SalaryContract> GetPagedResponse(int pageNumber, int pageSize)
        {
            return _context.GetTable<SalaryContract>()
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Created)
                .ToList();
        }
        public override bool All(Expression<Func<SalaryContract, bool>> predicate)
        {
            return _context.GetTable<SalaryContract>().Where(x => !x.IsDeleted).All(predicate);
        }
        public override bool Any(Expression<Func<SalaryContract, bool>> predicate)
        {
            return _context.GetTable<SalaryContract>().Where(x => !x.IsDeleted).Any(predicate);
        }
        public override int GetCount(Expression<Func<SalaryContract, bool>> predicate)
        {
            return _context.GetTable<SalaryContract>().Where(x => !x.IsDeleted).Count(predicate);
        }

        public void SoftDelete(SalaryContract entity, bool hasTransaction = false)
        {
            var table = _context.GetTable<SalaryContract>();


            var existing = table.SingleOrDefault(s => s.ContractID == entity.ContractID);
            if (existing == null) return;

            existing.IsDeleted = true;
            existing.LastModified = entity.LastModified;
            existing.LastModifiedBy = entity.LastModifiedBy;

            if (!hasTransaction)
            {
                _context.SubmitChanges();
            }
        }

        public void SoftDeleteRange(IList<SalaryContract> entities, bool hasTransaction = false)
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

        public IReadOnlyList<SalaryContract> GetAllIsDelete()
        {
            return _context.GetTable<SalaryContract>().Where(x => x.IsDeleted).ToList();
        }

        public SalaryContract GetCurrentContract(string employeeId)
        {
            return GetAll()
                .Where(x => x.EmployeeID == employeeId && !x.IsDeleted && x.EndDate == null)
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();
        }


    }
}

