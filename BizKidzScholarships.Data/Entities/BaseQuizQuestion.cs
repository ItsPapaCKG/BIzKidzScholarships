using BizKidzScholarships.Data.Base;
using BizKidzScholarships.Data.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Entities
{
    public class BaseQuizQuestion : BaseTrackableModel
    {
        public int QuestionId { get; set; }

        public virtual string Prompt { get; set; }

        public string? PromptImageKey { get; set; }

        public bool Multi { get; set; } = false;
    }

    public class QuizQuestion: BaseQuizQuestion
    {
        public override required string Prompt { get; set; }
    }

    public class QuizOption : BaseTrackableModel
    {
        public int OptionId { get; set; }

        public required string OptionKey { get; set; }

        public required string OptionValue { get; set; }

    }

    public class QuestionOption
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
    }

    public class TaskQuestion
    {
        public int QuestionId { get; set; }
        public int TaskId { get; set; }
    }

    public class QuizQuestionViewModel : BaseQuizQuestion
    {
        public QuizQuestionViewModel()
        {
            Options = new Dictionary<string, string>();
        }
        public Dictionary<string, string> Options { get; set; }
    }

    public class Quiz : ResponseModel
    {
        public int TaskId { get; set; }

        public QuizQuestionViewModel[] Questions { get; set; }
    }
}
