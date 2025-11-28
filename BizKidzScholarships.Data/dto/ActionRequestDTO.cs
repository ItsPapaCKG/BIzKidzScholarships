using BizKidzScholarships.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.dto
{
    public class ActionRequestDTO
    {
        public Guid RequestId { get; set; }

        public Guid UserId { get; set; }

        public ActionType ActionType { get; set; }

        public RequestStatus Status { get; set; }

        public string? Payload { get; set; }
    }
}
