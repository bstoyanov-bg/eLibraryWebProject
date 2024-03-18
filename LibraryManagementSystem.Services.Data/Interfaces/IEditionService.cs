using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.ViewModels.Edition;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IEditionService
    {
        // ready
        Task AddEditionAsync(EditionFormModel model);

        // ready
        Task EditBookEditionAsync(string editionId, EditionFormModel model);

        // ready
        Task DeleteEditionAsync(string editionId);

        // ready
        Task<Edition?> GetEditionByIdAsync(string editionId);

        // ready
        Task<EditionFormModel> GetCreateNewEditionModelAsync();

        // ready
        Task<EditionFormModel?> GetEditionForEditByIdAsync(string editionId);

        // ready
        Task<bool> EditionExistByIdAsync(string editionId);

        // ready
        Task<bool> EditionExistByVersionPublisherAndBookIdAsync(string version, string publisher, string bookId);

        // ready
        Task<string> GetBookIdByEditionIdAsync(string editionId);

        Task<IEnumerable<Edition>> GetAllBookEditionsForBookByBookId(string bookId);
    }
}
