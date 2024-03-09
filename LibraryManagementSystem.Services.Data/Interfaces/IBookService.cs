using LibraryManagementSystem.Web.ViewModels.Book;
using LibraryManagementSystem.Web.ViewModels.Home;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<IndexViewModel>> LastTenBooksAsync();

        Task<IEnumerable<AllBooksViewModel>> GetAllBooksAsync();

        Task<BookFormModel> GetNewCreateBookModelAsync();

        Task AddBookAsync(BookFormModel model);

        Task<BookFormModel?> GetBookForEditByIdAsync(string id);

        Task EditBookAsync(string bookId, BookFormModel model);

        Task<IEnumerable<BookSelectForEditionFormModel>>GetAllBooksForListAsync();

        Task<BookDetailsViewModel> GetBookDetailsForUserAsync(string id);
    }
}
