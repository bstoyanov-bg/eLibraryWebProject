using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.ViewModels.LendedBooks;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ILendedBooksService
    {
        Task AddBookToCollectionAsync(string userId, Book book);

        Task<bool> DoesUserHaveBookInCollectionAsync(string userId, string bookId);

        Task<IEnumerable<MyBooksViewModel>> GetMyBooksAsync(string userId);

        Task<bool> CkeckIfBookIsAlreadyAddedToUserCollection(string userId, Book book);

        Task<int> GetCountOfActiveBooksForUserAsync(string userId);
    }
}
