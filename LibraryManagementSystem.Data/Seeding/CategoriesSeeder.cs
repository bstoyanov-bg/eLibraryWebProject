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
                        Name = "Academic book",
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Name = "Adventure stories",
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Name = "Classics",
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Name = "Mystery",
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Name = "Roman",
                        IsDeleted = false,
                    },
                    new Category
                    {
                        Name = "Science fiction",
                        IsDeleted = false,
                    }
                };

            await dbContext.Categories.AddRangeAsync(catgeories);
            await dbContext.SaveChangesAsync();
        }
    }
}
