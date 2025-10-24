using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class StoreFixedExpenseDto : BaseAuditLog
    {
        public string ExpenseId { get; set; }
        public string StoreId { get; set; }
        public string MonthYear { get; set; }
        public decimal? RentCost { get; set; }
        public decimal ElectricityCost { get; set; }
        public decimal WaterCost { get; set; }
        public string Note { get; set; }
    }
}
