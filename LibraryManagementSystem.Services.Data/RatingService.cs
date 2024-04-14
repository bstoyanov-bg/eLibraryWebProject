using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Rating;
using Microsoft.EntityFrameworkCore;

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
            Book? book = await this.bookService.GetBookByIdAsync(model.BookId);

            Rating rating = new Rating
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

        public async Task<IEnumerable<CommentViewModel>> GetCommentsForBookAsync(string bookId)
        {
            return await this.dbContext
                .Ratings
                .AsNoTracking()
                .Where(r => r.BookId == Guid.Parse(bookId))
                .OrderByDescending(r => r.CreatedOn)
                .Select(r => new CommentViewModel
                {
                    BookId = r.BookId.ToString(),
                    BookComment = r.Comment,
                    BookRating = r.BookRating,
                    Username = r.User.UserName!,
                    CreatedOn = r.CreatedOn,
                }).ToListAsync();
        }

        public async Task<decimal?> GetAverageRatingForBookAsync(string bookId)
        {
            var ratingsForBook = await this.dbContext
                .Ratings
                .Where(r => r.BookId.ToString() == bookId)
                .ToListAsync();

            if (ratingsForBook.Any())
            {
                decimal averageRating = ratingsForBook.Average(r => r.BookRating);
                return averageRating;
            }
            else
            {
                return 0;
            }
        }

        public async Task<bool> HasUserGaveRatingToBookAsync(string userId, string bookId)
        {
            return await this.dbContext
                .Ratings
                .AsNoTracking()
                .Where(r => r.UserId.ToString() == userId &&
                             r.BookId.ToString() == bookId)
                .AnyAsync();
        }

        public async Task<int> GetCountOfRatingsAsync()
        {
            return await this.dbContext
                .Ratings
                .AsNoTracking()
                .CountAsync();
        }
    }
}
