using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ProductDto : BaseAuditLog
    {
        public string ProductId { get; set; } 
        public string CategoryId { get; set; } 
        public string ProductName { get; set; } 
        public string Unit { get; set; } 
        public decimal StandardPrice { get; set; }
        public byte Status { get; set; }
    }
}
