using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.dto
{
    public class UserPointsView
    {
        public int UserId { get; set; }

        public int Points { get; set; }

        public int Entries { get; set; }

        public DateTime Updated { get; set; }
    }
}
