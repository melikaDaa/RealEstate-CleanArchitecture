using Microsoft.AspNetCore.Identity;
using RealEstateApp.Infrastructure.Identity.Entities;
using System.Threading.Tasks;

namespace RealEstateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultAdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)

        {
            await EnsureRoleExistsAsync(roleManager, "Admin");
            await EnsureRoleExistsAsync(roleManager, "User");
            await EnsureAdminUserExistsAsync(userManager);
        }
        private static async Task EnsureRoleExistsAsync(RoleManager<IdentityRole> roleManager,string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
               var result= await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    LogErrors(result.Errors, $"Failed to create role: {roleName}");
                }   
            }
        }
        private static async Task EnsureAdminUserExistsAsync(UserManager<ApplicationUser> userManager)
        {
            const string adminEmail = "admin@admin.com";
            const string adminPassword = "AdminPassword123!";

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin User",
                    EmailConfirmed = true,
                };

                var creationResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (creationResult.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (!roleResult.Succeeded)
                    {
                        LogErrors(roleResult.Errors, $"Failed to assign 'Admin' role to user: {adminEmail}");
                    }
                }
                else
                {
                    LogErrors(creationResult.Errors, $"Failed to create admin user: {adminEmail}");
                }
            }
        }

        private static void LogErrors(IEnumerable<IdentityError> errors,string contextMessage)
        {
            foreach (var error in errors)
            {
                Console.WriteLine($"{contextMessage} - Error: {error.Description}");
            }
        }

    }
}
