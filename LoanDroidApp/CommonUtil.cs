using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


using DBSetup.Interfaces;
using Models.data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        public static bool hasPermission(IEnumerable<Claim> claims, string claim) {
            return claims.Where(u => u.Value.Equals(claim)).Count() > 0 ? true : false;
        }
    }
}
