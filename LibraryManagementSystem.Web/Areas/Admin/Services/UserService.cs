using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.Areas.Admin.Services.Interfaces;
using LibraryManagementSystem.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Web.Areas.Admin.Services
{
    public class UserService : IUserService
    {
        private readonly ELibraryDbContext dbContext;
        //private readonly IPersonalDataProtector personalDataProtector;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(ELibraryDbContext dbContext, UserManager<ApplicationUser> userManager/*, IPersonalDataProtector personalDataProtector*/)
        {
            this.dbContext = dbContext;
            //this.personalDataProtector = personalDataProtector;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            return await this.dbContext
                .Users
                .AsNoTracking()
                .Where(u => u.IsDeleted == false)
                .Select(u => new UserViewModel
                {
                    Id = u.Id.ToString(),
                    Email = u.Email!,
                    UserName = u.UserName!,
                    FullName = u.FirstName + " " + u.LastName,
                    City = u.City,
                    PhoneNumber = u.PhoneNumber!,
                    MaxLoanedBooks = u.MaxLoanedBooks
                })
                .OrderBy(c => c.FullName)
                .ToListAsync();
        }

        public async Task DeleteUserAsync(string userId)
        {
            ApplicationUser? userToDelete = await dbContext.Users.FindAsync(userId);

            if (userToDelete != null)
            {
                // Clear sensitive personal data
                foreach (var property in typeof(ApplicationUser).GetProperties())
                {
                    var attribute = Attribute.GetCustomAttribute(property, typeof(ProtectedPersonalDataAttribute));
                    if (attribute != null)
                    {
                        // Clear the property value using personal data protector
                        var protectedData = (string?)property.GetValue(userToDelete);
                        if (!string.IsNullOrEmpty(protectedData))
                        {
                            //var unprotectedData = personalDataProtector.Unprotect(protectedData);
                            property.SetValue(userToDelete, null);
                        }
                    }
                }
                userToDelete!.IsDeleted = true;

                dbContext.Users.Update(userToDelete);
                await dbContext.SaveChangesAsync();
            }
        }

        //public Task<EditUserInputModel> GetUserForEditByIdAsync(string userId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<(ApplicationUser, IdentityResult)> EditUserAsync(string userId, EditUserInputModel editUserInputModel)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
