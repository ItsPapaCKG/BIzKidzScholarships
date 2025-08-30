using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizKidzScholarships.Data.Entities
{
    public class TaskSubmission
    {
        [Key]
        public required int AttemptNumber { get; set; }

        [ForeignKey("User")]
        public required Guid UserId { get; set; }

        public required IdentityUser<Guid> User { get; set; }

        [ForeignKey("Task")]
        public required int TaskId { get; set; }

        public required TaskItem Task { get; set; }

        [MaxLength(50)]
        public string? FieldName { get; set; }

        [MaxLength(256)]
        public required string SubmissionData { get; set; }
    }
}
