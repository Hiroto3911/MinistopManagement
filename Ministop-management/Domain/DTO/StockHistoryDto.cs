using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class StockHistoryDto
    {
        public string StockHistoryId { get; set; }
        public string StockDetailId { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeType { get; set; }
        public int QuantityChange { get; set; }
        public string RefId { get; set; }
    }
}
