using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Models.Book;
using LibraryManagementSystem.Web.ViewModels.Book;
using LibraryManagementSystem.Web.ViewModels.Home;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IBookService
    {
        // ready
        Task AddBookAsync(BookFormModel model);

        // ready
        Task EditBookAsync(string bookId, BookFormModel model);

        // ready
        Task DeleteBookAsync(string bookId);

        // ready
        Task<Book?> GetBookByIdAsync(string bookId);

        // ready
        Task<BookFormModel> GetCreateNewBookModelAsync();

        // ready
        Task<BookFormModel?> GetBookForEditByIdAsync(string bookId);

        // ready
        Task<BookDetailsViewModel> GetBookDetailsForUserAsync(string bookId);

        // ready
        Task<AllBooksFilteredAndPagedServiceModel> GetAllBooksFilteredAndPagedAsync(AllBooksQueryModel queryModel);

        // ready
        Task<bool> BookExistByIdAsync(string bookId);

        // ready
        Task<bool> BookExistByTitleAndAuthorIdAsync(string title, string authorId);


        Task<bool> HasUserRatedBookAsync(string userId, string bookId);

        // ready
        Task<IEnumerable<IndexViewModel>> LastNineBooksAsync();

        // ready
        Task<IEnumerable<BookSelectForEditionFormModel>>GetAllBooksForListAsync();
    }
}
