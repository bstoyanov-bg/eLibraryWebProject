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
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return string.Empty;
            }

            return $"{user.FirstName} {user.LastName}";
        }
    }
}
