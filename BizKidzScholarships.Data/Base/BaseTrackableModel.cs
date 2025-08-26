using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Base
{
    public abstract class BaseTrackableModel
    {
        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
