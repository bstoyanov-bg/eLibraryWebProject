using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Models.Author;
using LibraryManagementSystem.Web.ViewModels.Author;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IAuthorService
    {
        Task EditAuthorAsync(string authorId, AuthorFormModel model);

        Task DeleteAuthorAsync(string authorId);

        Task<Author> AddAuthorAsync(AuthorFormModel model);

        Task<Author?> GetAuthorByIdAsync(string authorId);

        Task<AuthorFormModel?> GetAuthorForEditByIdAsync(string authorId);

        Task<AuthorDetailsViewModel> GetAuthorDetailsForUserAsync(string authorId);

        Task<AllAuthorsFilteredAndPagedServiceModel> GetAllAuthorsFilteredAndPagedAsync(AllAuthorsQueryModel queryModel);

        Task<bool> AuthorExistByNameAndNationalityAsync(string firstName, string lastName, string nationality);

        Task<bool> AuthorExistByIdAsync(string authorId);

        Task<IEnumerable<AuthorsSelectForBookFormModel>> GetAllAuthorsForListAsync();
    }
}
