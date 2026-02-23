using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Models.SaveModels
{
    public class TaskSaveModel
    {
        public TaskSaveModel()
        {
            AssignedUsers = new List<string>();
        }

        public int Id { get; set; }

        public required string TaskNameInternal { get; set; }

        [MaxLength(150)]
        public required string TaskTitle { get; set; }

        public string TaskDescription { get; set; } = string.Empty;

        public string? TaskImageKey { get; set; }

        public required Boolean TaskEnabled { get; set; } = false;

        public required TaskType TaskType { get; set; } = TaskType.FileUpload;

        public required int Reward { get; set; }

        public required string TaskPromptTitle { get; set; }

        public required string TaskPromptSubtitle { get; set; }

        public bool IsGlobalTask { get; set; } = true;

        public List<string> AssignedUsers { get; set; }
    }
}
