using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class PromotionProductDto
    {
        public string Id { get; set; }
        public string PromotionId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal DiscountAmount { get; set; }
        public int MinQuantity { get; set; }
        public string Note { get; set; }
    }
}
