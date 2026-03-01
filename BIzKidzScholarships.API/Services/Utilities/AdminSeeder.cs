using BizKidzScholarships.Data.dto;
using Microsoft.AspNetCore.Identity;

namespace BizKidzScholarships.API.Services.Utilities
{
    public static class IdentitySeeder
    {
        public static async Task SeedAdminUser(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var userService = scope.ServiceProvider.GetRequiredService<IUserDataService>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser<Guid>>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            string adminEmail = "grantsputnam@gmail.com";
            string adminPassword = "Password1!";
            string adminPhoneNumber = "2392856774";

            string jdEmail = "jd@bizkidzusa.org";
            string jdPassword = "Password1!";
            string jdPhoneNumber = "2396751235";

            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Kid"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Kid"));
            }

            // Check if user exists
            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                user = new IdentityUser<Guid>
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    PhoneNumber = adminPhoneNumber
                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    await userManager.AddToRoleAsync(user, "Kid");

                    var grantProfile = new RegisterDTO()
                    {
                        FirstName = "Grant",
                        LastName = "Putnam",
                        PhoneNumber = "2392856774",
                        Email = adminEmail,
                        UserType = Data.Enums.UserType.Parent,
                        Password = ""
                    };

                    await userService.SetGlobalTasksForUser(user.Id);

                    await userService.RegisterUserProfile(user.Id, grantProfile);
                }
                else
                {
                    throw new Exception("Failed to create admin user");
                }
            }

            var jduser = await userManager.FindByEmailAsync(jdEmail);

            if (jduser == null)
            {
                jduser = new IdentityUser<Guid>
                {
                    UserName = jdEmail,
                    Email = jdEmail,
                    PhoneNumber = jdPhoneNumber
                };

                var result = await userManager.CreateAsync(jduser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(jduser, "Admin");
                    await userManager.AddToRoleAsync(jduser, "Kid");

                    var jdProfile = new RegisterDTO()
                    {
                        FirstName = "JD",
                        LastName = "Ribali",
                        PhoneNumber = jdPhoneNumber,
                        Email = jdEmail,
                        UserType = Data.Enums.UserType.Parent,
                        Password = ""
                    };

                    await userService.SetGlobalTasksForUser(jduser.Id);

                    await userService.RegisterUserProfile(jduser.Id, jdProfile);
                }
                else
                {
                    throw new Exception("Failed to create admin user");
                }
            }
        }
    }
}
