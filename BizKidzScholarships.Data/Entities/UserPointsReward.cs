using BizKidzScholarships.Data.Base;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizKidzScholarships.Data.Entities
{
    public class UserPointsReward : BaseTrackableModel
    {
        public int AwardId { get; set; }

        [Key]
        [ForeignKey("Users")]
        public Guid UserId { get; set; }

        public int? TaskId { get; set; }

        public required int AttemptNumber { get; set; }

        public int Points { get; set; }

        public bool IsNew { get; set; }

        public IdentityUser<Guid> User { get; set; }

        public TaskItem Task { get; set; }
    }
}
