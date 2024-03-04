using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Data.Seeding.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data.Seeding
{
    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ELibraryDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Categories.AnyAsync())
            {
                return;
            }

            IEnumerable<Category> catgeories = new HashSet<Category>
                {
                    new Category
                    {
                        Id = 1,
                        Name = "Academic book",
                    },
                    new Category
                    {
                        Id = 2,
                        Name = "Adventure stories",
                    },
                    new Category
                    {
                        Id = 3,
                        Name = "Classics",
                    },
                    new Category
                    {
                        Id = 4,
                        Name = "Mystery",
                    },
                    new Category
                    {
                        Id = 5,
                        Name = "Roman",
                    },
                    new Category
                    {
                        Id = 6,
                        Name = "Science fiction",
                    }
                };

            await dbContext.Categories.AddRangeAsync(catgeories);
            await dbContext.SaveChangesAsync();
        }
    }
}
