using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class StockCheckDetailDto
    {
        public string Id { get; set; }
        public string CheckId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantitySystem { get; set; }
        public int QuantityActual { get; set; }
        public int QuantityVariance { get; set; }
        public string Note { get; set; } 
    }
}
