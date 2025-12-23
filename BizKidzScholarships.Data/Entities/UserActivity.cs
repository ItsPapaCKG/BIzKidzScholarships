using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Entities
{
    public class UserActivity
    {
        public string FullName { get; set; }

        public string Task { get; set; }

        public int Reward { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
