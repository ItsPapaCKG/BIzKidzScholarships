using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Entities
{
    public class TaskSubmission
    {
        [Key]
        public required int Id { get; set; }

        [ForeignKey("User")]
        public required int UserId { get; set; }

        [ForeignKey("Task")]
        public required int TaskId { get; set; }

        [MaxLength(50)]
        public string? FieldName { get; set; }

        [MaxLength(256)]
        public required string SubmissionData { get; set; } 
    }
}
