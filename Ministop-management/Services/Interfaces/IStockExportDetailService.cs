using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStockExportDetailService
    {
        Result<bool> Any(string exportId);
        Result<bool> CreatestockExportDetaill(StockExportDetailDto StockExportDetailDto);
        Result<IReadOnlyList<StockExportDetailDto>> GetAll();
        Result<int> GetCount(string storeID, DateTime dateNow);
        PagedResult<IReadOnlyList<StockExportDetailDto>> GetExportDetails(string storeID, int month, int year, int pageNumber, int pageSize);
        PagedResult<IReadOnlyList<StockExportDetailDto>> GetStockExportDetail(string stockExportId, int pageNumber, int pageSize);
        Result<StockExportDetailDto> GetStockExportDetailByID(string id);
        Result<bool> RemoveRangeStockExportDetailByExportID(string exportID);
        Result<bool> RemoveStockExportDetail(string id);
        Result<bool> UpdateStockExportDetail(StockExportDetailDto stockExportDetailEdit);
    }
}
