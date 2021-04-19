// =============================
// Email: bluestar1027@hotmail.com

// =============================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models.data.Interfaces;

namespace Models.data
{
    public class ApplicationPage : IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PageGroup PageGroup { get; set; }
        public string Claims { get; set; }
        public int Order { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDevice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDevice { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
