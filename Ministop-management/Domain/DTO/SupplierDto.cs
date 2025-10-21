using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class SupplierDto : BaseAuditLog
    {
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
