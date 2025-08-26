using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Enums
{
    public enum TaskStatus
    {
        Hidden = -3,
        Disabled = -2,
        Rejected = -1,
        Open,
        Pending,
        Completed
    }
}
