using LibraryManagementSystem.Data;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Author;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Data
{
    public class AuthorService : IAuthorService
    {
        private readonly ELibraryDbContext dbContext;

        public AuthorService(ELibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAuthorAsync(AuthorFormModel model)
        {
            // Validate input
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            //Check if author exists in DB ???

            // Create a new Author object
            var author = new LibraryManagementSystem.Data.Models.Author
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Biography = model.Biography,
                BirthDate = model.BirthDate,
                DeathDate = model.DeathDate,
                Nationality = model.Nationality,
            };

            await this.dbContext.AddAsync(author);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllAuthorsViewModel>> GetAllAuthorsAsync()
        {
            return await this.dbContext.Authors
                .AsNoTracking()
                .Select(a => new AllAuthorsViewModel
                {
                    Id = a.Id.ToString(),
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Nationality = a.Nationality,
                    BooksCount = a.Books.Count,
                })
                .OrderBy(a => a.FirstName)
                .ToArrayAsync();
        }

        public async Task<AuthorFormModel?> GetAuthorForEditByIdAsync(string authorId)
        {
            var authorToEdit = await this.dbContext.Authors.FirstOrDefaultAsync(a => a.Id.ToString() == authorId);

            if (authorToEdit != null)
            {
                AuthorFormModel author = new AuthorFormModel()
                {
                    FirstName = authorToEdit.FirstName,
                    LastName = authorToEdit.LastName,
                    Biography = authorToEdit.Biography,
                    BirthDate = authorToEdit.BirthDate,
                    DeathDate = authorToEdit.DeathDate,
                    Nationality = authorToEdit.Nationality,
                };

                return author;
            }

            return null;
        }

        public async Task EditAuthorAsync(string authorId, AuthorFormModel model)
        {
            var authorToEdit = await this.dbContext.Authors.FirstOrDefaultAsync(a => a.Id.ToString() == authorId);

            if (authorToEdit != null)
            {
                authorToEdit.FirstName = model.FirstName;
                authorToEdit.LastName = model.LastName;
                authorToEdit.Biography = model.Biography;
                authorToEdit.BirthDate = model.BirthDate;
                authorToEdit.DeathDate = model.DeathDate;
                authorToEdit.Nationality = model.Nationality;
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuthorSelectForBookFormModel>> GetAllAuthorsForListAsync()
        {
            return await this.dbContext.Authors
                .AsNoTracking()
                .Select(a => new AuthorSelectForBookFormModel
                {
                    Id = a.Id.ToString(),
                    Name = $"{a.FirstName} {a.LastName}",
                    Nationality = a.Nationality,
                })
                .ToListAsync();
        }
    }
}
