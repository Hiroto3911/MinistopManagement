using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class SalaryDto
    {
        public string SalaryId { get; set; }
        public string ContractId { get; set; }
        public string MonthYear { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deduction { get; set; }
        public string Status { get; set; }
    }
}
