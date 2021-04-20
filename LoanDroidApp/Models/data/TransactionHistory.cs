using Models.data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.data
{
    public class TransactionHistory : IAuditableEntity
    {
        public long Id { get; set; }
        public long FromPaymentId { get; set; }
        public long ToPaymentId { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }
        public bool IsSuccess { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDevice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDevice { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
