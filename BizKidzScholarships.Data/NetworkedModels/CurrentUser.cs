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

        public string? Id { get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? _httpContextAccessor.HttpContext.User.FindFirst("sub")?.Value; }
        public bool IsAuthenticated { get => _httpContextAccessor.HttpContext.User?.Identity?.IsAuthenticated == true; }
    }
}
