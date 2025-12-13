using BizKidzScholarships.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.Entities
{
    public class ConfigurationItem : BaseTrackableModel
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
}
