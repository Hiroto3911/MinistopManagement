using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Services.Interfaces
{
    public interface IStockImportService
    {
        Result<bool> CreatestockImport(StockImportDto StockImportDto);
        Result<IReadOnlyList<StockImportDto>> GetAll();
        PagedResult<IReadOnlyList<StockImportDto>> GetStockImport(string storeId, int pageNumber, int pageSize);
        Result<StockImportDto> GetStockImportByID(string id);
        Result<bool> RemoveStockImport(string id);
        Result<bool> UpdateStockImport(StockImportDto stockImportEdit);
    }
}