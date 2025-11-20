using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class Top3BestSellingStoreDto
    {
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public decimal TotalInvoices { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalProductsSold { get; set; }
        public decimal AverageInvoiceValue { get; set; }
    }
}
