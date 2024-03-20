using LibraryManagementSystem.Web.ViewModels.Rating;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IRatingService
    {
        Task GiveRatingAsync(RatingFormModel model);

        //Task<RatingFormModel> GetCreateNewRatingModelAsync(string bookId);
    }
}
