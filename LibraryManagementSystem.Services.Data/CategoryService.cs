using LibraryManagementSystem.Data;
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
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == model.Name);

            if (category != null)
            {
                throw new InvalidOperationException("Category with the same name already exists.");
            }

            var cat = new LibraryManagementSystem.Data.Models.Category
            {
                Name = model.Name
            };

            await dbContext.AddAsync(cat);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllCategoriesViewModel>> GetAllCategoriesAsync()
        {
            return await this.dbContext.Categories
                .AsNoTracking()
                .Select(c => new AllCategoriesViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    BooksCount = c.BooksCategories.Count(),
                })
                .ToListAsync();
        }

        public async Task<CategoryFormModel?> GetCategoryForEditByIdAsync(int categoryId)
        {
           var categoryToEdit = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            if (categoryToEdit != null)
            {
                CategoryFormModel category = new CategoryFormModel()
                {
                    Name = categoryToEdit.Name,
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

        public async Task<string> GetCategoryNameByCategoryIdAsync(int categoryId)
        {
            var categoryName = await dbContext.BooksCategories
                                              .Where(bc => bc.CategoryId == categoryId)
                                              .Select(bc => bc.Category.Name)
                                              .FirstOrDefaultAsync();

            if (categoryName == null)
            {
                return string.Empty;
            }

            return categoryName;
        }

        public async Task<string> GetCategoryNameByBookIdAsync(string bookId)
        {
            var categoryName = await dbContext.BooksCategories
                                               .Where(bc => bc.BookId.ToString() == bookId)
                                               .Select(c => c.Category.Name)
                                               .FirstOrDefaultAsync();

            if (categoryName == null)
            {
                return string.Empty;
            }

            return categoryName;
        }

        public async Task<int> GetCategoryIdByBookIdAsync(string bookId)
        {
            return await dbContext.BooksCategories
                                  .Where(bc => bc.BookId.ToString() == bookId)
                                  .Select(bc => bc.CategoryId)
                                  .FirstOrDefaultAsync();
        }
    }
}
