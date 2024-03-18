using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Edition;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Data
{
    public class EditionService : IEditionService
    {
        private readonly ELibraryDbContext dbContext;
        private readonly IBookService bookService;

        public EditionService(ELibraryDbContext dbContext, IBookService bookService)
        {
            this.dbContext = dbContext;
            this.bookService = bookService;
        }

        // ready
        public async Task AddEditionAsync(EditionFormModel model)
        {
            var book = await bookService.GetBookByIdAsync(model.BookId);

            var edition = new Edition
            {
                Version = model.Version,
                Publisher = model.Publisher,
                EditionYear = model.EditionYear,
                BookId = Guid.Parse(model.BookId),
            };

            // IT DOES NOT SAVE ???
            book!.Editions.Add(edition);

            await this.dbContext.Editions.AddAsync(edition);
            await this.dbContext.SaveChangesAsync();
        }

        // ready
        public async Task EditBookEditionAsync(string editionId, EditionFormModel model)
        {
            var editionToEdit = await GetEditionByIdAsync(editionId);

            if (editionToEdit != null)
            {
                editionToEdit.Version = model.Version;
                editionToEdit.Publisher = model.Publisher;
                editionToEdit.EditionYear = model.EditionYear;
                editionToEdit.BookId = Guid.Parse(model.BookId);
            }

            await this.dbContext.SaveChangesAsync();
        }

        // ready
        public async Task DeleteEditionAsync(string editionId)
        {
            var editionToDelete = await GetEditionByIdAsync(editionId);

            if (editionToDelete != null)
            {
                editionToDelete.IsDeleted = true;
            }

            await this.dbContext.SaveChangesAsync();
        }

        // ready
        public async Task<Edition?> GetEditionByIdAsync(string editionId)
        {
            return await dbContext
                .Editions
                .Where(e => e.IsDeleted == false)
                .FirstOrDefaultAsync(e => e.Id.ToString() == editionId);
        }

        // ready
        public async Task<EditionFormModel> GetCreateNewEditionModelAsync()
        {
            var books = await bookService.GetAllBooksForListAsync();

            EditionFormModel model = new EditionFormModel
            {
                BooksDropDown = books,
            };

            return model;
        }

        // ready
        public async Task<EditionFormModel?> GetEditionForEditByIdAsync(string editionId)
        {
            var books = await bookService.GetAllBooksForListAsync();

            var editionToEdit = await GetEditionByIdAsync(editionId);

            if (editionToEdit != null)
            {
                EditionFormModel edition = new EditionFormModel()
                {
                    Id = editionToEdit.Id.ToString(),
                    Version = editionToEdit.Version,
                    Publisher = editionToEdit.Publisher,
                    EditionYear = editionToEdit.EditionYear,
                    BookId = editionToEdit.BookId.ToString(),
                    BooksDropDown = books,
                };

                return edition;
            }

            return null;
        }

        // ready
        public async Task<bool> EditionExistByIdAsync(string editionId)
        {
            return await this.dbContext
                .Editions
                .AsNoTracking()
                .Where(e => e.IsDeleted == false &&
                            e.Id.ToString() == editionId)
                .AnyAsync();
        }

        // ready
        public async Task<bool> EditionExistByVersionPublisherAndBookIdAsync(string version, string publisher, string bookId)
        {
            return await this.dbContext
                .Editions
                .AsNoTracking()
                .Where(e => e.IsDeleted == false &&
                            e.Version == version &&
                            e.Publisher == publisher &&
                            e.BookId == Guid.Parse(bookId))
                .AnyAsync();
        }

        // ready
        public async Task<string> GetBookIdByEditionIdAsync(string editionId)
        {
           var bookId = await this.dbContext
                                  .Editions
                                  .AsNoTracking()
                                  .Where(e => e.Id.ToString() == editionId)
                                  .Select(e => e.BookId.ToString())
                                  .FirstOrDefaultAsync();

            if (bookId == null)
            {
                return string.Empty;
            }

            return bookId;
        }

        // ready
        public async Task<IEnumerable<Edition>> GetAllBookEditionsForBookByBookId(string bookId)
        {
            return await this.dbContext
                .Editions
                .Where(e => e.IsDeleted == false &&
                            e.BookId.ToString() == bookId)
                .ToListAsync();
        }
    }
}
