using Models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp.Models
{
    public class ApplicationMessage : Message
    {
        public string FromUserName { get; set; }
        public string FromFriendlyName { get; set; }
        public byte[] FromAvatarImage { get; set; }
        public string ToUserName { get; set; }
        public string ToFriendlyName { get; set; }
        public byte[] ToAvatarImage { get; set; }
    }
    public class DatatableMessage: IDatatable
    {
        public DatatablePagination meta { get; set; }
        public List<ApplicationMessage> data { get; set; }
    }
}
