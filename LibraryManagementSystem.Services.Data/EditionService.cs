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
            var edition = new Edition
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

            var editionToEdit = await dbContext.Editions.FirstOrDefaultAsync(e => e.Id.ToString() == editionId);

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

        public async Task EditBookAsync(string editionId, EditionFormModel model)
        {
            var editionToEdit = await dbContext.Editions.FirstOrDefaultAsync(e => e.Id.ToString() == editionId);

            if (editionToEdit != null)
            {
                editionToEdit.Version = model.Version;
                editionToEdit.Publisher = model.Publisher;
                editionToEdit.EditionYear = model.EditionYear;
                editionToEdit.BookId = Guid.Parse(model.BookId);
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
