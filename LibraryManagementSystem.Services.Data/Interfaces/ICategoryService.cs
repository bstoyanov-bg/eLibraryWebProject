using LibraryManagementSystem.Web.ViewModels.Category;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ICategoryService
    {
        // ready
        Task AddCategoryAsync(CategoryFormModel categoryModel);

        Task EditCategoryAsync(int categoryId, CategoryFormModel categoryModel);

        // ready
        Task DeleteCategoryAsync(int categoryId);

        // ready
        Task<CategoryFormModel> GetCategoryForEditByIdAsync(int categoryId);

        // ready
        Task<bool> CategoryExistByNameAsync(string categoryName);

        // ready
        Task<bool> CategoryExistByIdAsync(int categoryId);

        //// REMOVE
        //Task<string> GetCategoryNameByCategoryIdAsync(int categoryId);
        //// REMOVE
        //Task<string> GetCategoryNameByBookIdAsync(string bookId);

        // ready
        Task<int> GetCategoryIdByBookIdAsync(string bookId);


        // ready
        Task<IEnumerable<AllCategoriesViewModel>> GetAllCategoriesAsync();

        // ready
        Task<IEnumerable<string>> GetAllCategoriesNamesAsync();
    }
}
