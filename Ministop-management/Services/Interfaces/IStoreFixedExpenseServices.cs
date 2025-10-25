using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStoreFixedExpenseServices
    {
        Result<string> CreateStoreFixedExpense(StoreFixedExpenseDto expenseDto);
        Result<IReadOnlyList<StoreFixedExpenseDto>> GetAll();
        Result<IReadOnlyList<StoreFixedExpenseDto>> GetAllStoreFixedExpenseIsDelete();
        PagedResult<IReadOnlyList<StoreFixedExpenseDto>> GetStoreFixedExpense(int pageNumber, int pageSize);
        PagedResult<IReadOnlyList<StoreFixedExpenseDto>> GetStoreFixedExpense(string storeId, int pageNumber, int pageSize);
        Result<StoreFixedExpenseDto> GetStoreFixedExpenseByID(string id);
        Result<bool> RemoveStoreFixedExpense(string expenseId);
        Result<bool> RestoreStoreFixedExpense(List<string> listRestoreId);
        Result<string> UpdateStoreFixedExpense(StoreFixedExpenseDto expenseEdit);
    }
}
