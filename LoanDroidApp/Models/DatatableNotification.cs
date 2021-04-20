using Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class ApplicationNotification : Notification{
        public bool IsReaded { get; set; }
        public string Cliam { get; set; }
    }
    public class DatatableNotification : IDatatable
    {
        public DatatablePagination meta { get; set; }
        public List<ApplicationNotification> data { get; set; }
    }
}
