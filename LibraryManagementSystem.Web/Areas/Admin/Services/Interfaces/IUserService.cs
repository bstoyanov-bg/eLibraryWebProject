using LibraryManagementSystem.Web.Areas.Admin.ViewModels;

namespace LibraryManagementSystem.Web.Areas.Admin.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();

        Task DeleteUserAsync(string userId);

        //Task<(ApplicationUser, IdentityResult)> EditUserAsync(string userId, EditUserInputModel editUserInputModel);

        //Task<EditUserInputModel> GetUserForEditByIdAsync(string userId);
    }
}
