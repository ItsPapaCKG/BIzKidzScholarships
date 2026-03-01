using BizKidzScholarships.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Models
{
    public class UploadHandshakeConfirmationModel
    {
        public required Guid RequestId { get; set; }
        public required RequestStatus Status { get; set; }

        public required int ConsentId { get; set; }
    }
}
