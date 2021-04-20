using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.data
{
    public class Notification
    {
        public long Id { get; set; }
        public bool Readed { get; set; }
        public int AlertNum { get; set; }
    }
}
