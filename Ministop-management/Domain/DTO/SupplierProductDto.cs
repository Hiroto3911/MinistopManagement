using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class SupplierProductDto
    {
        public string Id { get; set; }
        public string SupplierId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal SupplyPrice { get; set; }
        public byte Status { get; set; }
    }
}
