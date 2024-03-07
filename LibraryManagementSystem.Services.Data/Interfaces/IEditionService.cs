using LibraryManagementSystem.Web.ViewModels.Edition;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IEditionService
    {
        Task<EditionFormModel> GetNewCreateEditionModelAsync();

        Task AddEditionAsync(EditionFormModel model);

        Task<EditionFormModel?> GetEditionForEditByIdAsync(string id);

        Task EditBookAsync(string id, EditionFormModel model);
    }
}
