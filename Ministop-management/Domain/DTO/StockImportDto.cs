using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class StockImportDto
    {
        public string ImportID { get; set; }
        public string StoreId { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public byte Status { get; set; }
        public string Note { get; set; }
        public DateTime ImportDate { get; set; }
    }
}
