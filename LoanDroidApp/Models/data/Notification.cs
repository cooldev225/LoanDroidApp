using Models.data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.data
{
    public class Notification: IAuditableEntity
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Claim { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDevice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDevice { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
