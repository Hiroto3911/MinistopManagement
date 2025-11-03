using Domain.DTO;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPromotionProductService
    {
        Result<bool> CreatePromotionProduct(PromotionProductDto PromotionProductDto);
        Result<IReadOnlyList<PromotionProductDto>> GetAll();
        PagedResult<IReadOnlyList<PromotionProductDto>> GetPromotionProduct(string promotionId, int pageNumber, int pageSize);
        Result<PromotionProductDto> GetPromotionProductByID(string id);
        Result<bool> RemovePromotionProduct(string PromotionProductId);
        Result<bool> UpdatePromotionProduct(PromotionProductDto PromotionProductEdit);
    }
}
