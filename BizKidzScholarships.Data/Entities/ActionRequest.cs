using BizKidzScholarships.Data.Base;
using BizKidzScholarships.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Entities
{
    public class ActionRequest : BaseTrackableModel
    {
        [Key]
        public Guid RequestId { get; set; }
        public Guid UserId { get; set; }

        public ActionType ActionType { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public DateTimeOffset Expiration { get; set; }

        public string? Payload { get; set; }

        public IdentityUser<Guid> User { get; set; }
    }
}
