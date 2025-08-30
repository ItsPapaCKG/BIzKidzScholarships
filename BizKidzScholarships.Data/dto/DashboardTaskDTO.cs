using BizKidzScholarships.Data.Entities;
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
        [MaxLength(150)]
        public required string TaskTitle { get; set; }

        [MaxLength(500)]
        public string TaskDescription { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? TaskImageKey { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Disabled;

    }
}
