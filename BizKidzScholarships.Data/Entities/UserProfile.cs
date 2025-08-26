using BizKidzScholarships.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizKidzScholarships.Data.Entities
{
    public class UserProfile : BaseTrackableModel
    {
        [Key]
        public required int UserId { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? PhoneNumber { get; set; }
        public string? BusinessEmail { get; set; }

        public string? BusinessName { get; set; }

        public string? BusinessLogoKey { get; set; }

        public int? Score { get; set; }

        public required User User { get; set; }
    }
}
