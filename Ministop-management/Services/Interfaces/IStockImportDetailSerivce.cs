using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStockImportDetailSerivce
    {
        Result<bool> Any(string importId);
        Result<bool> CreatestockImportDetail(StockImportDetailDto StockImportDetailDto);
        Result<IReadOnlyList<StockImportDetailDto>> GetAll();
    
        PagedResult<IReadOnlyList<StockImportDetailDto>> GetStockImportDetail(string stockImportId, int pageNumber, int pageSize);
        Result<StockImportDetailDto> GetStockImportDetailByID(string id);
        Result<bool> RemoveRangeStockImportDetailByImportID(string importID);
        Result<bool> RemoveStockImportDetail(string id);
        Result<bool> UpdateStockImportDetail(StockImportDetailDto stockImportDetailEdit);
    }
}
