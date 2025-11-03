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
    public class StoreFixedExpenseRepository : GenericRepository<StoreFixedExpense>, IStoreFixedExpenseRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public StoreFixedExpenseRepository(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public override IReadOnlyList<StoreFixedExpense> GetPagedResponse(int pageNumber, int pageSize)
        {
            return _context.GetTable<StoreFixedExpense>()
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Created)
                .ToList();
        }
        public IReadOnlyList<StoreFixedExpenseDto> GetPagedResponse(Expression<Func<StoreFixedExpense, bool>> predicate, int pageNumber, int pageSize)
        {
            var query = from e in _context.GetTable<StoreFixedExpense>().Where(predicate)
                        join s in _context.GetTable<Store>() on e.StoreID equals s.StoreID
                        where !e.IsDeleted 
                        orderby e.Created descending
                        select new StoreFixedExpenseDto
                        {
                            ExpenseId = e.ExpenseID,
                            StoreId = s.StoreID,
                            StoreName = s.StoreName,
                            ElectricityCost = e.ElectricityCost,
                            RentCost = e.RentCost,
                            WaterCost = e.WaterCost,
                            Status = e.Status,
                            Note = e.Note,
                            Created = e.Created,
                            CreatedBy = e.CreatedBy,
                            LastModified = e.LastModified,
                            LastModifiedBy = e.LastModifiedBy
                        };
            return query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();
        }
        public override bool All(Expression<Func<StoreFixedExpense, bool>> predicate)
        {
            return _context.GetTable<StoreFixedExpense>().Where(x => !x.IsDeleted).All(predicate);
        }
        public override bool Any(Expression<Func<StoreFixedExpense, bool>> predicate)
        {
            return _context.GetTable<StoreFixedExpense>().Where(x => !x.IsDeleted).Any(predicate);
        }
        public override int GetCount(Expression<Func<StoreFixedExpense, bool>> predicate)
        {
            return _context.GetTable<StoreFixedExpense>().Where(x => !x.IsDeleted).Count(predicate);
        }

        public void SoftDelete(StoreFixedExpense entity, bool hasTransaction = false)
        {
            var table = _context.GetTable<StoreFixedExpense>();


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

        public void SoftDeleteRange(IList<StoreFixedExpense> entities, bool hasTransaction = false)
        {
            var table = _context.GetTable<StoreFixedExpense>();
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

        public IReadOnlyList<StoreFixedExpense> GetAllIsDelete()
        {
            return _context.GetTable<StoreFixedExpense>().Where(x => x.IsDeleted).ToList();
        }
    }
}
