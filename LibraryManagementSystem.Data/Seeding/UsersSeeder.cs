using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Data.Seeding.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;
using static LibraryManagementSystem.Common.UserRoleNames;

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
                UserName = DevelopmentAdminUsername,
                PasswordHash = "pass.123",
                Email = DevelopmentAdminEmail,
                PhoneNumber = "111222333",
                IsDeleted = false,
                SecurityStamp = "49CC835AFE4D41B5AB5DC8CB6886ACD0",
            };

            ApplicationUser userOne = new ApplicationUser
            {
                Id = Guid.Parse("7F13235C-EAC9-4F60-AA69-BC8FC86FBD24"),
                FirstName = "Dimitar",
                LastName = "Todorov",
                DateOfBirth = DateOnly.ParseExact("05.10.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Address = "Gurko 5",
                Country = "Bulgaria",
                City = "Sofia",
                UserName = TestUserUsername,
                MaxLoanedBooks = MaxNumberOfBooksAllowed,
                PasswordHash = "pass.123",
                Email = TestUserEmail,
                PhoneNumber = "444555666",
                IsDeleted = false,
                SecurityStamp = "6D9FB13AE0D145F496E624C798A5268E",
                CreatedOn = DateTime.UtcNow.AddDays(10),
            };

            ApplicationUser userTwo = new ApplicationUser
            {
                Id = Guid.Parse("89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153"),
                FirstName = "Marina",
                LastName = "Zdravkova",
                DateOfBirth = DateOnly.ParseExact("01.05.1985", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Address = "Hristo Botev 12",
                Country = "Bulgaria",
                City = "Haskovo",
                UserName = "Marina85",
                MaxLoanedBooks = MaxNumberOfBooksAllowed,
                PasswordHash = "pass.123",
                Email = "marina85@abv.bg",
                PhoneNumber = "999666333",
                IsDeleted = false,
                SecurityStamp = "285DD83CB9D44020893880E27B178E15",
                CreatedOn = DateTime.UtcNow.AddDays(20),
            };

            ApplicationUser userThree = new ApplicationUser
            {
                Id = Guid.Parse("BBBDF7F6-94E2-4BE8-A036-84AF63E028FC"),
                FirstName = "Petko",
                LastName = "Georgiev",
                DateOfBirth = DateOnly.ParseExact("12.08.1978", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Address = "Pencho Vladigerov 15",
                Country = "Bulgaria",
                City = "Sofia",
                UserName = "PetkoBaby",
                MaxLoanedBooks = MaxNumberOfBooksAllowed,
                PasswordHash = "pass.123",
                Email = "petkobaby@abv.bg",
                PhoneNumber = "778811547",
                IsDeleted = false,
                SecurityStamp = "618A8DDF778F4DDDB8A5B22CC61D1FAA",
                CreatedOn = DateTime.UtcNow.AddDays(30),
            };

            await userManager.CreateAsync(admin, admin.PasswordHash);
            await userManager.CreateAsync(userOne, userOne.PasswordHash);
            await userManager.CreateAsync(userTwo, userTwo.PasswordHash);
            await userManager.CreateAsync(userThree, userThree.PasswordHash);

            await userManager.AddToRoleAsync(admin, AdminRole);
            await userManager.AddToRoleAsync(userOne, UserRole);
            await userManager.AddToRoleAsync(userTwo, UserRole);
            await userManager.AddToRoleAsync(userThree, UserRole);
        }
    }
}
