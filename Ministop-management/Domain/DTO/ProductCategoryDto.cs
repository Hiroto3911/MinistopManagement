using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ProductCategoryDto : BaseAuditLog
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
