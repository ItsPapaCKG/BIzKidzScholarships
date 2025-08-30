using BizKidzScholarships.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizKidzScholarships.Data.Entities
{
    public class UserProfile : BaseTrackableModel
    {
        [Key]
        public required int UserId { get; set; }

        [MaxLength(30)]
        public required string FirstName { get; set; }

        [MaxLength(40)]
        public required string LastName { get; set; }

        [MaxLength(12)]
        public string? PhoneNumber { get; set; }

        [MaxLength(40)]
        public string? BusinessEmail { get; set; }

        [MaxLength(60)]
        public string? BusinessName { get; set; }

        [MaxLength(150)]
        public string? BusinessLogoKey { get; set; }

        public int? Score { get; set; }

        public required User User { get; set; }
    }
}
