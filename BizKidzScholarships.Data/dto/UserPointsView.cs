using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.dto
{
    public class UserPointsView
    {
        public Guid UserId { get; set; }

        public int Points { get; set; }
        public int PreviousPoints { get; set; }

        public int Entries { get; set; }
        public int PreviousEntries { get; set; }

        public DateTimeOffset Updated { get; set; }
    }
}
