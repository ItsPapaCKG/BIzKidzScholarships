using BizKidzScholarships.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizKidzScholarships.Data.Entities
{
    public class User : BaseTrackableModel
    {
        [Key]
        public int Id { get; set; }

        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public ICollection<UserTask> UserTasks { get; set; } = [];

        public required UserProfile Profile { get; set; }

        public required ICollection<TaskSubmission> TaskSubmissions { get; set; }
    }
}
