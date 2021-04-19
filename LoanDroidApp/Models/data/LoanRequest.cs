using Models.data;
using Models.data.Interfaces;
using System;
namespace LoanDroidApp.Models
{
    public class LoanRequest : IAuditableEntity
    {
        public long Id { get; set; }
        public string ClientId { get; set; }
        public double Amount { get; set; }
        public string InterestingRate { get; set; }
        public LoanCycle Cycle { get; set; }
        public LoanStatus Status { get; set; }
        public string StatusReason { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedDevice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDevice { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}