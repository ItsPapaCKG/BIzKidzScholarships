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

        [MaxLength(500)]
        public string TaskDescription { get; set; } = string.Empty;

        public string? TaskImageKey { get; set; }

    }
}
