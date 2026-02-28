using BizKidzScholarships.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.dto
{
    public class UserConsentDTO
    {
        public ConsentType ConsentType { get; set; }

        public int DocumentId { get; set; }

        public bool IsGranted { get; set; }
    }
}
