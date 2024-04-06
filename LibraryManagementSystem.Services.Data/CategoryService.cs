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

        public async Task AddCategoryAsync(CategoryFormModel categoryModel)
        {
            Category category = new Category
            {
                Name = categoryModel.Name
            };

            await this.dbContext.AddAsync(category);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task EditCategoryAsync(int categoryId, CategoryFormModel categoryModel)
        {
            Category? categoryToEdit = await this.GetCategoryByIdAsync(categoryId);

            if (categoryToEdit != null)
            {
                categoryToEdit.Name = categoryModel.Name;
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            Category? categoryToDelete = await this.GetCategoryByIdAsync(categoryId);

            if (categoryToDelete != null)
            {
                categoryToDelete.IsDeleted = true;
            }

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId)
        {
            return await this.dbContext
                 .Categories
                 .FirstOrDefaultAsync(c => c.IsDeleted == false &&
                                  c.Id == categoryId);
        }

        public async Task<CategoryFormModel?> GetCategoryForEditByIdAsync(int categoryId)
        {
            Category? categoryToEdit = await this.GetCategoryByIdAsync(categoryId);

            if (categoryToEdit != null)
            {
                return new CategoryFormModel
                {
                    Name = categoryToEdit.Name,
                };
            }

            return null;
        }

        public async Task<bool> CategoryExistByNameAsync(string categoryName)
        {
            return await this.dbContext
                .Categories
                .AsNoTracking()
                .Where(c => c.IsDeleted == false)
                .AnyAsync(c => c.Name == categoryName);
        }

        public async Task<bool> CategoryExistByIdAsync(int categoryId)
        {
            return await this.dbContext
                .Categories
                .AsNoTracking()
                .Where(c => c.IsDeleted == false)
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task<int> GetCategoryIdByBookIdAsync(string bookId)
        {
            return await this.dbContext
                .BooksCategories
                .Where(bc => bc.BookId.ToString() == bookId)
                .AsNoTracking()
                .Select(bc => bc.CategoryId)
                .FirstAsync();
        }

        public async Task<int> GetCountOfActiveCategoriesAsync()
        {
            return await this.dbContext
                .Categories
                .AsNoTracking()
                .Where(u => u.IsDeleted == false)
                .CountAsync();
        }

        public async Task<int> GetCountOfDeletedCategoriesAsync()
        {
            return await this.dbContext
                .Categories
                .AsNoTracking()
                .Where(u => u.IsDeleted == true)
                .CountAsync();
        }

        public async Task<string> GetCategoryNameByBookIdAsync(string bookId)
        {
            return await this.dbContext
                .Categories
                .AsNoTracking()
                .Where(c => c.BooksCategories.Any(bc => bc.BookId == Guid.Parse(bookId)))
                .Select(c => c.Name)
                .FirstAsync();
        }

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
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

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
