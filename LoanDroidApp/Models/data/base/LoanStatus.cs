using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.data
{
    public enum LoanStatus
    {
        New,
        Representante_Processing,
        Representante_Rejected,
        Contactor_Checking,
        Contactor_Rejected,
        Service_Processing,
        Service_Rejected,
        Debug_Processing,
        Debug_Rejected,
        Collection_Processing,
        Investor_Piad,
        Interesting_Process,
        Interesting_Completed,
        Interesting_Incompleted
    }
}
