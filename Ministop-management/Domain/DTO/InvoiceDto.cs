using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class InvoiceDto
    {
        public string InvoiceId { get; set; }
        public string StoreId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal? FinalAmount { get; set; }
        public decimal? DiscountTotal { get; set; }
        public byte status { get; set; }
        public decimal? SubTotal { get; set; }
    }
}
