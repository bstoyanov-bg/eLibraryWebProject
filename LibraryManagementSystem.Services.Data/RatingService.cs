using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Rating;

namespace LibraryManagementSystem.Services.Data
{
    public class RatingService : IRatingService
    {
        private readonly ELibraryDbContext dbContext;
        private readonly IBookService bookService;

        public RatingService(ELibraryDbContext dbContext, IBookService bookService)
        {
            this.dbContext = dbContext;
            this.bookService = bookService;
        }

        public async Task GiveRatingAsync(RatingFormModel model)
        {
            var book = await bookService.GetBookByIdAsync(model.BookId);

            var rating = new Rating
            {
                BookRating = model.BookRating,
                Comment = model.Comment,
                BookId = Guid.Parse(model.BookId),
                UserId = Guid.Parse(model.UserId),
            };

            book!.Ratings.Add(rating);

            await this.dbContext.Ratings.AddAsync(rating);
            await this.dbContext.SaveChangesAsync();
        }

        //public RatingFormModel GetCreateNewRatingModelAsync(string bookId, string userId)
        //{
        //    RatingFormModel model = new RatingFormModel
        //    {
        //        BookId = bookId,
        //        UserId = userId,
        //    };

        //    return model;
        //}
    }
}
