using BizKidzScholarships.Data.dto;

namespace BizKidzScholarships.Data.Models
{
    public class PresignedPostURLDataModel : ResponseModel
    {
        public string? Url { get; set; }
        public string? Key { get; set; }
        public Dictionary<string, string>? Fields { get; set; }
    }
}
