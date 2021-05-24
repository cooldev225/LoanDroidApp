using Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class ApplicationMilestone
    {
        public long InvestmentId { get; set; }
        public string InvestorId { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class DatatableMilestone: IDatatable
    {
        public DatatablePagination meta { get; set; }
        public List<ApplicationMilestone> data { get; set; }
    }
}
