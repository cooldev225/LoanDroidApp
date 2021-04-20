using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.data
{
    public enum InvestStatus
    {
        New,        
        Service_Processing,
        Service_rejected,
        Created_Milestone,
        Completed_Payment,
        Debug_Processing,
        Debug_rejected,
        Collection_Processing,
        Collection_Error,
        Saving_Process,
        Incompleted_Investment,
        Completed_Investment
    }
}
