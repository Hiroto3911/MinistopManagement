using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IInvoiceService
    {
        Result<IReadOnlyList<InvoiceDto>> GetAll();
        Result<InvoiceDto> GetInvoiceByID(string id);
        Result<string> CreateInvoice(InvoiceDto invoiceDto);
        //Result<bool> UpdateInvoice(InvoiceDto invoiceEdit);
        Result<bool> RemoveInvoice(string invoiceId);
        PagedResult<IReadOnlyList<InvoiceDto>> GetInvoice(string storeId, int pageNumber, int pageSize);
        Result<bool> UpdateInvoice(string invoiceID);
    }
}
