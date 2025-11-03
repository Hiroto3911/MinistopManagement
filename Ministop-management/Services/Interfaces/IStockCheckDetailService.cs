using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStockCheckDetailService
    {
        Result<bool> Any(string checkId);
        Result<bool> CreateStockCheckDetail(StockCheckDetailDto stockCheckDetailDto);
        Result<IReadOnlyList<StockCheckDetailDto>> GetAll();
        PagedResult<IReadOnlyList<StockCheckDetailDto>> GetStockCheckDetail(string stockCheckID, int pageNumber, int pageSize);
        Result<StockCheckDetailDto> GetStockCheckDetailByID(string id);
        Result<bool> RemoveRangeStockCheckDetailByCheckID(string checkID);
        Result<bool> RemoveStockCheckDetail(string id);
        Result<bool> UpdateStockCheckDetail(StockCheckDetailDto stockCheckDetailEdit);
    }
}
