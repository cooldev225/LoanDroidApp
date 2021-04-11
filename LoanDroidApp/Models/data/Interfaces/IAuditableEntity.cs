// =============================
// Email: bluestar1027@hotmail.com

// =============================

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Models.data.Interfaces
{
    public interface IAuditableEntity
    {
        public string CreatedBy { get; set; }
        public string CreatedDevice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDevice { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
