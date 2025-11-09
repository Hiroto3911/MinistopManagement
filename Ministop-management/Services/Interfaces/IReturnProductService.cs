using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IReturnProductService
    {
        Result<IReadOnlyList<ReturnProductDto>> GetAll();
        Result<ReturnProductDto> GetReturnProductByID(string id);
        Result<bool> CreateReturnProduct(ReturnProductDto returnProductID);
        //Result<bool> UpdateInvoice(InvoiceDto invoiceEdit);
        Result<bool> RemoveReturnProduct(string returnProductID);
        PagedResult<IReadOnlyList<ReturnProductDto>> GetReturnProduct(string invoceID, int pageNumber, int pageSize);
    }
}
