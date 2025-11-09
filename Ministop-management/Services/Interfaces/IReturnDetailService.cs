using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IReturnDetailService
    {
        Result<IReadOnlyList<ReturnDetailDto>> GetAll();
        Result<ReturnDetailDto> GetReturnDetailByID(string id);
        PagedResult<IReadOnlyList<ReturnDetailDto>> GetReturnDetail(string invoiceID,int pageNumber, int pageSize);
        Result<bool> CreateReturnDetail(ReturnDetailDto returnDetailDto);
        Result<bool> UpdateReturnDetail(ReturnDetailDto returnDetailsEdit);
        Result<bool> RemoveReturnDetail(string returnID);
        Result<bool> Any(string returnID);
        Result<bool> RemoveRangeReturnDetailByReturnID(string returnID);
    }
}
