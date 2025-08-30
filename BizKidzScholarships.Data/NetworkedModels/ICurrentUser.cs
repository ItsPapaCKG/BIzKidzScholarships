using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.NetworkedModels
{
    public interface ICurrentUser
    {
        string? Id { get; }

        bool IsAuthenticated { get; }
    }
}
