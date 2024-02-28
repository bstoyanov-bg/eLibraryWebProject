using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Author;
using LibraryManagementSystem.Web.ViewModels.Book;
using LibraryManagementSystem.Web.ViewModels.Home;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Data
{
    public class BookService : IBookService
    {
        private readonly ELibraryDbContext dbContext;

        public BookService(ELibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<IndexViewModel>> LastTenBooksAsync()
        {
            IEnumerable<IndexViewModel> lastTenBooks = await dbContext.Books
                .OrderByDescending(h => h.CreatedOn)
                .Take(10)
                .Select(b => new IndexViewModel()
                {
                    Id = b.Id.ToString(),
                    Title = b.Title,
                    Description = b.Description,
                    Author = $"{b.Author.FirstName} {b.Author.LastName}",
                    ImageUrl = b.CoverImagePathUrl ?? string.Empty,
                })
                .ToArrayAsync();

            return lastTenBooks;
        }

        public async Task<IEnumerable<AllBooksViewModel>> GetAllBooksAsync()
        {
            return await this.dbContext.Books
                .Select(b => new AllBooksViewModel
                {
                    Id = b.Id.ToString(),
                    Title = b.Title,
                    YearPublished = b.YearPublished,
                    Publisher = b.Publisher,
                    AuthorName = $"{b.Author.FirstName} {b.Author.LastName}",
                    // TO DO
                    //Category = 
                }).ToListAsync();
        }

        public async Task<BookFormModel> GetNewCreateBookModelAsync()
        {
            var authors = await dbContext.Authors
               .Select(a => new AuthorSelectForBookFormModel
               {
                   Id = a.Id.ToString(),
                   Name = $"{a.FirstName} {a.LastName}",
                   Nationality = a.Nationality,
               }).ToListAsync();

            BookFormModel model = new BookFormModel
            {
                Authors = authors
            };

            return model;
        }

        public async Task AddBookAsync(BookFormModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            //Check if book exists in DB ???

            // Create a new Book object
            var book = new Book
            {
                Title = model.Title,
                ISBN = model.ISBN,
                YearPublished = model.YearPublished,
                Description = model.Description,
                Publisher = model.Publisher,
                CoverImagePathUrl = model.CoverImagePathUrl,
                AuthorId = Guid.Parse(model.AuthorId),
            };

            await dbContext.AddAsync(book);
            await dbContext.SaveChangesAsync();
        }
    }
}
    