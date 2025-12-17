using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.dto
{
    public class UserPointsRecordDTO
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public int Points { get; set; }

        public int Entries { get; set; }

    }
}
