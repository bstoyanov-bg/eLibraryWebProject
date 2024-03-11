using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Author;
using LibraryManagementSystem.Web.ViewModels.Book;
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
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var author = await dbContext
                .Authors
                .FirstOrDefaultAsync(a => a.FirstName == model.FirstName &&
                                          a.LastName == model.LastName &&
                                          a.BirthDate == model.BirthDate &&
                                          a.Nationality == model.Nationality);

            if (author != null)
            {
                throw new InvalidOperationException("Author with the same name, birth date and nationality already exists.");
            }

            var newAuthor = new Author
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Biography = model.Biography,
                BirthDate = model.BirthDate,
                DeathDate = model.DeathDate,
                Nationality = model.Nationality,
            };

            await this.dbContext.AddAsync(newAuthor);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllAuthorsViewModel>> GetAllAuthorsAsync()
        {
            return await this.dbContext.Authors
                .AsNoTracking()
                .Where(a => a.IsDeleted == false)
                .Select(a => new AllAuthorsViewModel
                {
                    Id = a.Id.ToString(),
                    FirstName = a.FirstName,
                    LastName= a.LastName,
                    Nationality = a.Nationality,
                    BooksCount = a.Books
                                  .Where(b => b.IsDeleted == false)
                                  .Count(),
                })
                .OrderBy(a => a.FirstName)
                .ToArrayAsync();
        }

        public async Task<Author?> GetAuthorByIdAsync(string authorId)
        {
            return await this.dbContext
                .Authors
                .FirstOrDefaultAsync(a => a.Id.ToString() == authorId &&
                                          a.IsDeleted == false);
        }

        public async Task<AuthorFormModel?> GetAuthorForEditByIdAsync(string authorId)
        {
            var authorToEdit = await GetAuthorByIdAsync(authorId);

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
            var authorToEdit = await GetAuthorByIdAsync(authorId);

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
                .Where(a => a.IsDeleted == false)
                .Select(a => new AuthorSelectForBookFormModel
                {
                    Id = a.Id.ToString(),
                    Name = $"{a.FirstName} {a.LastName}",
                    Nationality = a.Nationality,
                })
                .ToListAsync();
        }

        public async Task<AuthorDetailsViewModel?> GetAuthorDetailsForUserAsync(string authorId)
        {
            var books = await this.dbContext.Books
                                            .Where(b => b.Author.Id.ToString() == authorId)
                                            .AsNoTracking()
                                            .Select(b => new BooksForAuthorDetailsViewModel 
                                            {
                                                 Title = b.Title,
                                                 ISBN = b.ISBN,
                                                 YearPublished = b.YearPublished,
                                                 Description = b.Description,
                                                 Publisher = b.Publisher,
                                                 CoverImagePathUrl = b.CoverImagePathUrl,
                                            }).ToListAsync();

            var author = await this.dbContext.Authors
                                             .Where(a => a.Id.ToString() == authorId &&
                                                         a.IsDeleted == false)
                                             .AsNoTracking()
                                             .Select(a => new AuthorDetailsViewModel 
                                             { 
                                                 Id = a.Id.ToString(),
                                                 FirstName = a.FirstName,
                                                 LastName = a.LastName,
                                                 Biography = a.Biography,
                                                 BirthDate = a.BirthDate,
                                                 DeathDate = a.DeathDate,
                                                 Nationality = a.Nationality,
                                                 Books = books
                                             }).FirstOrDefaultAsync();

            return author;
        }

        public async Task DeleteAuthorAsync(string authorId)
        {
            var authorToDelete = await dbContext
                .Authors
                .Where(a => a.IsDeleted == false)
                .FirstAsync(a => a.Id.ToString() == authorId);

            authorToDelete.IsDeleted = true;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
