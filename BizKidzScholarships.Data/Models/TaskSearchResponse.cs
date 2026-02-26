using BizKidzScholarships.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Models
{
    public class TaskSearchResponse
    {
        public TaskSearchResponse()
        {
            Results = new List<TaskItem>();
        }
        public List<TaskItem> Results { get; set; }

        public string Error { get; set; }
    }
}
