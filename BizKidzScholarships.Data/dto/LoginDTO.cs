using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.dto
{
    public class LoginDTO
    {
        [EmailAddress]
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
