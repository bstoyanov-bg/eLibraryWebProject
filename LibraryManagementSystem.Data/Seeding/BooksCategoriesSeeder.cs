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
                new BookCategory
                {
                    BookId = Guid.Parse("4C757EFD-2D1D-42A9-8460-88B9E1FFCC7D"),
                    CategoryId = 3,
                },
                new BookCategory
                {
                    BookId = Guid.Parse("8C6D196A-1E96-4DA0-9B65-91CD7736E13E"),
                    CategoryId = 3,
                },
                new BookCategory
                {
                    BookId = Guid.Parse("7DF9AE98-DBB6-4498-9ED5-3C6F19641CBF"),
                    CategoryId = 5,
                },
                new BookCategory
                {
                    BookId = Guid.Parse("DDDED6BD-AAB9-4503-B285-AA2DE7FF7BC3"),
                    CategoryId = 5,
                },
                new BookCategory
                {
                    BookId = Guid.Parse("42CBCCE4-349B-4D8C-A077-318A07BA74CC"),
                    CategoryId = 6,
                },
                new BookCategory
                {
                    BookId = Guid.Parse("D8EBD1EE-C555-418C-844C-28BE14D44314"),
                    CategoryId = 6,
                },
                new BookCategory
                {
                    BookId = Guid.Parse("6232A651-266B-45BC-B652-9696553914B7"),
                    CategoryId = 4,
                },
                new BookCategory
                {
                    BookId = Guid.Parse("A6D5D2D7-A6FB-46EF-AA1D-9502A0EF1C50"),
                    CategoryId = 4,
                },
            };

            await dbContext.BooksCategories.AddRangeAsync(booksCategories);
            await dbContext.SaveChangesAsync();
        }
    }
}
