using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Data.Seeding.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data.Seeding
{
    public class BooksCategoriesSeeder : ISeeder
    {
        public async  Task SeedAsync(ELibraryDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.BooksCategories.AnyAsync())
            {
                return;
            }

            IEnumerable<BookCategory> booksCategories = new HashSet<BookCategory>
            {
                new BookCategory
                {
                    BookId = Guid.Parse("F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6"),
                    CategoryId = 1,
                },
                new BookCategory
                {
                    BookId = Guid.Parse("5F14A26F-43EC-46C8-95E9-C1FE18FAC856"),
                    CategoryId = 1,
                },
                new BookCategory
                {
                    BookId = Guid.Parse("50E9B56F-9BC1-4356-AC0C-E3D5945778BA"),
                    CategoryId = 2,
                },
                new BookCategory
                {
                    BookId = Guid.Parse("3B7822CB-17D3-47FE-8C2B-2CA761F376F1"),
                    CategoryId = 2,
                },
            };

            await dbContext.BooksCategories.AddRangeAsync(booksCategories);
            await dbContext.SaveChangesAsync();
        }
    }
}
