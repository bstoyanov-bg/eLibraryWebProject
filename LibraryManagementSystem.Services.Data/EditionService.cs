using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Edition;


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
    }
}
