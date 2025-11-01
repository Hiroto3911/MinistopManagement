using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPromotionService
    {
        Result<bool> CreatePromotion(PromotionDto promotionDto);
        Result<IReadOnlyList<PromotionDto>> GetAll();
        Result<IReadOnlyList<PromotionDto>> GetAllPromotionIsDelete();
        PagedResult<IReadOnlyList<PromotionDto>> GetPromotion(int pageNumber, int pageSize);
        Result<PromotionDto> GetPromotionByID(string id);
        Result<bool> RemovePromotion(string PromotionId);
        Result<bool> RestorePromotion(List<string> listRestoreId);
        Result<bool> UpdatePromotion(PromotionDto PromotionEdit);
    }
}
