using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStockHistoryService
    {
        PagedResult<IReadOnlyList<StockHistoryDto>> GetStockHistory(string stockDetailId, int pageNumber = 1, int pageSize = 20);
    }
}
