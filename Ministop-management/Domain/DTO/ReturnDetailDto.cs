using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ReturnDetailDto
    {
        public string Id { get; set; }
        public string ReturnId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal RefundAmount { get; set;}
    }
}
