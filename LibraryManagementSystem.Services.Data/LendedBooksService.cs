using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Data
{
    public class LendedBooksService : ILendedBooksService
    {
        private readonly ELibraryDbContext dbContext;

        public LendedBooksService(ELibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddBookToCollectionAsync(string userId, Book book)
        {
            bool alreadyAdded = await dbContext.LendedBooks
                .AnyAsync(lb => lb.User.Id.ToString() == userId && lb.BookId == book.Id);

            if (alreadyAdded == false)
            {
                var userBook = new LendedBooks
                {
                    LoanDate = DateTime.UtcNow,
                    BookId = book.Id,
                    UserId = Guid.Parse(userId),
                };

                await dbContext.LendedBooks.AddAsync(userBook);
                await dbContext.SaveChangesAsync();

            }
        }

        public async Task<bool> DoesUserHaveBookInCollectionAsync(string userId, string bookId)
        {
            bool result = await dbContext
                .LendedBooks
                .AnyAsync(lb => lb.BookId.ToString() == bookId &&
                                lb.UserId.ToString() == userId);

            return result;
        }
    }
}
