using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class PriceProposalDto
    {
        public string ProposalId { get; set; } 
        public string ProductId { get; set; } 
        public string StoreId { get; set; } 
        public string ManagerId { get; set; } 
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public string Reason { get; set; } 
        public DateTime ProposalDate { get; set; }
        public string Status { get; set; } 
        public string ApprovedBy { get; set; }
    }
}
