using Domain.Entity;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStockCheckService
    {
        Result<bool> CreatestockCheck(StockCheckDto StockCheckDto);
        Result<IReadOnlyList<StockCheckDto>> GetAll();
        PagedResult<IReadOnlyList<StockCheckDto>> GetStockCheck(int pageNumber, int pageSize);
        Result<StockCheckDto> GetStockCheckByID(string id);
        Result<bool> RemoveStockCheck(string id);
        Result<bool> UpdateStockCheck(StockCheckDto stockImportEdit);
    }
}
