using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class SalaryContractDto : BaseAuditLog
    {
        public string ContractId { get; set; }
        public string EmployeeId { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HourlyRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate
        {
            get; set;
        }
    }
}
