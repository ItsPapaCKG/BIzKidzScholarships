using AutoMapper;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.NetworkedModels;
using BIzKidzScholarships.API.Services;
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

            services.AddScoped<IUserDataService, UserDataService>();

            services.AddTransient<IMapper, Mapper>();

            services.AddAuthorization();
            services.AddIdentityApiEndpoints<IdentityUser<Guid>>()
                .AddEntityFrameworkStores<BizKidzDbContext>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "frontend",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:8080",
                                            "https://localhost:50666")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

            services.AddIdentityCore<IdentityUser<Guid>>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequiredLength = 9;
                    options.Password.RequiredUniqueChars = 2;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;

                    //options.Stores.ProtectPersonalData = true;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<BizKidzDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.Name = "BizKidzCookie";
                options.Cookie.SameSite = SameSiteMode.None;

                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

                options.LoginPath = "/auth/login";
                options.AccessDeniedPath = "/auth/accessdenied";
            });
        }
    }
}
