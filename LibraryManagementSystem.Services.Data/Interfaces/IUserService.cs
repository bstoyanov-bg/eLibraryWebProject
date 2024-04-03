using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IUserService
    {
        Task<string?> GetFullNameByUsernameAsync(string username);
    }
}
