using LibraryManagementSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using LibraryManagementSystem.Data.Seeding.Contracts;
using static LibraryManagementSystem.Common.UserRoleNames;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;

namespace LibraryManagementSystem.Data.Seeding
{
    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ELibraryDbContext dbContext, IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (await dbContext.Users.AnyAsync())
            {
                return;
            }

            ApplicationUser admin = new ApplicationUser
            {
                Id = Guid.Parse("47C46DB7-E6F5-439C-88A8-CD144C55349A"),
                FirstName = "AdminFirst",
                LastName = "AdminLast",
                DateOfBirth = DateOnly.ParseExact("01.01.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Address = "Admin Address",
                Country = "Admin Country",
                City = "Admin City",
                UserName = "Admin-Username",
                PasswordHash = "pass.123",
                Email = DevelopmentAdminEmail,
                PhoneNumber = "111222333",
                IsDeleted = false,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            ApplicationUser user = new ApplicationUser
            {
                Id = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
                FirstName = "Dimitar",
                LastName = "Todorov",
                DateOfBirth = DateOnly.ParseExact("05.10.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Address = "Gurko 5",
                Country = "Bulgaria",
                City = "Sofia",
                UserName = "User-Username",
                MaxLoanedBooks = 5,
                PasswordHash = "pass.123",
                Email = TestUserEmail,
                PhoneNumber = "444555666",
                IsDeleted = false,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            await userManager.CreateAsync(admin, admin.PasswordHash);
            await userManager.CreateAsync(user, user.PasswordHash);

            await userManager.AddToRoleAsync(admin, AdminRole);
            await userManager.AddToRoleAsync(user, UserRole);
        }
    }
}
