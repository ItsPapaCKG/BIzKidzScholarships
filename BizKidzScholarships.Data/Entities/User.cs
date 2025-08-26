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

        public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();

        public required UserProfile Profile { get; set; }
    }
}
