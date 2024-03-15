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
        private readonly Lazy<IEditionService> editionServiceLazy;
        //private readonly IEditionService editionService;

        public BookService(ELibraryDbContext dbContext, ICategoryService categoryService, IAuthorService authorService, Lazy<IEditionService> editionServiceLazy/*, IEditionService editionService*/)
        {
            this.dbContext = dbContext;
            this.categoryService = categoryService;
            this.authorService = authorService;
            this.editionServiceLazy = editionServiceLazy;
            //this.editionService = editionService;
        }

        public async Task<IEnumerable<IndexViewModel>> LastTenBooksAsync()
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
                    ImageUrl = b.CoverImagePathUrl ?? string.Empty,
                })
                .ToListAsync();

            return lastNineAddedBooks;
        }


        // NOT USED ANYMORE
        //public async Task<IEnumerable<AllBooksViewModel>> GetAllBooksAsync()
        //{
        //    return await this.dbContext
        //        .Books
        //        .Where(b => b.IsDeleted == false)
        //        .AsNoTracking()
        //        .Select(b => new AllBooksViewModel
        //        {
        //            Id = b.Id.ToString(),
        //            Title = b.Title,
        //            YearPublished = b.YearPublished,
        //            Publisher = b.Publisher,
        //            AuthorName = $"{b.Author.FirstName} {b.Author.LastName}",
        //            Category = b.BooksCategories
        //                        .Where(bc => bc.BookId == b.Id)
        //                        .Select(c => c.Category.Name)
        //                        .First(),
        //            EditionsCount = b.Editions
        //                             .Where(e => e.IsDeleted == false)
        //                             .Count(),
        //        }).ToListAsync();
        //}

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

            var book = await dbContext
                .Books
                .Where(b => b.IsDeleted == false)
                .FirstOrDefaultAsync(a => a.Title == model.Title &&
                                          a.ISBN == model.ISBN);

            if (book != null)
            {
                throw new InvalidOperationException("Book with the same title and ISBN already exists.");
            }

            var newBook = new Book
            {
                Title = model.Title,
                ISBN = model.ISBN,
                YearPublished = model.YearPublished,
                Description = model.Description,
                Publisher = model.Publisher,
                CoverImagePathUrl = model.CoverImagePathUrl,
                AuthorId = Guid.Parse(model.AuthorId),
            };

            await dbContext.Books.AddAsync(newBook);
            await dbContext.SaveChangesAsync();

            var bookCategory = new BookCategory { BookId = newBook.Id, CategoryId = model.CategoryId };

            await dbContext.BooksCategories.AddAsync(bookCategory);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Book?> GetBookByIdAsync(string bookId)
        {
            return await this.dbContext
                .Books
                .Where(b => b.IsDeleted == false)
                .FirstOrDefaultAsync(b => b.Id.ToString() == bookId);
        }

        public async Task<BookFormModel?> GetBookForEditByIdAsync(string bookId)
        {
            var authors = await authorService.GetAllAuthorsForListAsync();

            var categories = await categoryService.GetAllCategoriesAsync();

            var bookToEdit = await GetBookByIdAsync(bookId);

            var categoryId = this.categoryService.GetCategoryIdByBookIdAsync(bookId);

            if (bookToEdit == null) 
            { 
                return null;
            }

            BookFormModel book = new BookFormModel()
            {
                Id = bookToEdit.Id.ToString(),
                Title = bookToEdit.Title,
                ISBN = bookToEdit.ISBN,
                YearPublished = bookToEdit.YearPublished,
                Description = bookToEdit.Description,
                Publisher = bookToEdit.Publisher,
                CoverImagePathUrl = bookToEdit.CoverImagePathUrl,
                AuthorId = bookToEdit.AuthorId.ToString(),
                Authors = authors,
                CategoryId = await categoryId,
                Categories = categories,
                FilePath = bookToEdit.FilePath,
            };

            return book;
        }

        public async Task EditBookAsync(string bookId, BookFormModel model)
        {
            var bookToEdit = await GetBookByIdAsync(bookId);

            if (bookToEdit != null)
            {
                bookToEdit.Title = model.Title;
                bookToEdit.ISBN = model.ISBN;
                bookToEdit.YearPublished = model.YearPublished;
                bookToEdit.Description = model.Description;
                bookToEdit.Publisher = model.Publisher;
                bookToEdit.CoverImagePathUrl = model.CoverImagePathUrl;
                bookToEdit.AuthorId = Guid.Parse(model.AuthorId);
                bookToEdit.FilePath = model.FilePath;
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

        public async Task<BookDetailsViewModel> GetBookDetailsForUserAsync(string bookId)
        {
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

            var categoryName = await this.dbContext
                .Categories
                .Where(c => c.BooksCategories.Any(bc => bc.BookId == Guid.Parse(bookId)))
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            var book = await this.dbContext
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
                    CoverImagePathUrl = b.CoverImagePathUrl,
                    AuthorName = $"{b.Author.FirstName} {b.Author.LastName}",
                    CategoryName = categoryName ?? string.Empty,
                    Editions = editions,
                }).FirstOrDefaultAsync();

            return book;
        }

        public async Task DeleteBookAsync(string bookId)
        {
            var bookToDelete = await GetBookByIdAsync(bookId);

            IEditionService editionService = editionServiceLazy.Value;

            var bookEditionsForDelete = await editionService.GetAllBookEditionsForBookbyBookId(bookId);

            foreach (var edition in bookEditionsForDelete)
            {
                await editionService.DeleteBookEditionAsync(edition.Id.ToString());
            }

            bookToDelete.IsDeleted = true;

            await this.dbContext.SaveChangesAsync();
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
                    ImageURL = b.CoverImagePathUrl ?? string.Empty,
                    EditionsCount = b.Editions.Where(e => e.IsDeleted == false).Count(),
                }).ToListAsync();

            int totalBooks = booksQuery.Count();

            return new AllBooksFilteredAndPagedServiceModel()
            {
                TotalBooksCount = totalBooks,
                Books = allBooks,
            };
        }

        // NOT NEEDED
        //public async Task AddFileToBookAsync(string bookId, BookFormModel model)
        //{
        //    var bookToAddFile = await GetBookByIdAsync(bookId);

        //    if (bookToAddFile != null)
        //    {
        //        bookToAddFile.Id = Guid.Parse(model.Id!);
        //        bookToAddFile.FilePath = model.FilePath;
        //    }

        //    await dbContext.SaveChangesAsync();
        //}
    }
}