using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using TaskStatus = BizKidzScholarships.Data.Enums.TaskStatus;

namespace BizKidzScholarships.Data.Entities
{
    [PrimaryKey(nameof(UserId), nameof(TaskId))]
    public class UserTask
    {
        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Task")]
        public int TaskId { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Disabled;

        public required User User { get; set; }

        public required TaskItem Task { get; set; }
    }
}
