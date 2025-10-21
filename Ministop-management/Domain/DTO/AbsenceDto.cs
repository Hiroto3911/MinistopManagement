using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class AbsenceDto
    {
        public string AbsenceId { get; set; }
        public string EmployeeId { get; set; } 
        public string ShiftId { get; set; }
        public DateTime WorkDate { get; set; }
        public bool IsLeaveOfAbsence { get; set; }
        public string Reason { get; set; }
        public bool IsPaid { get; set; }
    }
}
