using LibraryManagementSystem.Web.ViewModels.Rating;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IRatingService
    {
        Task GiveRatingAsync(RatingFormModel model);

        Task<IEnumerable<CommentViewModel>> GetCommentsForBookAsync(string bookId);

        Task<decimal?> GetAverageRatingForBookAsync(string bookId);

        Task<bool> HasUserGaveRatingToBookAsync(string userId, string bookId);
    }
}
