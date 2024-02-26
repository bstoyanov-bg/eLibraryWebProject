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

        public async Task AddCategoryAsync(CategoryFormModel model)
        {
            // Validate input
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == model.Name);

            //Check if category exists in DB
            if (category != null)
            {
                throw new InvalidOperationException("Category with the same name already exists.");
            }

            // Create a new Category object
            var cat = new Category
            {
                Name = model.Name
            };

            await dbContext.AddAsync(cat);
            await dbContext.SaveChangesAsync();
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

        public async Task<CategoryFormModel?> GetCategoryByIdAsync(int categoryId)
        {
           var categoryById = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            if (categoryById != null)
            {
                CategoryFormModel category = new CategoryFormModel()
                {
                    Name = categoryById.Name,
                };

                return category;
            }

            return null;
        }

        public async Task EditCategoryAsync(int categoryId, CategoryFormModel model)
        {
            var categoryToEdit = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            if (categoryToEdit != null)
            {
                categoryToEdit.Name = model.Name;

            }

            await dbContext.SaveChangesAsync();
        }
    }
}
