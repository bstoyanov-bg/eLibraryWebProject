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

        public async Task AddBookToCollectionAsync(string userId, string bookId)
        {
                var userBook = new LendedBooks
                {
                    LoanDate = DateTime.UtcNow,
                    BookId = Guid.Parse(bookId),
                    UserId = Guid.Parse(userId),
                };

                await dbContext.LendedBooks.AddAsync(userBook);
                await dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsBookActiveInUserCollectionAsync(string userId, string bookId)
        {
            bool result = await this.dbContext
                .LendedBooks
                .AnyAsync(lb => lb.BookId.ToString() == bookId &&
                                lb.UserId.ToString() == userId &&
                                lb.ReturnDate == null);

            return result;
        }

        public async Task<IEnumerable<MyBooksViewModel>> GetMyBooksAsync(string userId)
        {
            return await dbContext
                .LendedBooks
                .Where(lb => lb.UserId.ToString() == userId &&
                             lb.ReturnDate == null)
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

        public async Task<int> GetCountOfActiveBooksForUserAsync(string userId)
        {
            return await this.dbContext
                .LendedBooks
                .AsNoTracking()
                .Where(lb => lb.UserId.ToString() == userId &&
                             lb.ReturnDate == null)
                .CountAsync();
        }

        public async Task<bool> IsBookReturnedAsync(string userId, string bookId)
        {
            return await this.dbContext
                .LendedBooks
                .AnyAsync(lb => lb.User.Id.ToString() == userId &&
                                lb.BookId.ToString() == bookId &&
                                lb.ReturnDate == null);
        }

        public async Task ReturnBookAsync(string userId, string bookId)
        {
            var bookToReturn = await this.dbContext
                .LendedBooks
                .Where(lb => lb.UserId.ToString() == userId &&
                             lb.BookId.ToString() == bookId &&
                             lb.ReturnDate == null)
                .FirstAsync();

            bookToReturn!.ReturnDate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
        }
    }
}
