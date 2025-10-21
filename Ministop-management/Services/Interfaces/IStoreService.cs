using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStoreService
    {
        Result<bool> CreateStore(StoreDto storeEntity);
        Result<IReadOnlyList<StoreDto>> GetAll();
        Result<IReadOnlyList<StoreDto>> GetAllStoreIsDelete();
        PagedResult<IReadOnlyList<StoreDto>> GetStore(int pageNumber, int pageSize);
        Result<StoreDto> GetStoreByID(string id);
        Result<bool> RemoveStore(string storeId);
        Result<bool> RestoreStore(List<string> listRestoreId);
        Result<bool> UpdateStore(StoreDto storeEdit);
    }
}
