using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.data
{
    public enum LoanStatus
    {
        New,
        Contactor_Checking,
        Contactor_Rejected,
        Service_Mapping,
        Service_rejected,
        Debug_Processing,
        Debug_rejected,
        Collection_Processing,
        Investor_Piad,
        Interesting_Process,
        Interesting_completed,
        Interesting_Incompleted
    }
}
