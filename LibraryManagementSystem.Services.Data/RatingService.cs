using LibraryManagementSystem.Data;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Rating;

namespace LibraryManagementSystem.Services.Data
{
    public class RatingService : IRatingService
    {
        private readonly ELibraryDbContext dbContext;

        public RatingService(ELibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddRatingAsync(RatingFormModel model)
        {
            throw new NotImplementedException();
        }
    }
}
