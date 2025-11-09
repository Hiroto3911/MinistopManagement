using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public  class InventoryReportDto
    {
        public string ProductID { get; set; }
        public int OpeningStock { get; set; }
        public int ImportInPeriod { get; set; }
        public int ExportInPeriod { get; set; }
        public int SaleInPeriod { get; set; }
        public int CheckIncrease { get; set; }
        public int CheckDecrease { get; set; }
        public int TotalExport { get; set; }
        public int ClosingStock { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
    }
}
