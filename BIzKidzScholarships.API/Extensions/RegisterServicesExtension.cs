using AutoMapper;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.NetworkedModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Runtime.CompilerServices;

namespace BIzKidzScholarships.API.Extensions
{
    public static class RegisterServicesExtension
    {

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddTransient<IMapper, Mapper>();

            services.AddAuthorization();
            services.AddIdentityApiEndpoints<IdentityUser<Guid>>()
                .AddEntityFrameworkStores<BizKidzDbContext>();

            services.AddIdentityCore<IdentityUser<Guid>>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-";

                    options.Password.RequiredLength = 9;
                    options.Password.RequiredUniqueChars = 2;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<BizKidzDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();
        }
    }
}
