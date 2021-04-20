using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.data
{
    public enum LoanCycle
    {
        Weekly,
        Monthly,
        Quarter,
        HalfOfYear,
        Annual
    }
    public static class LoanCycleCalculator { 
        public static DateTime NextDate (LoanCycle cycle,DateTime current){
            if (cycle == LoanCycle.Weekly) return current.AddDays(7);
            if (cycle == LoanCycle.Monthly) return current.AddMonths(1);
            if (cycle == LoanCycle.Quarter) return current.AddMonths(3);
            if (cycle == LoanCycle.HalfOfYear) return current.AddMonths(6);
            if (cycle == LoanCycle.Annual) return current.AddYears(1);
            return current;
        }
    }
}
