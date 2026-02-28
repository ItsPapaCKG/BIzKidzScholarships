using BizKidzScholarships.Data.Base;
using BizKidzScholarships.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Entities
{
    public class UserResult : BaseTrackableModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string? ChildFullName { get; set; }

        public string Email { get; set; }

        public UserType UserType { get; set; }
        public int Points { get; set; }
        public int Entries { get; set; }
    }
}
