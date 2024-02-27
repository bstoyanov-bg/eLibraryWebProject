using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
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
            var author = new Author
            {
                FirstName = model.FirstName, 
                LastName = model.LastName,
                Biography = model.Biography,
                BirthDate = model.BirthDate,
                DeathDate = model.DeathDate,
                Nationality = model.Nationality,
            };

            await dbContext.AddAsync(author);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllAuthorsViewModel>> GetAllAuthorsAsync()
        {
            return await this.dbContext.Authors
                .Select(a => new AllAuthorsViewModel
                {
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Nationality = a.Nationality,
                    BooksCount = a.Books.Count,
                })
                .ToListAsync();
        }
    }
}
