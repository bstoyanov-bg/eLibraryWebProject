using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Data
{
    public class UserService : IUserService
    {
        private readonly ELibraryDbContext dbContext;

        public UserService(ELibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string?> GetFullNameByUsernameAsync(string username)
        {
            ApplicationUser? user = await this.dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return string.Empty;
            }

            return $"{user.FirstName} {user.LastName}";
        }

        public async Task<bool> IsUserDeletedAsync(string userId)
        {
            return await this.dbContext
                .Users
                .AsNoTracking()
                .AnyAsync(u => u.Id.ToString() == userId && 
                               u.IsDeleted == true);
        }
    }
}
