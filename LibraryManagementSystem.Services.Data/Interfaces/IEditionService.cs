using LibraryManagementSystem.Web.ViewModels.Edition;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IEditionService
    {
        Task<EditionFormModel> GetNewCreateEditionModelAsync();

        Task AddEditionAsync(EditionFormModel model);

        Task<EditionFormModel?> GetEditionForEditByIdAsync(string editionId);

        Task EditBookEditionAsync(string id, EditionFormModel model);

        Task DeleteEditionAsync(string editionId);

        Task<string> GetBookIdByEditionIdAsync(string editionId);
    }
}
