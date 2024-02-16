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
                    //new Category
                    //{
                    //    Name = "Academic book",
                    //},
                    new Category
                    {
                        Name = "Adventure stories",
                    },
                    //new Category
                    //{
                    //    Name = "Classics",
                    //},
                    //new Category
                    //{
                    //    Name = "Mystery",
                    //},
                    //new Category
                    //{
                    //    Name = "Roman",
                    //},
                    //new Category
                    //{
                    //    Name = "Science fiction",
                    //}
                };

            await dbContext.Categories.AddRangeAsync(catgeories);
            await dbContext.SaveChangesAsync();
        }
    }
}
