using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.NetworkedModels;
using System.Runtime.CompilerServices;

namespace BIzKidzScholarships.API.Extensions
{
    public static class RegisterServicesExtension
    {

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();
        }
    }
}
