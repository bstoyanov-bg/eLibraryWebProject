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

        public async Task<EditionFormModel> GetNewCreateEditionModelAsync()
        {
            var books = await bookService.GetAllBooksForListAsync();

            EditionFormModel model = new EditionFormModel
            {
                BooksDropDown = books,
            };

            return model;
        }

        public async Task AddEditionAsync(EditionFormModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var book = await bookService.GetBookByIdAsync(model.BookId);

            if (book == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var edition = new Edition
            {
                Version = model.Version,
                Publisher = model.Publisher,
                EditionYear = model.EditionYear,
                BookId = book.Id,
            };

            // Does not work ??? WHY
            book.Editions.Add(edition);

            await dbContext.Editions.AddAsync(edition);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Edition?> GetEditionByIdAsync(string editionId)
        {
            return await dbContext
                .Editions
                .Where(e => e.IsDeleted == false)
                .FirstOrDefaultAsync(e => e.Id.ToString() == editionId);
        }

        public async Task<EditionFormModel?> GetBookEditionForEditByIdAsync(string editionId)
        {
            var books = await bookService.GetAllBooksForListAsync();

            var editionToEdit = await GetEditionByIdAsync(editionId);

            if (editionToEdit == null)
            {
                return null;
            }

            EditionFormModel edition = new EditionFormModel()
            {
                Version = editionToEdit.Version,
                Publisher = editionToEdit.Publisher,
                EditionYear = editionToEdit.EditionYear,
                BookId = editionToEdit.BookId.ToString(),
                BooksDropDown = books,
            };

            return edition;
        }

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

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteBookEditionAsync(string editionId)
        {
            var editionToDelete = await GetEditionByIdAsync(editionId);

            if (editionToDelete == null)
            {
                throw new ArgumentNullException(nameof(editionToDelete));
            }

            editionToDelete.IsDeleted = true;

            await this.dbContext.SaveChangesAsync();
        }

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

        public async Task<IEnumerable<Edition>> GetAllBookEditionsForBookbyBookId(string bookId)
        {
            return await this.dbContext
                .Editions
                .Where(e => e.BookId.ToString() == bookId)
                .ToListAsync();
        }
    }
}
