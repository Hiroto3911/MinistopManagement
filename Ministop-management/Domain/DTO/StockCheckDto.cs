using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class StockCheckDto
    {
        public string CheckId { get; set; }
        public string StoreId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime CheckDate { get; set; }
    }
}
