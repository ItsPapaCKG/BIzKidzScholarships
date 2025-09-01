using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.dto
{
    public class ResponseModel
    {
        public bool Succeeded { get; set; } = true;

        public List<string> Errors { get; set; } = [];
    }
}
