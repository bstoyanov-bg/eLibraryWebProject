﻿using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Book;
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

        public async Task<IEnumerable<AllBooksViewModel>> GetAllBooksAsync()
        {
            return await this.dbContext
                .Books
                .Where(b => b.IsDeleted == false)
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
                    EditionsCount = b.Editions
                                     .Where(e => e.IsDeleted == false)
                                     .Count(),
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
                .FirstOrDefaultAsync(b => b.Id == Guid.Parse(bookId));
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
            foreach (var edition in bookToDelete.Editions)
            {
                await editionService.DeleteBookEditionAsync(edition.Id.ToString());
            }
            //foreach (var edition in bookToDelete.Editions)
            //{
            //    await this.editionService.DeleteEditionAsync(edition.Id.ToString());
            //}

            bookToDelete.IsDeleted = true;

            await this.dbContext.SaveChangesAsync();
        }
    }
}