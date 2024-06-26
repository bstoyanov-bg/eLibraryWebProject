﻿using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.LendedBook;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Data
{
    public class LendedBookService : ILendedBookService
    {
        private readonly ELibraryDbContext dbContext;

        public LendedBookService(ELibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddBookToCollectionAsync(string userId, string bookId)
        {
            LendedBook userBook = new LendedBook
            {
                LoanDate = DateTime.UtcNow,
                BookId = Guid.Parse(bookId),
                UserId = Guid.Parse(userId),
            };

            await this.dbContext.LendedBooks.AddAsync(userBook);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task ReturnBookAsync(string userId, string bookId)
        {
            LendedBook bookToReturn = await this.dbContext
                .LendedBooks
                .Where(lb => lb.UserId.ToString() == userId &&
                             lb.BookId.ToString() == bookId &&
                             lb.ReturnDate == null)
                .FirstAsync();

            bookToReturn.ReturnDate = DateTime.UtcNow;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task ReturnAllBooksAsync(string userId)
        {
            var booksToReturn = await this.dbContext
                .LendedBooks
                .Where(lb => lb.UserId.ToString() == userId &&
                             lb.ReturnDate == null)
                .ToListAsync();

            foreach (LendedBook bookToReturn in booksToReturn)
            {
                bookToReturn.ReturnDate = DateTime.UtcNow;
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsBookActiveInUserCollectionAsync(string userId, string bookId)
        {
            return await this.dbContext
                .LendedBooks
                .AsNoTracking()
                .AnyAsync(lb => lb.BookId.ToString() == bookId &&
                                lb.UserId.ToString() == userId &&
                                lb.ReturnDate == null);

        }

        public async Task<bool> BookExistsInUserHistoryCollectionAsync(string userId, string bookId)
        {
            return await this.dbContext
                .LendedBooks
                .AsNoTracking()
                .AnyAsync(lb => lb.BookId.ToString() == bookId &&
                                lb.UserId.ToString() == userId &&
                                lb.ReturnDate != null);
        }

        public async Task<bool> IsBookReturnedAsync(string userId, string bookId)
        {
            return await this.dbContext
                .LendedBooks
                .AsNoTracking()
                .AnyAsync(lb => lb.User.Id.ToString() == userId &&
                                lb.BookId.ToString() == bookId &&
                                lb.ReturnDate == null);
        }

        public async Task<bool> AreThereAnyNotReturnedBooksAsync(string userId)
        {
            bool notReturnedBook = await this.dbContext
                .LendedBooks
                .AsNoTracking()
                .AnyAsync(lb => lb.UserId.ToString() == userId &&
                                lb.ReturnDate == null);

            return notReturnedBook;
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

        public async Task<int> GetCountOfPeopleReadingTheBookAsync(string bookId)
        {
            return await this.dbContext
                .LendedBooks
                .AsNoTracking()
                .Where(lb => lb.BookId.ToString() == bookId &&
                             lb.ReturnDate == null)
                .CountAsync();
        }

        public async Task<int> GetCountOfLendedBooksAsync()
        {
            return await this.dbContext
                .LendedBooks
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<IEnumerable<MyBooksViewModel>> GetMyBooksAsync(string userId)
        {
            return await this.dbContext
                .LendedBooks
                .AsNoTracking()
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
                    ImageFilePath = b.Book.ImageFilePath,
                    EditionsCount = b.Book.Editions.Count(),
                    FilePath = b.Book.FilePath,
                }).ToListAsync();
        }
    }
}
