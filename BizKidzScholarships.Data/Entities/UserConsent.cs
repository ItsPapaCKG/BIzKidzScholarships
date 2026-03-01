using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Entities
{
    public class UserConsent
    {
        public int Id { get; set; }
        public string? UserAgent { get; set; }
        public string? IPAddress { get; set; }

        public required DateTimeOffset ConsentTimeUtc { get; set; }

        public required ConsentType ConsentType { get; set; }
        public required int DocumentId { get; set; } // record that includes version and exact text

        public SiteDocument Document { get; set; }

        public required Guid UserId { get; set; }

        public required bool IsGranted { get; set; } = false;

        public Guid? SubmissionId { get; set; }
    }

    public enum ConsentType
    {
        MediaConsent,
        PrivacyPolicy,
        TermsOfService
    }

    public class SiteDocument
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string ContentsHTML { get; set; }

        public ConsentType Type { get; set; }

        public string Version { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
