using System.ComponentModel.DataAnnotations;

namespace BizKidzScholarships.Data.dto
{
    public class RegisterDTO : LoginDTO
    {

        [Phone]
        [MinLength(10)]
        public required string PhoneNumber { get; set; }
    }
}
