using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.ViewModels.Edition;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IEditionService
    {
        Task AddEditionAsync(EditionFormModel model);

        Task EditBookEditionAsync(string editionId, EditionFormModel model);

        Task DeleteEditionAsync(string editionId);

        Task<Edition?> GetEditionByIdAsync(string editionId);

        Task<EditionFormModel> GetCreateNewEditionModelAsync();

        Task<EditionFormModel?> GetEditionForEditByIdAsync(string editionId);

        Task<bool> EditionExistByIdAsync(string editionId);

        Task<bool> EditionExistByVersionPublisherAndBookIdAsync(string version, string publisher, string bookId);

        Task<string> GetBookIdByEditionIdAsync(string editionId);

        Task<IEnumerable<Edition>> GetAllBookEditionsByBookIdAsync(string bookId);
    }
}
