using BizKidzScholarships.Data.Base;
using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.dto
{
    public class UserProfileDTO
    {
        public required Guid UserId { get; set; }

        [MaxLength(30)]
        public required string FirstName { get; set; }

        [MaxLength(40)]
        public required string LastName { get; set; }

        public DateTimeOffset Birthday { get; set; }

        [MaxLength(12)]
        public string? PhoneNumber { get; set; }

        [MaxLength(40)]
        public string? Email { get; set; }

        [MaxLength(60)]
        public string? BusinessName { get; set; }

        [MaxLength(150)]
        public string? BusinessLogoKey { get; set; }

        public bool ProfileComplete { get; set; }

        public required UserType UserType { get; set; }
    }

    // TODO: Include step to upload byte[] image to S3, retrieve the link, and set to profile column LogoKey
    public class UpdateUserProfileDTO : DTOBase
    {
        [MaxLength(30)]
        public string? FirstName { get; set; }

        [MaxLength(40)]
        public string? LastName { get; set; }

        [MaxLength(12)]
        public string? PhoneNumber { get; set; }

        [MaxLength(40)]
        public string? Email { get; set; }

        public DateTimeOffset Birthday { get; set; }

        [MaxLength(60)]
        public string? BusinessName { get; set; }

        [MaxLength(150)]
        public string? BusinessLogoKey { get; set; }
    }
}
