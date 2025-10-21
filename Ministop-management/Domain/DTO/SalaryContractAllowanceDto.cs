using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class SalaryContractAllowanceDto
    {
        public string Id { get; set; }
        public string ContractId { get; set; }
        public string AllowanceId { get; set; }
        public decimal? CustomAmount { get; set; }
    }
}
