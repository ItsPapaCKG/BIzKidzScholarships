using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Models
{
    public class PresignedPostURLDataModel : ResponseModel
    {
        public string? Url { get; set; }
        public string? Key { get; set; }
        public Dictionary<string, string>? Fields { get; set; }
    }
}
