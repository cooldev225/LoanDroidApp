using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

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
        public static bool hasPermission(IEnumerable<Claim> claims, string claim) {
            return claims.Where(u => u.Value.Equals(claim)).Count() > 0 ? true : false;
        }
        public static string DateTimeToFriendly(DateTime date) {
            if (date.Year < 1000) return "";
            return date.Day + "/" + date.Month + "/" + date.Year;
        }
        public static string UTFToUnicode(string str)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(str);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            return iso.GetString(isoBytes);
        }
        public static string UnicodeToUtf(string str)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = iso.GetBytes(str);
            byte[] isoBytes = Encoding.Convert(iso, utf8, utfBytes);
            return utf8.GetString(isoBytes);
        }
    }
}
