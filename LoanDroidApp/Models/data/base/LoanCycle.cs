using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.data
{
    public enum LoanCycle
    {
        SEMANAL,
        QUINCENAL,
        MENSUAL,
        DIARIO,
    }
    public static class LoanCycleCalculator { 
        public static DateTime NextDate (LoanCycle cycle,DateTime current){
            if (cycle == LoanCycle.SEMANAL) return current.AddDays(7);
            if (cycle == LoanCycle.QUINCENAL) return current.AddDays(15);
            if (cycle == LoanCycle.MENSUAL) return current.AddMonths(1);
            if (cycle == LoanCycle.DIARIO) return current.AddDays(1);
            return current;
        }
    }
}
