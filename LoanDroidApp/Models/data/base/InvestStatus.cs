using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.data
{
    public enum InvestStatus
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
        Created_Milestone,
        Completed_Payment,
        Saving_Process,
        Incompleted_Investment,
        Completed_Investment
    }
}
