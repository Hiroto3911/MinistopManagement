using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPriceProposalService
    {
        Result<IReadOnlyList<PriceProposalDto>> GetAll();
        Result<PriceProposalDto> GetpriceProposaByID(string id);
        PagedResult<IReadOnlyList<PriceProposalDto>> GetpriceProposal(string storeId, int pageNumber, int pageSize);
        Result<bool> CreatepriceProposal(PriceProposalDto priceProposalDto);
        Result<bool> UpdatepriceProposalDto(PriceProposalDto priceProposalEdit);
        Result<bool> RemovepriceProposalDto(string priceProposalID);
    }
}
