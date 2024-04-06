namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IUserService
    {
        Task<string?> GetFullNameByUsernameAsync(string username);

        Task<bool> IsUserDeletedAsync(string userId);
    }
}
