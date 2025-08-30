using BizKidzScholarships.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizKidzScholarships.Data.Entities
{
    public class User : BaseTrackableModel
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public required string Username { get; set; }

        [MaxLength(75)]
        public required string Email { get; set; }

        [MaxLength(512)]
        public required string Password { get; set; }

        public ICollection<UserTask> UserTasks { get; set; } = [];

        public required UserProfile Profile { get; set; }

        public required ICollection<TaskSubmission> TaskSubmissions { get; set; }

        public required ICollection<UserPointsReward> Rewards { get; set; }
             
    }
}
