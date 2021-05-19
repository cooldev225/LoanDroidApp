using Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class DatatableCompany: IDatatable
    {
        public DatatablePagination meta { get; set; }
        public List<Company> data { get; set; }
    }
}
