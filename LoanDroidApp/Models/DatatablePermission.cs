using Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class ApplicationRolePermission { 
        public int Id { get; set; }
        public string Name { get; set; }
        public PageGroup PageGroup { get; set; }
        public string Claims { get; set; }
        public int Order { get; set; }
        public string Action { get; set; }
    }
    public class DatatablePermission: IDatatable
    {
        public DatatablePagination meta { get; set; }
        public List<ApplicationRolePermission> data { get; set; }
    }
}
