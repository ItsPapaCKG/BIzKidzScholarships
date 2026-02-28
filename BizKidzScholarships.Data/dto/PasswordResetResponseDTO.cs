using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.dto
{
    public class PasswordResetResponseDTO : ResponseModel
    {
        public Guid RequestId { get; set; }

        public Guid OneTimeURL { get; set; }
    }
}
