using Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class DatatableRole: IDatatable
    {
        public DatatablePagination meta { get; set; }
        public List<ApplicationRole> data { get; set; }
    }
}
