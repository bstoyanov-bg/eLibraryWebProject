using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Models.Book;
using LibraryManagementSystem.Web.ViewModels.Book;
using LibraryManagementSystem.Web.ViewModels.Home;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IBookService
    {
        Task DeleteBookAsync(string bookId);

        Task<Book> EditBookAsync(string bookId, BookFormModel model);

        Task<Book> AddBookAsync(BookFormModel model);

        Task<Book?> GetBookByIdAsync(string bookId);

        Task<BookFormModel> GetCreateNewBookModelAsync();

        Task<BookFormModel?> GetBookForEditByIdAsync(string bookId);

        Task<BookDetailsViewModel> GetBookDetailsForUserAsync(string bookId);

        Task<AllBooksFilteredAndPagedServiceModel> GetAllBooksFilteredAndPagedAsync(AllBooksQueryModel queryModel);

        Task<bool> BookExistByIdAsync(string bookId);

        Task<bool> BookExistByTitleAndAuthorIdAsync(string title, string authorId);

        Task<bool> BookExistByISBNAsync(string isbn);

        Task<bool> HasUserRatedBookAsync(string userId, string bookId);

        Task<bool> DoesBookHaveUploadedFileAsync(string bookId);

        Task<int> GetCountOfActiveBooksAsync();

        Task<int> GetCountOfDeletedBooksAsync();

        Task<IEnumerable<IndexViewModel>> LastNineBooksAsync();

        Task<IEnumerable<BookSelectForEditionFormModel>>GetAllBooksForListAsync();
    }
}
