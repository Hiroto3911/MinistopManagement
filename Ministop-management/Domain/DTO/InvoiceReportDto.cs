using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class InvoiceReportDto
    {
        public string InvoiceID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhone { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
