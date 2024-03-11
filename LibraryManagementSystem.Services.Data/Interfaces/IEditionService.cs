using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.ViewModels.Edition;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IEditionService
    {
        Task<EditionFormModel> GetNewCreateEditionModelAsync();

        Task AddEditionAsync(EditionFormModel model);

        Task<Edition?> GetEditionByIdAsync(string editionId);

        Task<EditionFormModel?> GetBookEditionForEditByIdAsync(string editionId);

        Task EditBookEditionAsync(string editionId, EditionFormModel model);

        Task DeleteBookEditionAsync(string editionId);

        Task<string> GetBookIdByEditionIdAsync(string editionId);

        Task<IEnumerable<Edition>> GetAllBookEditionsForBookbyBookId(string bookId);
    }
}
