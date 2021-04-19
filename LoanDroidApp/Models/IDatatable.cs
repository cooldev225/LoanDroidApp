using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class DatatablePagination
    {
        public int total { get; set; }
        public int page { get; set; }
        public int perpage { get; set; }
        public int pages { get; set; }
    }
    public class DatatableSort
    {
        public string field { get; set; }
        public string sort { get; set; }
    }
    public class DatatableParams
    {
        public DatatablePagination pagination { get; set; }
        public DatatableSort sort { get; set; }
    }
    public interface IDatatable
    {
        public DatatablePagination meta { get; set; }
    }
}
