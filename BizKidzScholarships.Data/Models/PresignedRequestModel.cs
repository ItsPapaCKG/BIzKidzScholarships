using BizKidzScholarships.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Models
{
    public class PresignedRequestModel
    {
        public ActionType ActionType { get; set; }
        public int? TaskId { get; set; }
        public required string extension { get; set; }
    }
}
