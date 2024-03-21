﻿using LibraryManagementSystem.Web.ViewModels.Rating;

namespace LibraryManagementSystem.Services.Data.Interfaces
{
    public interface IRatingService
    {
        Task GiveRatingAsync(RatingFormModel model);

        Task<decimal?> GetAverageRatingForBookAsync(string bookId);

        Task<bool> HasUserGaveRatingToBookAsync(string userId, string bookId);

        //Task<RatingFormModel> GetCreateNewRatingModelAsync(string bookId, string userId);
    }
}
