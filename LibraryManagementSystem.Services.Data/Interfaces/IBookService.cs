using LibraryManagementSystem.Web.ViewModels.Book;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IBookService
    {
        Task CreateBookAsync(AddBookInputModel addBookInputModel);
    }
}
