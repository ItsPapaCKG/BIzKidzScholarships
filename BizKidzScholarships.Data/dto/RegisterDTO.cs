using System.ComponentModel.DataAnnotations;

namespace BizKidzScholarships.Data.dto
{
    public class RegisterDTO
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        [Phone]
        [MinLength(10)]
        public required string PhoneNumber { get; set; }
    }
}
