using LibraryManagementSystem.Web.ViewModels.Category;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryFormModel model);

        Task<IEnumerable<AllViewModel>> GetAllCategoriesAsync();
    }
}
