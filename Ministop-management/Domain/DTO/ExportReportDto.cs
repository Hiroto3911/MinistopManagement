using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ExportReportDto
    {
        public string ExportID { get; set; }
        public DateTime ExportDate { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Reason { get; set; }
        public string TypeExport { get; set; }
        public byte Status { get; set; }
    }

}
