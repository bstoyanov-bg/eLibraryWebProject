using LibraryManagementSystem.Web.ViewModels.Category;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ICategoryService
    {
        Task<int> CreateCategoryAsync(CategoryFormModel addCategoryInputModel);

        Task<IEnumerable<AllViewModel>> GetAllCategoriesAsync();
    }
}
