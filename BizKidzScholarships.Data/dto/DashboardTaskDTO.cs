using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = BizKidzScholarships.Data.Enums.TaskStatus;

namespace BizKidzScholarships.Data.dto
{
    public class DashboardTaskDTO
    {
        public int TaskId { get; set; }

        public required string TaskTitle { get; set; }

        public string TaskDescription { get; set; } = string.Empty;

        public string? TaskImageKey { get; set; }

        public int? Reward { get; set; }

        public TaskType TaskType { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Disabled;

    }
}
