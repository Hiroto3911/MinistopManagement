using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class InvoiceDetail
    {
        public string Id { get; set; } 
        public string InvoiceId { get; set; } 
        public string ProductId { get; set; } 
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
