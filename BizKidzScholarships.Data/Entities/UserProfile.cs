using BizKidzScholarships.Data.Base;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BizKidzScholarships.Data.Entities
{
    public class UserProfile : BaseTrackableModel
    {
        public required Guid UserId { get; set; }

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

        public required IdentityUser<Guid> User { get; set; }
    }
}
