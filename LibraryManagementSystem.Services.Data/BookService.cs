using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Services.Data.Models.Book;
using LibraryManagementSystem.Web.ViewModels.Book;
using LibraryManagementSystem.Web.ViewModels.Book.Enums;
using LibraryManagementSystem.Web.ViewModels.Edition;
using LibraryManagementSystem.Web.ViewModels.Home;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Data
{
    public class BookService : IBookService
    {
        private readonly ELibraryDbContext dbContext;
        private readonly ICategoryService categoryService;
        private readonly IAuthorService authorService;
        private readonly Lazy<IRatingService> ratingServiceLazy;
        private readonly Lazy<IEditionService> editionServiceLazy;

        public BookService(ELibraryDbContext dbContext, 
            ICategoryService categoryService, 
            IAuthorService authorService,
            Lazy<IRatingService> ratingServiceLazy,
            Lazy<IEditionService> editionServiceLazy)
        {
            this.dbContext = dbContext;
            this.categoryService = categoryService;
            this.authorService = authorService;
            this.editionServiceLazy = editionServiceLazy;
            this.ratingServiceLazy = ratingServiceLazy;
        }

        public async Task EditBookAsync(string bookId, BookFormModel model)
        {
            Book? bookToEdit = await GetBookByIdAsync(bookId);

            if (bookToEdit != null)
            {
                bookToEdit.Title = model.Title;
                bookToEdit.ISBN = model.ISBN;
                bookToEdit.YearPublished = model.YearPublished;
                bookToEdit.Description = model.Description;
                bookToEdit.Publisher = model.Publisher;
                bookToEdit.ImageFilePath = model.ImageFilePath;
                bookToEdit.AuthorId = Guid.Parse(model.AuthorId);
                bookToEdit.FilePath = model.FilePath;
            }

            var currentBookCategory = await this.dbContext
                .BooksCategories
                .FirstOrDefaultAsync(bc => bc.BookId.ToString() == bookId);

            if (currentBookCategory != null)
            {
                this.dbContext.BooksCategories.Remove(currentBookCategory);
                await this.dbContext.SaveChangesAsync();
            }

            var newBookCategory = new BookCategory { BookId = bookToEdit!.Id, CategoryId = model.CategoryId };

            await this.dbContext.BooksCategories.AddAsync(newBookCategory);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(string bookId)
        {
            Book? bookToDelete = await GetBookByIdAsync(bookId);

            IEditionService editionService = this.editionServiceLazy.Value;

            var bookEditionsForDelete = await editionService.GetAllBookEditionsByBookIdAsync(bookId);

            if (bookToDelete != null)
            {
                foreach (var edition in bookEditionsForDelete)
                {
                    await editionService.DeleteEditionAsync(edition.Id.ToString());
                }

                bookToDelete.IsDeleted = true;
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<Book> AddBookAsync(BookFormModel model)
        {
            Book book = new Book
            {
                Title = model.Title,
                ISBN = model.ISBN,
                YearPublished = model.YearPublished,
                Description = model.Description,
                Publisher = model.Publisher,
                ImageFilePath = model.ImageFilePath,
                AuthorId = Guid.Parse(model.AuthorId),
            };

            await this.dbContext.Books.AddAsync(book);
            await this.dbContext.SaveChangesAsync();

            BookCategory bookCategory = new BookCategory { BookId = book.Id, CategoryId = model.CategoryId };

            await this.dbContext.BooksCategories.AddAsync(bookCategory);
            await this.dbContext.SaveChangesAsync();

            return book;
        }

        public async Task<Book?> GetBookByIdAsync(string bookId)
        {
            return await this.dbContext
                .Books
                .Where(b => b.IsDeleted == false)
                .FirstOrDefaultAsync(b => b.Id.ToString() == bookId);
        }

        public async Task<BookFormModel> GetCreateNewBookModelAsync()
        {
            var authors = await this.authorService.GetAllAuthorsForListAsync();

            var categories = await this.categoryService.GetAllCategoriesAsync();

            BookFormModel model = new BookFormModel
            {
                Authors = authors,
                Categories = categories,
            };

            return model;
        }

        public async Task<BookFormModel?> GetBookForEditByIdAsync(string bookId)
        {
            var authors = await this.authorService.GetAllAuthorsForListAsync();

            var categories = await this.categoryService.GetAllCategoriesAsync();

            Book? bookToEdit = await this.GetBookByIdAsync(bookId);

            int categoryId = await this.categoryService.GetCategoryIdByBookIdAsync(bookId);

            if (bookToEdit != null)
            {
                BookFormModel book = new BookFormModel()
                {
                    Id = bookToEdit.Id.ToString(),
                    Title = bookToEdit.Title,
                    ISBN = bookToEdit.ISBN,
                    YearPublished = bookToEdit.YearPublished,
                    Description = bookToEdit.Description,
                    Publisher = bookToEdit.Publisher,
                    ImageFilePath = bookToEdit.ImageFilePath,
                    AuthorId = bookToEdit.AuthorId.ToString(),
                    Authors = authors,
                    CategoryId = categoryId,
                    Categories = categories,
                    FilePath = bookToEdit.FilePath,
                };

                return book;
            }

            return null;
        }

        public async Task<BookDetailsViewModel> GetBookDetailsForUserAsync(string bookId)
        {
            // Refactor
            var editions = await this.dbContext
                .Editions
                .Where(e => e.BookId.ToString() == bookId &&
                            e.IsDeleted == false)
                .Select(e => new EditionsForBookDetailsViewModel
                {
                    Id = e.Id.ToString(),
                    Version = e.Version,
                    EditionYear = e.EditionYear,
                    Publisher = e.Publisher,
                }).ToListAsync();

            // Refactor
            string categoryName = await this.dbContext
                .Categories
                .Where(c => c.BooksCategories.Any(bc => bc.BookId == Guid.Parse(bookId)))
                .Select(c => c.Name)
                .FirstAsync();
    
            IRatingService ratingService = this.ratingServiceLazy.Value;

            decimal? bookRating = await ratingService.GetAverageRatingForBookAsync(bookId);

            BookDetailsViewModel book = await this.dbContext
                .Books
                .Where(b => b.Id.ToString() == bookId &&
                            b.IsDeleted == false)
                .Select(b => new BookDetailsViewModel
                {
                    Id = b.Id.ToString(),
                    ISBN = b.ISBN,
                    Title = b.Title,
                    YearPublished = b.YearPublished,
                    Description = b.Description,
                    Publisher = b.Publisher,
                    ImageFilePath = b.ImageFilePath,
                    AuthorName = $"{b.Author.FirstName} {b.Author.LastName}",
                    AuthorId = b.AuthorId.ToString(),
                    CategoryName = categoryName,
                    Editions = editions,
                    Rating = (decimal)bookRating,
                }).FirstAsync();

            return book;
        }

        public async Task<AllBooksFilteredAndPagedServiceModel> GetAllBooksFilteredAndPagedAsync(AllBooksQueryModel queryModel)
        {
            IQueryable<Book> booksQuery = this.dbContext
                .Books
                .Where(b => b.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryModel.Category))
            {
                booksQuery = booksQuery
                    .Where(b => b.BooksCategories
                    .Any(bc => bc.Category.Name == queryModel.Category));
            }

            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";

                booksQuery = booksQuery
                    .Where(b => EF.Functions.Like(b.Title, wildCard) ||
                                EF.Functions.Like(b.Author.FirstName + " " + b.Author.LastName, wildCard));
            }

            booksQuery = queryModel.BookSorting switch
            {
                BookSorting.Newest => booksQuery
                    .OrderByDescending(b => b.CreatedOn),
                BookSorting.Oldest => booksQuery
                    .OrderBy(b => b.CreatedOn),
                BookSorting.ByYearPublishedAscending => booksQuery
                    .OrderByDescending(b => b.YearPublished),
                BookSorting.ByYearPublishedDescending => booksQuery
                    .OrderBy(b => b.YearPublished),
                BookSorting.ByTitleAscending => booksQuery
                    .OrderBy(b => b.Title),
                BookSorting.ByTitleDescending => booksQuery
                    .OrderByDescending(b => b.Title),
                _ => booksQuery
                    .OrderBy(b => b.Title),
            };

            IEnumerable<AllBooksViewModel> allBooks = await booksQuery
                .Skip((queryModel.CurrentPage - 1) * queryModel.BooksPerPage)
                .Take(queryModel.BooksPerPage)
                .Select(b => new AllBooksViewModel
                {
                    Id = b.Id.ToString(),
                    Title = b.Title,
                    YearPublished = b.YearPublished,
                    Publisher = b.Publisher,
                    AuthorName = $"{b.Author.FirstName} {b.Author.LastName}",
                    Category = b.BooksCategories.Select(bc => bc.Category.Name).First(),
                    ImageFilePath = b.ImageFilePath ?? string.Empty,
                    EditionsCount = b.Editions.Where(e => e.IsDeleted == false).Count(),
                }).ToListAsync();

            int totalBooks = booksQuery.Count();

            return new AllBooksFilteredAndPagedServiceModel()
            {
                TotalBooksCount = totalBooks,
                Books = allBooks,
            };
        }

        public async Task<bool> BookExistByIdAsync(string bookId)
        {
            return await this.dbContext
                .Books
                .Where(b => b.IsDeleted == false &&
                            b.Id.ToString() == bookId)
                .AnyAsync();
        }

        public async Task<bool> BookExistByTitleAndAuthorIdAsync(string title, string authorId)
        {
            return await this.dbContext
                .Books
                .AsNoTracking()
                .Where(b => b.IsDeleted == false &&
                            b.Title == title &&
                            b.AuthorId == Guid.Parse(authorId))
                .AnyAsync();
        }

        public async Task<bool> HasUserRatedBookAsync(string userId, string bookId)
        {
            return await this.dbContext
                .Ratings
                .AnyAsync(r => r.UserId.ToString() == userId &&
                               r.BookId.ToString() == bookId);
        }

        public async Task<IEnumerable<IndexViewModel>> LastNineBooksAsync()
        {
            IEnumerable<IndexViewModel> lastNineAddedBooks = await dbContext
                .Books
                .AsNoTracking()
                .Where(b => b.IsDeleted == false)
                .OrderByDescending(h => h.CreatedOn)
                .Take(9)
                .Select(b => new IndexViewModel()
                {
                    Id = b.Id.ToString(),
                    Title = b.Title,
                    Description = b.Description,
                    Author = $"{b.Author.FirstName} {b.Author.LastName}",
                    ImageFilePath = b.ImageFilePath ?? string.Empty,
                })
                .ToListAsync();

            return lastNineAddedBooks;
        }

        public async Task<IEnumerable<BookSelectForEditionFormModel>> GetAllBooksForListAsync()
        {
            return await this.dbContext
                .Books
                .Where(b => b.IsDeleted == false)
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
