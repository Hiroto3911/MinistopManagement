using Domain.DTO;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PromotionProductReponsitory : GenericRepository<Promotion_Product>, IPromotionProductRepository
    {
        private readonly MinistopDataContextDataContext _context;
        public PromotionProductReponsitory(MinistopDataContextDataContext context) : base(context)
        {
            _context = context;
        }
        public IReadOnlyList<PromotionProductDto> GetPagedResponse(Expression<Func<Promotion_Product, bool>> predicate, int pageNumber, int pageSize)
        {
            var query = from e in _context.GetTable<Promotion_Product>().Where(predicate)
                        join s in _context.GetTable<Product>() on e.ProductID equals s.ProductID
                        select new PromotionProductDto
                        {
                            Id = e.Id,
                            PromotionId = e.PromotionID,
                            ProductId = e.ProductID,
                            ProductName = s.ProductName,
                            DiscountAmount = e.DiscountAmount,
                            MinQuantity = e.MinQuantity,
                            Note = e.Note,
                        };
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        }
        public Promotion_Product GetActivePromotionForProduct(string productId, int qty)
        {
            return _context.GetTable<Promotion_Product>().Where(x => x.ProductID == productId && x.MinQuantity <= qty && x.Promotion.Status == false && DateTime.UtcNow.ToLocalTime() >= x.Promotion.StartDate && DateTime.UtcNow.ToLocalTime() <= x.Promotion.EndDate).OrderByDescending(x => x.Promotion.Priority).FirstOrDefault();
        }
    }
}
