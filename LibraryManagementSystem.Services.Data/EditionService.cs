using LibraryManagementSystem.Data;
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
    }
}
