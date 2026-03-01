using BizKidzScholarships.Data.Base;
using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.dto
{
    public class AdminGetTaskResponse : BaseTrackableModel
    {
        public int TaskId { get; set; }

        public required string TaskNameInternal { get; set; }

        public required string TaskTitle { get; set; }

        public string TaskDescription { get; set; } = string.Empty;

        public string? TaskImageKey { get; set; }

        public required Boolean TaskEnabled { get; set; } = false;

        public required TaskType TaskType { get; set; } = TaskType.FileUpload;

        public required int Reward { get; set; }

        public required string TaskPromptTitle { get; set; }

        public required string TaskPromptSubtitle { get; set; }

        public bool IsGlobalTask { get; set; } = true;
    }
}
