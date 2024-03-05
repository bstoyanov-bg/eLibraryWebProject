using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Data.Seeding.Contracts;

namespace LibraryManagementSystem.Data.Seeding
{
    public class ELibraryDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ELibraryDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var seeders = new List<ISeeder>
                          {
                              new RolesSeeder(),
                              new AuthorsSeeder(),
                              new CategoriesSeeder(),
                              new BooksSeeder(),
                              new BooksCategoriesSeeder(),
                              new UsersSeeder(),
                          };


            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
