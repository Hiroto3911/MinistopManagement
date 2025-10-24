using Domain.DTO;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IStoreFixedExpenseRepository : IGenericRepository<StoreFixedExpense>
    {
        IReadOnlyList<StoreFixedExpense> GetAllIsDelete();
        IReadOnlyList<StoreFixedExpense> GetPagedResponse(Expression<Func<StoreFixedExpense, bool>> predicate, int pageNumber, int pageSize);
        void SoftDelete(StoreFixedExpense entity, bool hasTransaction = false);
        void SoftDeleteRange(IList<StoreFixedExpense> entities, bool hasTransaction = false);
    }
}
