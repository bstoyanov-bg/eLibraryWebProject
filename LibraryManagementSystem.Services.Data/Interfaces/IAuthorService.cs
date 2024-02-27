using LibraryManagementSystem.Web.ViewModels.Author;
using LibraryManagementSystem.Web.ViewModels.Category;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IAuthorService
    {
        Task AddAuthorAsync(AuthorFormModel model);

        Task<IEnumerable<AllAuthorsViewModel>> GetAllAuthorsAsync();
    }
}
