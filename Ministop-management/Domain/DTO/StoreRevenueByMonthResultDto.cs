using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class StoreRevenueByMonthResultDto
    {
        public int Year { get; set; }
       
        public int Month { get; set; }
       
        public string StoreID { get; set; }
      
        public string StoreName { get; set; }
       
        public decimal? Revenue { get; set; }
    }
}
