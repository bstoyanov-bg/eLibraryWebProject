using LibraryManagementSystem.Web.ViewModels.Category;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface ICategoryService
    {
        Task CreateCategoryAsync(AddCategoryInputModel addCategoryInputModel);
    }
}
