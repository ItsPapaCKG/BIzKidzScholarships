using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using TaskStatus = BizKidzScholarships.Data.Enums.TaskStatus;

namespace BizKidzScholarships.Data.Entities
{
    [PrimaryKey(nameof(UserId), nameof(TaskId))]
    public class UserTask
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("Task")]
        public int TaskId { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Disabled;

        public IdentityUser<Guid> User { get; set; }

        public TaskItem Task { get; set; }
    }
}
