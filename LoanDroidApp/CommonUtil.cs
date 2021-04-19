using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanDroidApp
{
    public static class CommonUtil
    {
        public static byte[] DecodeUrlBase64(string s)
        {
            s = s.Split(",").Length > 1 ? s.Split(",")[1] : s;
            s = s.Replace('-', '+').Replace('_', '/').PadRight(4 * ((s.Length + 3) / 4), '=');
            return Convert.FromBase64String(s);
        }
    }
}
