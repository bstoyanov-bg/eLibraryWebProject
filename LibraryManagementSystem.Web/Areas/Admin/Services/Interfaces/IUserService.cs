using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Models.User;
using LibraryManagementSystem.Web.Areas.Admin.ViewModels.User;

namespace LibraryManagementSystem.Web.Areas.Admin.Services.Interfaces
{
    public interface IUserService
    {
        Task DeleteUserAsync(string userId);

        Task<ApplicationUser> GetUserByIdAsync(string userId);

        Task<bool> isUserAdmin(string userId);

        Task<AllUsersFilteredAndPagedServiceModel> GetAllUsersFilteredAndPagedAsync(AllUsersQueryModel queryModel);
    }
}
