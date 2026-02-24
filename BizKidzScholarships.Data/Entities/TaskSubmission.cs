using BizKidzScholarships.Data.Base;
using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.Enums;
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

    public class AdminTaskSubmissionView
    {
        public required Guid SubmissionId { get; set; }

        public required int AttemptNumber { get; set; }

        public required Guid UserId { get; set; }

        public required int TaskId { get; set; }

        public required TaskType TaskType { get; set; }

        public DateTimeOffset Created { get; set; }
    }

    public class AdminTaskSubmissionsSearchResults : ResponseModel
    {
        public List<AdminTaskSubmissionView> Results { get; set; }

        public AdminTaskSubmissionsSearchResults()
        {
            Results = new List<AdminTaskSubmissionView>();
        }
    }
}
