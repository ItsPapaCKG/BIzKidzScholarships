using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.NetworkedModels
{
    public interface ICurrentUser
    {
        Guid Id { get; }

        bool IsAuthenticated { get; }

        string Email { get; }

        Task<IList<string>> GetRolesAsync();
    }
}
