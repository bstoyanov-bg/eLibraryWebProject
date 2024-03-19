using LibraryManagementSystem.Web.ViewModels.Rating;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IRatingService
    {
        Task AddRatingAsync(RatingFormModel model);
    }
}
