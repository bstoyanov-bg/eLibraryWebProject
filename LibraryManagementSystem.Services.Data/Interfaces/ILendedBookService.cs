using LibraryManagementSystem.Web.ViewModels.LendedBook;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ILendedBookService
    {
        Task AddBookToCollectionAsync(string userId, string bookId);

        Task ReturnBookAsync(string userId, string bookId);

        Task ReturnAllBooksAsync(string userId);

        Task<bool> IsBookActiveInUserCollectionAsync(string userId, string bookId);

        Task<bool> BookExistsInUserHistoryCollectionAsync(string userId, string bookId);

        Task<bool> IsBookReturnedAsync(string userId, string bookId);

        Task<bool> AreThereAnyNotReturnedBooksAsync(string userId);

        Task<int> GetCountOfActiveBooksForUserAsync(string userId);

        Task<int> GetCountOfPeopleReadingTheBookAsync(string bookId);

        Task<int> GetCountOfLendedBooksAsync();

        Task<IEnumerable<MyBooksViewModel>> GetMyBooksAsync(string userId);
    }
}
