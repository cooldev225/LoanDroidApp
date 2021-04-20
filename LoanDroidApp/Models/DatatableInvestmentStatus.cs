using Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class ApplicationInvestmentStatus : InvestmentStatus{
        public string ByUserName { get; set; }
        public string ByFriendlyName { get; set; }
        public byte[] ByAvatarImage { get; set; }
    }
    public class DatatableInvestmentStatus : IDatatable
    {
        public DatatablePagination meta { get; set; }
        public List<ApplicationInvestmentStatus> data { get; set; }
    }
}
