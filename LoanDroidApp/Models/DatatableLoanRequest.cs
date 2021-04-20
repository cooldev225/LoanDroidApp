using Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class ApplicationLoanRequest : LoanRequest{
        public string UserName { get; set; }
        public string FriendlyName { get; set; }
        public byte[] AvatarImage { get; set; }
        public string LoanCycle { get; set; }
    }
    public class DatatableLoanRequest : IDatatable
    {
        public DatatablePagination meta { get; set; }
        public List<ApplicationLoanRequest> data { get; set; }
    }
}
