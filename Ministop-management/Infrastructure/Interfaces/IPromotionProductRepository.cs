using Domain.DTO;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IPromotionProductRepository : IGenericRepository<Promotion_Product>
    {
        Promotion_Product GetActivePromotionForProduct(string productId, int qty);
        IReadOnlyList<PromotionProductDto> GetPagedResponse(Expression<Func<Promotion_Product, bool>> predicate, int pageNumber, int pageSize);
    }
}
