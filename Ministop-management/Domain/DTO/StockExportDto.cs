using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class StockExportDto
    {
        public string ExportId { get; set; }
        public string StoreId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string TypeExport { get; set; }
        public DateTime ExportDate { get; set; }
        public string Reason { get; set; }
        public byte Status { get; set; }
    }
}
