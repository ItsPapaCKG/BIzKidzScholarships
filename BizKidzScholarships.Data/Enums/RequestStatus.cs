using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Enums
{
    public enum RequestStatus
    {
        Denied = -1,
        Cancelled,
        Open,
        Pending,
        Closed,
        Success,
        Failed
    }
}
