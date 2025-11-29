using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class SalaryContractReportMainDto
    {
        public string ContractID { get; set; }
        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string EmploymentType { get; set; }
        public string StoreName { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HourlyRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal TotalAllowance { get; set; }
        public decimal EstimatedTotalIncome { get; set; }
        public bool Gender { get; set; }              
        public DateTime BirthDate { get; set; }      
        public string Phone { get; set; }
        public string Address { get; set; }                 
        public string IdentityNumber { get; set; }
    }
}
