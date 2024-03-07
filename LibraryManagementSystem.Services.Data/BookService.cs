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
        private readonly ICategoryService categoryService;
        private readonly IAuthorService authorService;

        public BookService(ELibraryDbContext dbContext, ICategoryService categoryService, IAuthorService authorService)
        {
            this.dbContext = dbContext;
            this.categoryService = categoryService;
            this.authorService = authorService;
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
                .AsNoTracking()
                .Select(b => new AllBooksViewModel
                {
                    Id = b.Id.ToString(),
                    Title = b.Title,
                    YearPublished = b.YearPublished,
                    Publisher = b.Publisher,
                    AuthorName = $"{b.Author.FirstName} {b.Author.LastName}",
                    Category = b.BooksCategories
                                .Where(bc => bc.BookId == b.Id)
                                .Select(c => c.Category.Name)
                                .First(),
                }).ToListAsync();
        }

        public async Task<BookFormModel> GetNewCreateBookModelAsync()
        {
            var authors = await authorService.GetAllAuthorsForListAsync();

            var categories = await categoryService.GetAllCategoriesAsync();

            BookFormModel model = new BookFormModel
            {
                Authors = authors,
                Categories = categories,
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
            var book = new LibraryManagementSystem.Data.Models.Book
            {
                Title = model.Title,
                ISBN = model.ISBN,
                YearPublished = model.YearPublished,
                Description = model.Description,
                Publisher = model.Publisher,
                CoverImagePathUrl = model.CoverImagePathUrl,
                AuthorId = Guid.Parse(model.AuthorId),
            };

            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();

            var bookCategory = new BookCategory { BookId = book.Id, CategoryId = model.CategoryId };

            await dbContext.BooksCategories.AddAsync(bookCategory);
            await dbContext.SaveChangesAsync();
        }

        public async Task<BookFormModel?> GetBookForEditByIdAsync(string bookId)
        {
            var authors = await authorService.GetAllAuthorsForListAsync();

            var categories = await categoryService.GetAllCategoriesAsync();

            var bookToEdit = await dbContext.Books.FirstOrDefaultAsync(b => b.Id.ToString() == bookId);

            if (bookToEdit == null)
            {
                return new BookFormModel();
            }

            var categoryId = await categoryService.GetCategoryIdByBookIdAsync(bookId);

            BookFormModel book = new BookFormModel()
            {
                Title = bookToEdit.Title,
                ISBN = bookToEdit.ISBN,
                YearPublished = bookToEdit.YearPublished,
                Description = bookToEdit.Description,
                Publisher = bookToEdit.Publisher,
                CoverImagePathUrl = bookToEdit.CoverImagePathUrl,
                AuthorId = bookToEdit.AuthorId.ToString(),
                Authors = authors,
                CategoryId = categoryId,
                Categories = categories,
            };

            return book;
        }

        public async Task EditBookAsync(string bookId, BookFormModel model)
        {
            var bookToEdit = await dbContext.Books.FirstOrDefaultAsync(b => b.Id.ToString() == bookId);

            if (bookToEdit != null)
            {
                bookToEdit.Title = model.Title;
                bookToEdit.ISBN = model.ISBN;
                bookToEdit.YearPublished = model.YearPublished;
                bookToEdit.Description = model.Description;
                bookToEdit.Publisher = model.Publisher;
                bookToEdit.CoverImagePathUrl = model.CoverImagePathUrl;
                bookToEdit.AuthorId = Guid.Parse(model.AuthorId);
            }
            await dbContext.SaveChangesAsync();

            var existingBookCategory = await dbContext.BooksCategories.FirstOrDefaultAsync(bc => bc.BookId == bookToEdit.Id);

            if (existingBookCategory != null)
            {
                dbContext.BooksCategories.Remove(existingBookCategory);
                await dbContext.SaveChangesAsync();
            }

            var bookCategory = new BookCategory { BookId = bookToEdit.Id, CategoryId = model.CategoryId };

            await dbContext.BooksCategories.AddAsync(bookCategory);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookSelectForEditionFormModel>> GetAllBooksForListAsync()
        {
            return await this.dbContext.Books
                .AsNoTracking()
                .Select(b => new BookSelectForEditionFormModel
                {
                    Id = b.Id.ToString(),
                    Title = b.Title,
                    AuthorName = $"{b.Author.FirstName} {b.Author.LastName}",
                    YearPublished = b.YearPublished.ToString() ?? string.Empty,
                    Publisher = b.Publisher ?? string.Empty,
                })
                .ToListAsync();
        }
    }
}