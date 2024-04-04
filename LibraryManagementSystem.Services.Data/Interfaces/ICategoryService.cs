using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.ViewModels.Category;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryFormModel categoryModel);

        Task EditCategoryAsync(int categoryId, CategoryFormModel categoryModel);

        Task DeleteCategoryAsync(int categoryId);

        Task<Category?> GetCategoryByIdAsync(int categoryId);

        Task<CategoryFormModel?> GetCategoryForEditByIdAsync(int categoryId);

        Task<bool> CategoryExistByNameAsync(string categoryName);

        Task<bool> CategoryExistByIdAsync(int categoryId);

        Task<int> GetCategoryIdByBookIdAsync(string bookId);

        Task<int> GetCountOfActiveCategoriesAsync();

        Task<int> GetCountOfDeletedCategoriesAsync();

        Task<IEnumerable<AllCategoriesViewModel>> GetAllCategoriesAsync();

        Task<IEnumerable<string>> GetAllCategoriesNamesAsync();
    }
}
