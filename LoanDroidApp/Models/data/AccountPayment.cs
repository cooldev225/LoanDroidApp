using Models.data;
using Models.data.Interfaces;
using System;
namespace LoanDroidApp.Models
{
    public class AccountPayment : IAuditableEntity
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public int Type { get; set; }
        public string GatewayUserName { get; set; }
        public string GatewayPassword { get; set; }
        public string GatewayEmail { get; set; }
        public string GatewayUrl { get; set; }
        public string GatewayName { get; set; }
        public string CardFirstName { get; set; }
        public string CardLastName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardAddress1 { get; set; }
        public string CardAddress2 { get; set; }
        public string BankCountry { get; set; }
        public string BankRoutingNumber { get; set; }
        public string BankCurrency { get; set; }
        public string BankAccountHolder { get; set; }
        public string BankName { get; set; }
        public string BankStreet { get; set; }
        public string BankCity { get; set; }
        public string BankRegion { get; set; }
        public string BankSwiftBicNumber { get; set; }
        public string BankIBANNumber { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedDevice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDevice { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}