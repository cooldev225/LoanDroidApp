using Models.data;
using Models.data.Interfaces;
using System;
namespace LoanDroidApp.Models
{
    public class LoanInterestPayment : IAuditableEntity
    {
        public long Id { get; set; }
        public long LoanRequestId { get; set; }
        public long AccountPaymentId { get; set; }
        public double Capital { get; set; }
        public double Interest { get; set; }
        public double Balabnce { get; set; }
        public int TimesNum { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedDevice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDevice { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}