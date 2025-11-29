using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class EmployeeDto : BaseAuditLog
    {
        public string EmployeeId { get; set; } 
        public string StoreId { get; set; } 
        public string FullName { get; set; } 
        public bool Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; } 
        public string Position { get; set; } 
        public string EmploymentType { get; set; } 
        public string PasswordHash { get; set; }
        public string Address { get; set; }           
        public string IdentityNumber { get; set; }
    }
}
