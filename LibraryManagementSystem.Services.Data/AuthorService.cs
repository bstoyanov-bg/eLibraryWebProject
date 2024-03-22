using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Services.Data.Models.Author;
using LibraryManagementSystem.Web.ViewModels.Author;
using LibraryManagementSystem.Web.ViewModels.Author.Enums;
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

        // ready
        public async Task AddAuthorAsync(AuthorFormModel model)
        {
            var author = new Author
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Biography = model.Biography,
                BirthDate = model.BirthDate,
                DeathDate = model.DeathDate,
                Nationality = model.Nationality,
                ImagePathUrl = model.ImagePathUrl,
            };

            await this.dbContext.Authors.AddAsync(author);
            await this.dbContext.SaveChangesAsync();
        }

        // ready
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
                authorToEdit.ImagePathUrl = model.ImagePathUrl;
            }

            await this.dbContext.SaveChangesAsync();
        }

        // ready
        public async Task DeleteAuthorAsync(string authorId)
        {
            var authorToDelete = await GetAuthorByIdAsync(authorId);

            if (authorToDelete != null)
            {
                authorToDelete.IsDeleted = true;
            }

            await this.dbContext.SaveChangesAsync();
        }

        // ready
        public async Task<Author?> GetAuthorByIdAsync(string authorId)
        {
            return await this.dbContext
                .Authors
                .FirstOrDefaultAsync(a => a.IsDeleted == false &&
                                          a.Id.ToString() == authorId);
        }

        // ready
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
                    ImagePathUrl = authorToEdit.ImagePathUrl,
                };

                return author;
            }

            return null;
        }

        // ready
        public async Task<AuthorDetailsViewModel> GetAuthorDetailsForUserAsync(string authorId)
        {
            var books = await this.dbContext
                .Books
                .Where(b => b.Author.Id.ToString() == authorId &&
                            b.IsDeleted == false)
                .AsNoTracking()
                .Select(b => new BooksForAuthorDetailsViewModel
                {
                    Id = b.Id.ToString(),
                    Title = b.Title,
                    ISBN = b.ISBN,
                    YearPublished = b.YearPublished,
                    Description = b.Description,
                    Publisher = b.Publisher,
                    CoverImagePathUrl = b.CoverImagePathUrl,
                }).ToListAsync();

            var author = await this.dbContext
                .Authors
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
                    ImagePathUrl = a.ImagePathUrl,
                    Books = books
                }).FirstAsync();

            return author;
        }

        // ready
        public async Task<AllAuthorsFilteredAndPagedServiceModel> GetAllAuthorsFilteredAndPagedAsync(AllAuthorsQueryModel queryModel)
        {
            IQueryable<Author> authorsQuery = this.dbContext
                .Authors
                .Where(a => a.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";

                authorsQuery = authorsQuery
                    .Where(a => EF.Functions.Like(a.Nationality, wildCard) ||
                                EF.Functions.Like(a.FirstName + " " + a.LastName, wildCard));
            }

            authorsQuery = queryModel.AuthorSorting switch
            {
                AuthorSorting.Newest => authorsQuery
                    .OrderByDescending(a => a.CreatedOn),
                AuthorSorting.Oldest => authorsQuery
                    .OrderBy(a => a.CreatedOn),
                AuthorSorting.ByNameAscending => authorsQuery
                    .OrderBy(a => a.FirstName),
                AuthorSorting.ByNameDescending => authorsQuery
                    .OrderByDescending(a => a.FirstName),
                AuthorSorting.ByNationalityAscending => authorsQuery
                    .OrderBy(a => a.Nationality),
                AuthorSorting.ByNationalityDescending => authorsQuery
                    .OrderByDescending(a => a.Nationality),
                _ => authorsQuery
                    .OrderBy(a => a.FirstName),
            };

            IEnumerable<AllAuthorsViewModel> allAuthors = await authorsQuery
                .Skip((queryModel.CurrentPage - 1) * queryModel.AuthorsPerPage)
                .Take(queryModel.AuthorsPerPage)
                .Select(a => new AllAuthorsViewModel
                {
                    Id = a.Id.ToString(),
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Nationality = a.Nationality,
                    BooksCount = a.Books.Count(),
                    ImageURL = a.ImagePathUrl,
                }).ToListAsync();

            int totalAuthors = authorsQuery.Count();

            return new AllAuthorsFilteredAndPagedServiceModel()
            {
                TotalAuthorsCount = totalAuthors,
                Authors = allAuthors,
            };
        }

        // ready
        public async Task<bool> AuthorExistByNameAndNationalityAsync(string firstName, string lastName, string nationality)
        {
            return await this.dbContext
                .Authors
                .AsNoTracking()
                .Where(a => a.IsDeleted == false &&
                            a.FirstName == firstName &&
                            a.LastName == lastName &&
                            a.Nationality == nationality)
                .AnyAsync();
        }

        // ready
        public async Task<bool> AuthorExistByIdAsync(string authorId)
        {
            return await this.dbContext
                .Authors
                .AsNoTracking()
                .Where(a => a.IsDeleted == false &&
                            a.Id.ToString() == authorId)
                .AnyAsync();
        }

        // ready
        public async Task<IEnumerable<AuthorsSelectForBookFormModel>> GetAllAuthorsForListAsync()
        {
            return await this.dbContext
                .Authors
                .AsNoTracking()
                .Where(a => a.IsDeleted == false)
                .Select(a => new AuthorsSelectForBookFormModel
                {
                    Id = a.Id.ToString(),
                    Name = $"{a.FirstName} {a.LastName}",
                    Nationality = a.Nationality,
                })
                .ToListAsync();
        }
    }
}
