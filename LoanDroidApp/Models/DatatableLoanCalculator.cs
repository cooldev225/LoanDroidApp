using Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class ApplicationLoanCalcuator
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Capital { get; set; }
        public double Interest { get; set; }
        public double Dues { get; set; }
        public double Balance { get; set; }
    }
    public class DatatableLoanCalculator: IDatatable
    {
        public DatatablePagination meta { get; set; }
        public List<ApplicationLoanCalcuator> data { get; set; }
    }
}
