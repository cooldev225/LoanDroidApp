using Models.data;
using Models.data.Interfaces;
using System;
namespace Models.data
{
    public class Company : IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set;}
        public string Direction { get; set;}
        public string Phone { get; set; }
        public string CPhone { get; set; }
        public string Phone1 { get; set; }
        public string CPhone1 { get; set; }
        public string Phone2 { get; set; }
        public string CPhone2 { get; set; }
        public string Phone3 { get; set; }
        public string CPhone3 { get; set; }
        public string Url { get; set; }
        public long AccountPaymentId { get; set; }
        public string DestinationAccount { get; set; }
        public int Pay { get; set; }
        public int Vendor { get; set; }
        public Byte[] AvatarImage { get; set; }
        public string Comment { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedDevice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDevice { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}