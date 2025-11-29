using BizKidzScholarships.Data.Base;
using BizKidzScholarships.Data.Enums;
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

        public required TaskType TaskType { get; set; } = TaskType.FileUpload;

        public required int Reward { get; set; }

        public required string TaskPromptTitle { get; set; }

        public required string TaskPromptSubtitle { get; set; }

        public bool IsGlobalTask { get; set; } = true;

        public ICollection<TaskSubmission> TaskSubmissions { get; set; } = [];

        public ICollection<UserPointsReward> Rewards { get; set; } = [];

    }
}
