using LibraryManagementSystem.Web.ViewModels.LendedBooks;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ILendedBooksService
    {
        // ready
        Task AddBookToCollectionAsync(string userId, string bookId);

        Task ReturnBookAsync(string userId, string bookId);

        Task ReturnAllBooksAsync(string userId);

        // ready
        Task<bool> IsBookActiveInUserCollectionAsync(string userId, string bookId);

        // ready
        Task<bool> IsBookReturnedAsync(string userId, string bookId);

        // ready
        Task<bool> AreThereAnyNotReturnedBooksAsync(string userId);

        // ready
        Task<int> GetCountOfActiveBooksForUserAsync(string userId);

        // ready
        Task<IEnumerable<MyBooksViewModel>> GetMyBooksAsync(string userId);
    }
}
