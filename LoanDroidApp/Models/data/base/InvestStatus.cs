using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.data
{
    public enum InvestStatus
    {
        Debug_Processing,
        Debug_rejected,
        Collection_Processing,
        Created_Milestone,
        Completed_Payment,
        Saving_Process,
        Incompleted_Investment,
        Completed_Investment
    }
}
