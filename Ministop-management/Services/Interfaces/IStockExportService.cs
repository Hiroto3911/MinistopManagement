using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStockExportService
    {
        Result<bool> CreatestockExport(StockExportDto StockExportDto);
        Result<IReadOnlyList<StockExportDto>> GetAll();
        PagedResult<IReadOnlyList<StockExportDto>> GetStockExport(string storeId, int pageNumber, int pageSize);
        Result<StockExportDto> GetStockExportByID(string id);
        Result<bool> RemoveStockExport(string id);
        Result<bool> UpdateStockExport(StockExportDto stockImportEdit);
    }
}
