using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.dto
{
    public class SubmissionListItem
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }

        public Guid SubmissionId { get; set; }
        public string UserFullName { get; set; }
        public DateTimeOffset SubmittedOn { get; set; }
    }

    public class SubmissionListResponse
    {
        public List<SubmissionListItem> Items { get; set; }
    }
}
