using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Edition;
using Microsoft.EntityFrameworkCore;
using static LibraryManagementSystem.Common.DataModelsValidationConstants;


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
                Books = books,
            };

            return model;
        }

        public async Task AddEditionAsync(EditionFormModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Create a new Edition object
            var edition = new LibraryManagementSystem.Data.Models.Edition
            {
                Version = model.Version,
                Publisher = model.Publisher,
                EditionYear = model.EditionYear,
                BookId = Guid.Parse(model.BookId),
            };

            await dbContext.Editions.AddAsync(edition);
            await dbContext.SaveChangesAsync();
        }

        public async Task<EditionFormModel?> GetEditionForEditByIdAsync(string editionId)
        {
            var books = await bookService.GetAllBooksForListAsync();

            var editionToEdit = await dbContext
                .Editions
                .FirstOrDefaultAsync(e => e.Id.ToString() == editionId &&
                                          e.IsDeleted == false);

            if (editionToEdit == null)
            {
                return new EditionFormModel();
            }

            EditionFormModel edition = new EditionFormModel()
            {
                Version = editionToEdit.Version,
                Publisher = editionToEdit.Publisher,
                EditionYear = editionToEdit.EditionYear,
                BookId = editionToEdit.BookId.ToString(),
                Books = books,
            };

            return edition;
        }

        public async Task EditBookEditionAsync(string editionId, EditionFormModel model)
        {
            var editionToEdit = await dbContext
                .Editions
                .FirstOrDefaultAsync(e => e.Id.ToString() == editionId &&
                                          e.IsDeleted == false);

            if (editionToEdit != null)
            {
                editionToEdit.Version = model.Version;
                editionToEdit.Publisher = model.Publisher;
                editionToEdit.EditionYear = model.EditionYear;
                editionToEdit.BookId = Guid.Parse(model.BookId);
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteEditionAsync(string editionId)
        {
            var editionToDelete = await dbContext
                .Editions
                .Where(e => e.IsDeleted == false)
                .FirstAsync(e => e.Id.ToString() == editionId);

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
    }
}
