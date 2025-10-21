using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ShiftAssignmentDto : BaseAuditLog
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string ShiftId { get; set; }
        public DateTime WorkDate { get; set; }
        public string Note { get; set; }
    }
}
