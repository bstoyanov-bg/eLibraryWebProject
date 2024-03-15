using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.LendedBooks;
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
                var userBook = new LendedBooks
                {
                    LoanDate = DateTime.UtcNow,
                    BookId = book.Id,
                    UserId = Guid.Parse(userId),
                };

                await dbContext.LendedBooks.AddAsync(userBook);
                await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DoesUserHaveBookInCollectionAsync(string userId, string bookId)
        {
            bool result = await this.dbContext
                .LendedBooks
                .AnyAsync(lb => lb.BookId.ToString() == bookId &&
                                lb.UserId.ToString() == userId);

            return result;
        }

        public async Task<IEnumerable<MyBooksViewModel>> GetMyBooksAsync(string userId)
        {
            return await dbContext
                .LendedBooks
                .Where(lb => lb.UserId.ToString() == userId)
                .Select(b => new MyBooksViewModel
                {
                    Id = b.BookId.ToString(),
                    Title = b.Book.Title,
                    YearPublished = b.Book.YearPublished,
                    Publisher = b.Book.Publisher,
                    AuthorName = $"{b.Book.Author.FirstName} {b.Book.Author.LastName}",
                    Category = b.Book.BooksCategories.Select(bc => bc.Category.Name).First(),
                    ImageURL = b.Book.CoverImagePathUrl,
                    EditionsCount = b.Book.Editions.Count(),
                    FilePath = b.Book.FilePath,
                }).ToListAsync();
        }

        public async Task<bool> CkeckIfBookIsAlreadyAddedToUserCollection(string userId, Book book)
        {
            return await this.dbContext.LendedBooks
                .AnyAsync(lb => lb.User.Id.ToString() == userId && lb.BookId == book.Id);
        }

        public async Task<int> GetCountOfActiveBooksForUserAsync(string userId)
        {
            return await this.dbContext
                .LendedBooks
                .AsNoTracking()
                .Where(lb => lb.UserId.ToString() == userId &&
                             lb.ReturnDate == null)
                .CountAsync();
        }
    }
}
