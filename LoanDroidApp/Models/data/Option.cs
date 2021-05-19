using Models.data;
using Models.data.Interfaces;
using System;
namespace Models.data
{
    public class Option : IAuditableEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Kind { get; set;}
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDevice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDevice { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}