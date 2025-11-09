using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ReturnProductDto
    {
        public string ReturnId { get; set; }
        public string StoreId { get; set; }
        public string InvoiceId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
