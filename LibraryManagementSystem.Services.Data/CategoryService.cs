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


        // ready
        public async Task AddCategoryAsync(CategoryFormModel categoryModel)
        {
            var category = new Category
            {
                Name = categoryModel.Name
            };

            await dbContext.AddAsync(category);
            await dbContext.SaveChangesAsync();
        }

        public async Task EditCategoryAsync(int categoryId, CategoryFormModel categoryModel)
        {
            var categoryToEdit = await GetCategoryByIdAsync(categoryId);

            if (categoryToEdit != null)
            {
                categoryToEdit.Name = categoryModel.Name;
            }

            await dbContext.SaveChangesAsync();
        }

        // ready
        public async Task DeleteCategoryAsync(int categoryId)
        {
            var categoryToDelete = await GetCategoryByIdAsync(categoryId);

            if (categoryToDelete != null)
            {
                categoryToDelete.IsDeleted = true;
            }

            await this.dbContext.SaveChangesAsync();
        }

        // ready
        public async Task<Category?> GetCategoryByIdAsync(int categoryId)
        {
            return await dbContext
                 .Categories
                 .FirstOrDefaultAsync(c => c.IsDeleted == false &&
                                  c.Id == categoryId);
        }

        // ready
        public async Task<CategoryFormModel?> GetCategoryForEditByIdAsync(int categoryId)
        {
            var categoryToEdit = await GetCategoryByIdAsync(categoryId);

            if (categoryToEdit != null)
            {
                return new CategoryFormModel
                {
                    Name = categoryToEdit.Name,
                };
            }

            return null;
        }

        // ready
        public async Task<bool> CategoryExistByNameAsync(string categoryName)
        {
            return await this.dbContext
                .Categories
                .AsNoTracking()
                .Where(c => c.IsDeleted == false)
                .AnyAsync(c => c.Name == categoryName);
        }

        // ready
        public async Task<bool> CategoryExistByIdAsync(int categoryId)
        {
            return await this.dbContext
                .Categories
                .AsNoTracking()
                .Where(c => c.IsDeleted == false)
                .AnyAsync(c => c.Id == categoryId);
        }

        // ready
        public async Task<int> GetCategoryIdByBookIdAsync(string bookId)
        {
            return await dbContext.BooksCategories
                                  .Where(bc => bc.BookId.ToString() == bookId)
                                  .AsNoTracking()
                                  .Select(bc => bc.CategoryId)
                                  .FirstAsync();
        }

        // ready
        public async Task<IEnumerable<AllCategoriesViewModel>> GetAllCategoriesAsync()
        {
            return await this.dbContext
                .Categories
                .AsNoTracking()
                .Where(c => c.IsDeleted == false)
                .Select(c => new AllCategoriesViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    BooksCount = c.BooksCategories.Count(),
                })
                .ToListAsync();
        }

        // ready
        public async Task<IEnumerable<string>> GetAllCategoriesNamesAsync()
        {
            return await this.dbContext
                .Categories
                .AsNoTracking()
                .Where(c => c.IsDeleted == false)
                .Select(c => c.Name)
                .ToListAsync();
        }
    }
}
