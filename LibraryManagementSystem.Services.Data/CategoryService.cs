using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Category;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Data
{
    public class CategoryService : ICategoryService
    {
        private readonly ELibraryDbContext dbContext;

        public CategoryService(ELibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> CreateCategoryAsync(CategoryFormModel addCategoryInputModel)
        {
            // Validate input
            if (addCategoryInputModel == null)
            {
                throw new ArgumentNullException(nameof(addCategoryInputModel));
            }

            // Create a new Category object
            var category = new Category
            {
                Name = addCategoryInputModel.Name
            };

            await dbContext.AddAsync(category);
            await dbContext.SaveChangesAsync();

            // Return the newly created category
            return category.Id;
        }

        public async Task<IEnumerable<AllViewModel>> GetAllCategoriesAsync()
        {
            return await this.dbContext.Categories
                .Select(c => new AllViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();
        }
    }
}
