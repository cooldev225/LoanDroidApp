using Models.data;
using Models.data.Interfaces;
using System;
namespace LoanDroidApp.Models
{
    public class InvestmentStatus : IAuditableEntity
    {
        public long Id { get; set; }
        public long InvestmenttId { get; set; }
        public InvestStatus Status { get; set; }
        public string StatusReason { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedDevice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDevice { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}