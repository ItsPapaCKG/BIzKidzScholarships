using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Models
{
    public class PresignedHandshakeModel : ResponseModel
    {
        public Guid RequestId { get; set; }

        public PresignedPostURLDataModel PresignedUrlPayload { get; set; }
    }
}
