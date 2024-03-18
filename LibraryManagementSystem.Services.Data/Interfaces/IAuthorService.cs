using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Models.Author;
using LibraryManagementSystem.Web.ViewModels.Author;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IAuthorService
    {
        // ready
        Task AddAuthorAsync(AuthorFormModel model);

        // ready
        Task EditAuthorAsync(string authorId, AuthorFormModel model);

        // ready
        Task DeleteAuthorAsync(string authorId);

        // ready
        Task<Author?> GetAuthorByIdAsync(string authorId);

        // ready
        Task<AuthorFormModel?> GetAuthorForEditByIdAsync(string authorId);

        // ready
        Task<AuthorDetailsViewModel> GetAuthorDetailsForUserAsync(string authorId);

        // ready
        Task<AllAuthorsFilteredAndPagedServiceModel> GetAllAuthorsFilteredAndPagedAsync(AllAuthorsQueryModel queryModel);

        // ready
        Task<bool> AuthorExistByNameAndNationalityAsync(string firstName, string lastName, string nationality);

        // ready
        Task<bool> AuthorExistByIdAsync(string authorId);

        // ready
        Task<IEnumerable<AuthorsSelectForBookFormModel>> GetAllAuthorsForListAsync();

        // NOT USED ANYMORE
        //Task<IEnumerable<AllAuthorsViewModel>> GetAllAuthorsAsync();
    }
}
