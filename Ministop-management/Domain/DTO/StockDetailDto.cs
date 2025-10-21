using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class StockDetailDto
    {
        public string StockDetailId { get; set; }
        public string StoreId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
