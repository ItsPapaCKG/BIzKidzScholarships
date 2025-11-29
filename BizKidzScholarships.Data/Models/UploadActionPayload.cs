using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Models
{
    public class UploadActionPayload
    {
        public string S3Link { get; set; }

        public int? TaskId { get; set; }
    }
}
