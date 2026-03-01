using Amazon;
using Amazon.SimpleEmail;
using AutoMapper;
using BizKidzScholarships.API.Services;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.NetworkedModels;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace BizKidzScholarships.API.Extensions
{
    public static class RegisterServicesExtension
    {

        public static void RegisterServices(this IServiceCollection services, IConfigurationManager config)
        {
#if DEBUG
            var aws_access = Environment.GetEnvironmentVariable("AWS_Access");
            var aws_secret = Environment.GetEnvironmentVariable("AWS_Secret");
#endif

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto;

                options.KnownNetworks.Clear(); // Only if you trust the proxy
                options.KnownProxies.Clear();
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODAzNzcyODAwIiwiaWF0IjoiMTc3MjMyMjA0MSIsImFjY291bnRfaWQiOiIwMTljYTY5ZmU0YmU3NDllYTg0ZTFiOTNkY2RhOGQxNSIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2prYTBieGcxNjc0NXpjcngwM2ZjMDRyIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.aZFzgwo9dmO_5Vpjc88JxkXm3tWRzQSLp9Xb49gNJ78dL696v4yQpVT4zPiZOGCKyG07m1h-EiZf4I2N_ikrNdSHuDG2fdY2nBlI-2f9zqBr6vfrPchJKi0TP-yXITBRxEwUydiywNvzCobpjQF9njfRfluD8y9F7u2H5NEQbM_dm6r96Ttds6o1SrDBBXYt7GFvdn0lJfrV1rYFQMwvRt7Mkvq71aB4Wp910HcVOZ0Vv7G46mlnPcEZbQnUsdEirdfKtzHivDv0fDXmZMbRedKYtRIOjFfL_fxpCRae30zS5VqzxCzu29JxkVLyapea5ZzcMiUZDPqVq5BRSsHgRg";
            });

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddScoped<IUserDataService, UserDataService>();

            services.AddTransient<IMapper, Mapper>();

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
                        policy.WithOrigins(config["FrontEndUrl"]!)
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

            services.AddDefaultAWSOptions(config.GetAWSOptions());
            services.AddAWSService<IAmazonSimpleEmailService>();

            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", config =>
                {
                    config.PermitLimit = 20;              // 20 requests
                    config.Window = TimeSpan.FromMinutes(1); // per minute
                    config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    config.QueueLimit = 2;
                });

                options.AddFixedWindowLimiter("login", config =>
                {
                    config.PermitLimit = 4;              // 10 requests
                    config.Window = TimeSpan.FromMinutes(1); // per minute
                    config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    config.QueueLimit = 2;
                });
            });

            services.AddAuthorization();

            services.AddScoped<AdminService>();
        }
    }
}
