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

        public async Task AddEditionAsync(EditionFormModel model)
        {
            Book? book = await this.bookService.GetBookByIdAsync(model.BookId);

            Edition edition = new Edition
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

        public async Task EditBookEditionAsync(string editionId, EditionFormModel model)
        {
            Edition? editionToEdit = await this.GetEditionByIdAsync(editionId);

            if (editionToEdit != null)
            {
                editionToEdit.Version = model.Version;
                editionToEdit.Publisher = model.Publisher;
                editionToEdit.EditionYear = model.EditionYear;
                editionToEdit.BookId = Guid.Parse(model.BookId);
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteEditionAsync(string editionId)
        {
            Edition? editionToDelete = await this.GetEditionByIdAsync(editionId);

            if (editionToDelete != null)
            {
                editionToDelete.IsDeleted = true;
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<Edition?> GetEditionByIdAsync(string editionId)
        {
            return await this.dbContext
                .Editions
                .Where(e => e.IsDeleted == false)
                .FirstOrDefaultAsync(e => e.Id.ToString() == editionId);
        }

        public async Task<EditionFormModel> GetCreateNewEditionModelAsync()
        {
            var books = await this.bookService.GetAllBooksForListAsync();

            EditionFormModel model = new EditionFormModel
            {
                BooksDropDown = books,
            };

            return model;
        }

        public async Task<EditionFormModel?> GetEditionForEditByIdAsync(string editionId)
        {
            var books = await this.bookService.GetAllBooksForListAsync();

            Edition? editionToEdit = await this.GetEditionByIdAsync(editionId);

            if (editionToEdit != null)
            {
                EditionFormModel edition = new EditionFormModel()
                {
                    Id = editionToEdit.Id.ToString(),
                    Version = editionToEdit.Version,
                    Publisher = editionToEdit.Publisher,
                    EditionYear = editionToEdit.EditionYear,
                    BookId = editionToEdit.BookId.ToString(),
                    FilePath = editionToEdit.FilePath,
                    BooksDropDown = books,
                };

                return edition;
            }

            return null;
        }

        public async Task<bool> EditionExistByIdAsync(string editionId)
        {
            return await this.dbContext
                .Editions
                .AsNoTracking()
                .Where(e => e.IsDeleted == false &&
                            e.Id.ToString() == editionId)
                .AnyAsync();
        }

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

        public async Task<bool> DoesEditionHaveUploadedFileAsync(string editionId)
        {
            return await this.dbContext
                .Editions
                .AsNoTracking()
                .Where(e => e.Id.ToString() == editionId &&
                            e.FilePath != null)
                .AnyAsync();
        }

        public async Task<string> GetBookIdByEditionIdAsync(string editionId)
        {
           string? bookId = await this.dbContext
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

        public async Task<int> GetCountOfActiveEditionsAsync()
        {
            return await this.dbContext
                .Editions
                .AsNoTracking()
                .Where(u => u.IsDeleted == false)
                .CountAsync();
        }

        public async Task<int> GetCountOfDeletedEditionsAsync()
        {
            return await this.dbContext
                .Editions
                .AsNoTracking()
                .Where(u => u.IsDeleted == true)
                .CountAsync();
        }

        public async Task<IEnumerable<Edition>> GetAllBookEditionsByBookIdAsync(string bookId)
        {
            return await this.dbContext
                .Editions
                .Where(e => e.IsDeleted == false &&
                            e.BookId.ToString() == bookId)
                .ToListAsync();
        }
    }
}
