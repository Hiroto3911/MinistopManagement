using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IInvoiceDetailService
    {
        Result<IReadOnlyList<InvoiceDetailDto>> GetAll();
        Result<InvoiceDetailDto> GetInvoiceDetailByID(string id);
        PagedResult<IReadOnlyList<InvoiceDetailDto>> GetInvoiceDetail(int pageNumber, int pageSize);
        Result<bool> CreateInvoiceDetail(InvoiceDetailDto invoiceDetailDto);
        Result<bool> UpdateInvoiceDetail(InvoiceDetailDto invoiceDetailsEdit);
        Result<bool> RemoveInvoiceDetail(string invoiceId);
    }
}
