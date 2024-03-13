using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Models.Book;
using LibraryManagementSystem.Web.ViewModels.Author;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IAuthorService
    {
        Task AddAuthorAsync(AuthorFormModel model);

        Task<IEnumerable<AllAuthorsViewModel>> GetAllAuthorsAsync();

        Task<AllAuthorsFilteredAndPagedServiceModel> GetAllAuthorsFilteredAndPagedAsync(AllAuthorsQueryModel queryModel);

        Task<Author?> GetAuthorByIdAsync(string authorId);

        Task<AuthorFormModel?> GetAuthorForEditByIdAsync(string authorId);

        Task EditAuthorAsync(string authorId, AuthorFormModel model);

        Task<IEnumerable<AuthorSelectForBookFormModel>> GetAllAuthorsForListAsync();

        Task<AuthorDetailsViewModel?> GetAuthorDetailsForUserAsync(string authorId);

        Task DeleteAuthorAsync(string authorId);
    }
}
