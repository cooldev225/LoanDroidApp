using Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class ApplicationLoanRequestStatus : LoanRequestStatus{
        public string ByUserName { get; set; }
        public string ByFriendlyName { get; set; }
        public byte[] ByAvatarImage { get; set; }
    }
    public class DatatableLoanRequestStatus : IDatatable
    {
        public DatatablePagination meta { get; set; }
        public List<ApplicationLoanRequestStatus> data { get; set; }
    }
}
