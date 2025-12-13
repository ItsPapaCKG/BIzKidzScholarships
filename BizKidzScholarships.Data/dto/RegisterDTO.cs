using System.ComponentModel.DataAnnotations;

namespace BizKidzScholarships.Data.dto
{
    public class RegisterDTO : LoginDTO
    {

        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public DateTimeOffset Birthday { get; set; }

        [Phone]
        [MinLength(10)]
        public required string PhoneNumber { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
