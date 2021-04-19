using Models.data;
using Models.data.Interfaces;
using System;
namespace LoanDroidApp.Models
{
    public class LoanRequestStatus : IAuditableEntity
    {
        public long Id { get; set; }
        public long LoanRequestId { get; set; }
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