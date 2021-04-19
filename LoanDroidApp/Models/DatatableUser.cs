using Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class DatatableUser: IDatatable
    {
        public DatatablePagination meta { get; set; }
        public List<ApplicationUser> data { get; set; }
    }
}
