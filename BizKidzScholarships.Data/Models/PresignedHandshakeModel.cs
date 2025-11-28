using BizKidzScholarships.Data.dto;

namespace BizKidzScholarships.Data.Models
{
    public class PresignedHandshakeModel : ResponseModel
    {
        public Guid RequestId { get; set; }

        public PresignedPostURLDataModel PresignedUrlPayload { get; set; }
    }
}
