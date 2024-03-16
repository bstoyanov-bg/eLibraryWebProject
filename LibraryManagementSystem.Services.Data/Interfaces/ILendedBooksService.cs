using LibraryManagementSystem.Web.ViewModels.LendedBooks;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ILendedBooksService
    {
        Task AddBookToCollectionAsync(string userId, string bookId);

        Task<IEnumerable<MyBooksViewModel>> GetMyBooksAsync(string userId);

        Task<bool> IsBookActiveInUserCollectionAsync(string userId, string bookId);

        Task<int> GetCountOfActiveBooksForUserAsync(string userId);

        Task<bool> IsBookReturnedAsync(string userId, string bookId);

        Task ReturnBookAsync(string userId, string bookId);
    }
}
