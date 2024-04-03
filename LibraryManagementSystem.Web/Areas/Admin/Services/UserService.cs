using LibraryManagementSystem.Common;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.Areas.Admin.Services.Interfaces;
using LibraryManagementSystem.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Web.Areas.Admin.Servicesv
{
    public class UserService : IUserService
    {
        private readonly ELibraryDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(ELibraryDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
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
            ApplicationUser? userToDelete = await GetUserByIdAsync(userId);

            if (userToDelete != null)
            {
                foreach (PropertyInfo property in userToDelete.GetType().GetProperties())
                {
                    Attribute? attribute = Attribute.GetCustomAttribute(property, typeof(ProtectedPersonalDataAttribute));
                    if (attribute != null)
                    {
                        // Clear the property value based on its type
                        if (property.PropertyType == typeof(string) && property.Name == "UserName" || property.Name == "NormalizedUserName")
                        {
                            //property.SetValue(userToDelete, Guid.NewGuid().ToString());
                        }
                        else if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(userToDelete, string.Empty);
                        }
                        else if (property.PropertyType == typeof(int))
                        {
                            property.SetValue(userToDelete, 0);
                        }
                        else if (property.PropertyType == typeof(DateOnly))
                        {
                            property.SetValue(userToDelete, default);
                        }
                    }
                }

                userToDelete!.IsDeleted = true;

                this.dbContext.Users.Update(userToDelete);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            ApplicationUser? user = await this.userManager
               .FindByIdAsync(userId);

            if (user == null)
            {
                return null!;
            }

            return user;
        }

        public async Task<bool> isUserAdmin(string userId)
        {
            return await this.dbContext
                .UserRoles
                .Where(ur => ur.UserId.ToString() == userId)
                .Join(this.dbContext.Roles,
                    ur => ur.RoleId,
                    role => role.Id,
                    (ur, role) => new { UserRole = ur, RoleName = role.Name })
                .AnyAsync(ur => ur.RoleName == UserRole);
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
