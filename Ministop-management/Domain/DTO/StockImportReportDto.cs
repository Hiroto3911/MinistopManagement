using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class StockImportReportDto
    {
        public string ImportID { get; set; }
        public DateTime ImportDate { get; set; }
        public string StoreID { get; set; }
        public string SupplierID { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string SupplierName { get; set; }
        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalAmount { get; set; }
    }

}
