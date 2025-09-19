using BizKidzScholarships.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace BizKidzScholarships.Data.Entities
{
    public class TaskItem : BaseTrackableModel
    {
        [Key]
        public int Id { get; set; }

        public required string TaskNameInternal { get; set; }

        [MaxLength(150)]
        public required string TaskTitle { get; set; }

        public string TaskDescription { get; set; } = string.Empty;

        public string? TaskImageKey { get; set; }

        public required Boolean TaskEnabled { get; set; } = false;

        public required int Reward { get; set; }

        public ICollection<TaskSubmission> TaskSubmissions { get; set; } = [];

        public ICollection<UserPointsReward> Rewards { get; set; } = [];

    }
}
