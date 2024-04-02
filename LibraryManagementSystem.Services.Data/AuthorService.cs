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

        public async Task DeleteAuthorAsync(string authorId)
        {
            Author? authorToDelete = await GetAuthorByIdAsync(authorId);

            if (authorToDelete != null)
            {
                authorToDelete.IsDeleted = true;
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<Author> EditAuthorAsync(string authorId, AuthorFormModel model)
        {
            Author? authorToEdit = await GetAuthorByIdAsync(authorId);

            if (authorToEdit != null)
            {
                authorToEdit.FirstName = model.FirstName;
                authorToEdit.LastName = model.LastName;
                authorToEdit.Biography = model.Biography;
                authorToEdit.BirthDate = model.BirthDate;
                authorToEdit.DeathDate = model.DeathDate;
                authorToEdit.Nationality = model.Nationality;
                //authorToEdit.ImageFilePath = model.ImageFilePath;
            }

            await this.dbContext.SaveChangesAsync();

            return authorToEdit!;
        }

        public async Task<Author> AddAuthorAsync(AuthorFormModel model)
        {
            Author author = new Author
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Biography = model.Biography,
                BirthDate = model.BirthDate,
                DeathDate = model.DeathDate,
                Nationality = model.Nationality,
                ImageFilePath = model.ImageFilePath,
            };

            await this.dbContext.Authors.AddAsync(author);
            await this.dbContext.SaveChangesAsync();

            return author;
        }

        public async Task<Author?> GetAuthorByIdAsync(string authorId)
        {
            return await this.dbContext
                .Authors
                .FirstOrDefaultAsync(a => a.IsDeleted == false &&
                                          a.Id.ToString() == authorId);
        }

        public async Task<AuthorFormModel?> GetAuthorForEditByIdAsync(string authorId)
        {
            Author? authorToEdit = await this.GetAuthorByIdAsync(authorId);

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
                    ImageFilePath = authorToEdit.ImageFilePath,
                };

                return author;
            }

            return null;
        }

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
                    ImageFilePath = b.ImageFilePath,
                }).ToListAsync();

            AuthorDetailsViewModel author = await this.dbContext
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
                    ImageFilePath = a.ImageFilePath,
                    Books = books
                }).FirstAsync();

            return author;
        }

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
                    ImageFilePath = a.ImageFilePath,
                }).ToListAsync();

            int totalAuthors = authorsQuery.Count();

            return new AllAuthorsFilteredAndPagedServiceModel()
            {
                TotalAuthorsCount = totalAuthors,
                Authors = allAuthors,
            };
        }

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

        public async Task<bool> AuthorExistByIdAsync(string authorId)
        {
            return await this.dbContext
                .Authors
                .AsNoTracking()
                .Where(a => a.IsDeleted == false &&
                            a.Id.ToString() == authorId)
                .AnyAsync();
        }

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
