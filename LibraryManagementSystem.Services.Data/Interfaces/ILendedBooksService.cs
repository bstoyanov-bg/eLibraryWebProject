using LibraryManagementSystem.Data.Models;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ILendedBooksService
    {
        Task AddBookToCollectionAsync(string userId, Book book);

        Task<bool> DoesUserHaveBookInCollectionAsync(string userId, string bookId);
    }
}
