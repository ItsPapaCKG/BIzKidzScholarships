using BizKidzScholarships.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace BizKidzScholarships.Data.Entities
{
    public class TaskItem : BaseTrackableModel
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(60)]
        public required string TaskNameInternal { get; set; }

        [MaxLength(150)]
        public required string TaskTitle { get; set; }

        [MaxLength(500)]
        public string TaskDescription { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? TaskImageKey { get; set; }

        public required Boolean TaskEnabled { get; set; } = false;

        public required ICollection<TaskSubmission> TaskSubmissions { get; set; } = [];

        public required ICollection<UserPointsReward> Rewards { get; set; } = [];

    }
}
