using AutoMapper;
using BizKidzScholarships.API.Services;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.NetworkedModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace BizKidzScholarships.API.Extensions
{
    public static class RegisterServicesExtension
    {

        public static void RegisterServices(this IServiceCollection services)
        {
#if DEBUG
            var aws_access = Environment.GetEnvironmentVariable("AWS_Access");
            var aws_secret = Environment.GetEnvironmentVariable("AWS_Secret");
#endif

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddScoped<IUserDataService, UserDataService>();

            services.AddTransient<IMapper, Mapper>();

            //services.AddIdentityCore<IdentityUser<Guid>>(options =>
            //{
            //    options.User.RequireUniqueEmail = true;

            //    options.Password.RequiredLength = 9;
            //    options.Password.RequiredUniqueChars = 2;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequireNonAlphanumeric = true;

            //    //options.Stores.ProtectPersonalData = true;
            //})

            //    .AddRoles<IdentityRole<Guid>>()
            //    .AddEntityFrameworkStores<BizKidzDbContext>()
            //    .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            //    .AddSignInManager<SignInManager<IdentityUser<Guid>>>()
            //    .AddDefaultTokenProviders();

            //services.AddAuthorization();
            //services.AddIdentityApiEndpoints<IdentityUser<Guid>>()
            //    .AddEntityFrameworkStores<BizKidzDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
                options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
            });

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

            services.AddIdentityCore<IdentityUser<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 9;
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
    .AddRoles<IdentityRole<Guid>>()                                    // Add role subsystem
    .AddEntityFrameworkStores<BizKidzDbContext>()                      // Register UserStore + RoleStore + UserRoleStore
    .AddRoleManager<RoleManager<IdentityRole<Guid>>>()                 // RoleManager requires RoleStore
    .AddSignInManager<SignInManager<IdentityUser<Guid>>>()             // Cookie login requires SignInManager
    .AddDefaultTokenProviders();

            // Claims mapping
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
                options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
            });

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

            // Cookie authentication (CRITICAL)
            services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme, options =>
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

            services.AddAuthorization();

            services.AddScoped<AdminService>();
        }
    }
}
