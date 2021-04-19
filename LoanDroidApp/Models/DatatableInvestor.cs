using Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class ApplicationInvestor : ApplicationUser
    {
        public List<AccountPayment> AccountPayment { get; set; }
        public ApplicationInvestor(ApplicationUser user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.EmailConfirmed = user.EmailConfirmed;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.IsEnabled = user.IsEnabled;
            this.PhoneNumber = user.PhoneNumber;
            this.OtherPhone = user.OtherPhone;
            this.OfficeNumber = user.OfficeNumber;
            this.JobTitle = user.JobTitle;
            this.Address = user.Address;
            this.Claims = user.Claims;
            this.AvatarImage = user.AvatarImage;
            this.CreatedBy = user.CreatedBy;
            this.CreatedDate = user.CreatedDate;
            this.CreatedDevice = user.CreatedDevice;
            this.UpdatedBy = user.UpdatedBy;
            this.UpdatedDate = user.UpdatedDate;
            this.UpdatedDevice = user.UpdatedDevice;
        }
    }
    public class DatatableInvestor: IDatatable
    {
        public DatatablePagination meta { get; set; }
        public List<ApplicationInvestor> data { get; set; }
    }
}
