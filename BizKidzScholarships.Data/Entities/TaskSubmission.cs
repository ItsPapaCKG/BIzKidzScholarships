using BizKidzScholarships.Data.Base;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizKidzScholarships.Data.Entities
{
    public class TaskSubmission : BaseTrackableModel
    {
        public Guid SubmissionId { get; set; }

        public required int AttemptNumber { get; set; }

        public required Guid UserId { get; set; }

        public IdentityUser<Guid> User { get; set; }

        public required int TaskId { get; set; }

        public TaskItem Task { get; set; }

        [MaxLength(512)]
        public required string SubmissionData { get; set; }
    }
}
