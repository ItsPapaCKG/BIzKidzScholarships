using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Entities
{
    public class UserPointsReward
    {
        public int AwardId { get; set; }

        [Key]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        public int TaskId { get; set; }

        public required int AttemptNumber { get; set; }

        public int Points { get; set; }

        public DateTime Updated { get; set; }

        public required User User { get; set; }

        public required TaskItem Task { get; set; }
    }
}
