using LibraryManagementSystem.Web.ViewModels.Author;
using LibraryManagementSystem.Web.ViewModels.Book;
using LibraryManagementSystem.Web.ViewModels.Home;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<IndexViewModel>> LastTenBooksAsync();

        Task CreateBookAsync(AddBookInputModel addBookInputModel);

        Task<IEnumerable<AllBooksViewModel>> GetAllBooksAsync();
    }
}
