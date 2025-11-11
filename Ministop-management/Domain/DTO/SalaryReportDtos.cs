using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    // 1. Phiếu lương chính
    public class SalarySlipMainDto
    {
        public string SalaryID { get; set; }
        public string ContractID { get; set; }
        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string EmploymentType { get; set; }
        public string StoreName { get; set; }
        public string MonthYear { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deduction { get; set; }
        public int TotalHoursWorked { get; set; }
        public decimal TotalAllowance { get; set; }
        public decimal TotalIncome =>
            (EmploymentType == "Full-time" ? BasicSalary ?? 0 : (HourlyRate ?? 0) * TotalHoursWorked)
            + TotalAllowance + Bonus - Deduction;
    }

    // 2. Chi tiết phụ cấp
    public class SalarySlipAllowanceDto
    {
        public string AllowanceName { get; set; }
        public decimal Amount { get; set; }
    }

    // 3. Danh sách lương (mỗi nhân viên 1 dòng)
    public class SalaryListDto
    {
        public string SalaryID { get; set; }
        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string EmploymentType { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deduction { get; set; }
        public decimal TotalIncome { get; set; }
    }
}
