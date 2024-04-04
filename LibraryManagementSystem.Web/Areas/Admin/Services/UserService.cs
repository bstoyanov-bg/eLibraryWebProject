using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Models.User;
using LibraryManagementSystem.Web.Areas.Admin.Services.Interfaces;
using LibraryManagementSystem.Web.Areas.Admin.ViewModels.User;
using LibraryManagementSystem.Web.Areas.Admin.ViewModels.User.Enums;
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

        public async Task<AllUsersFilteredAndPagedServiceModel> GetAllUsersFilteredAndPagedAsync(AllUsersQueryModel queryModel)
        {
            IQueryable<ApplicationUser> usersQuery = this.dbContext
                .Users
                .Where(u => u.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";

                usersQuery = usersQuery
                    .Where(u => EF.Functions.Like(u.FirstName + " " + u.LastName, wildCard) ||
                                EF.Functions.Like(u.Email, wildCard));
            }

            usersQuery = queryModel.UserSorting switch
            {
                UserSorting.Newest => usersQuery
                    .OrderByDescending(b => b.CreatedOn),
                UserSorting.Oldest => usersQuery
                    .OrderBy(b => b.CreatedOn),
                UserSorting.ByNameAscending => usersQuery
                    .OrderBy(b => b.FirstName + " " + b.LastName),
                UserSorting.ByNameDescending => usersQuery
                    .OrderByDescending(b => b.FirstName + " " + b.LastName),
                _ => usersQuery
                    .OrderBy(b => b.FirstName + " " + b.LastName),
            };

            IEnumerable<AllUsersViewModel> allUsers = await usersQuery
                .Skip((queryModel.CurrentPage - 1) * queryModel.UsersPerPage)
                .Take(queryModel.UsersPerPage)
                .Select(u => new AllUsersViewModel
                {
                    Id = u.Id.ToString(),
                    Email = u.Email!,
                    UserName = u.UserName!,
                    FullName = u.FirstName + " " + u.LastName,
                    City = u.City,
                    PhoneNumber = u.PhoneNumber!,
                    MaxLoanedBooks = u.MaxLoanedBooks
                }).ToListAsync();

            int totalUsers = usersQuery.Count();

            return new AllUsersFilteredAndPagedServiceModel()
            {
                TotalUsersCount = totalUsers,
                Users = allUsers,
            };
        }
    }
}
