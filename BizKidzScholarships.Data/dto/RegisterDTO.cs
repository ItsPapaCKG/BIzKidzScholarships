using BizKidzScholarships.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace BizKidzScholarships.Data.dto
{
    public class RegisterDTO : LoginDTO
    {
        public required UserType UserType { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public DateTimeOffset Birthday { get; set; }

        [Phone]
        [MinLength(10)]
        public required string PhoneNumber { get; set; }
        public string ConfirmPassword { get; set; }

        public bool PrivacyConsent { get; set; } = false;

        public bool IAmOver13 { get; set; } = false;

        public bool MediaConsent { get; set; } = false;
    }
}
