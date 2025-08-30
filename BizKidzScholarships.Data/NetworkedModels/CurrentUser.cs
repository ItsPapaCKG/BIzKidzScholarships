using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BizKidzScholarships.Data.NetworkedModels
{
    public class CurrentUser : ICurrentUser
    {
        private IHttpContextAccessor _httpContextAccessor;
        public CurrentUser(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }
        public bool IsAuthenticated { get => _httpContextAccessor.HttpContext.User?.Identity?.IsAuthenticated == true; }

        public string Email => IsAuthenticated ? _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)!.Value : string.Empty;

        Guid ICurrentUser.Id => IsAuthenticated ? Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value) : Guid.Empty;

        public async Task<IList<string>> GetRolesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
