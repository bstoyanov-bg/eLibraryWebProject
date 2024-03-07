using LibraryManagementSystem.Web.ViewModels.Edition;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IEditionService
    {
        Task<EditionFormModel> GetNewCreateEditionModelAsync();

        Task AddEditionAsync(EditionFormModel model);
    }
}
