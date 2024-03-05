using LibraryManagementSystem.Data.Seeding.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Data.Models
{
    public class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ELibraryDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            await SeedRoleAsync(roleManager, AdminRole);
            await SeedRoleAsync(roleManager, UserRole);
        }

        private static async Task SeedRoleAsync(RoleManager<IdentityRole<Guid>> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
