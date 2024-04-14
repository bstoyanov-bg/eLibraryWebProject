using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data;
using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using static LibraryManagementSystem.Services.Tests.DatabaseSeeder;

namespace LibraryManagementSystem.Services.Tests
{
    [TestFixture]
    public class RatingServiceTests
    {
        private DbContextOptions<ELibraryDbContext> dbOptions;
        private ELibraryDbContext dbContext;

        private IRatingService ratingService;

        private Mock<IBookService> bookServiceMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<ELibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "ELibraryTestDb")
                .Options;

            this.bookServiceMock = new Mock<IBookService>();
        }

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new ELibraryDbContext(this.dbOptions);
            this.dbContext.Database.EnsureCreated();
            SeedDatabase(this.dbContext);

            this.ratingService = new RatingService(this.dbContext, bookServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        //[Test]
        //public async Task GiveRatingAsync_ValidModel_AddsRatingToBookAndSavesChanges()
        //{
        //    var model = new RatingFormModel
        //    {
        //        BookId = "F08224F2-E2FA-426D-BEEE-E2DAA72B5EB6",
        //        UserId = "89A4BE4E-2B5E-4FB7-AA5A-E3FEEBBA0153",
        //        BookRating = 4.0m,
        //        Comment = "I thoroughly enjoyed reading this book.",
        //    };
        //    var book = FirstBook;

        //    this.bookServiceMock.Setup(service => service.GetBookByIdAsync(model.BookId)).ReturnsAsync(book);

        //    await this.ratingService.GiveRatingAsync(model);

        //    Assert.That(book!.Ratings.Count, Is.EqualTo(1));
        //}

        [Test]
        public async Task GetCommentsForBookAsync_ReturnsComments()
        {
            var bookId = FirstBook!.Id.ToString();
            var ratings = new List<Rating>
            {
                FirstRating!,
                ThirdRating!,
            };

            var result = await ratingService.GetCommentsForBookAsync(bookId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));

            var firstComment = result.First();
            Assert.Multiple(() =>
            {
                Assert.That(firstComment.BookId, Is.EqualTo(bookId));
                Assert.That(firstComment.BookComment, Is.EqualTo(FirstRating!.Comment));
                Assert.That(firstComment.BookRating, Is.EqualTo(4));
                Assert.That(firstComment.Username, Is.EqualTo(FirstRating!.User.UserName!));
            });

            var secondComment = result.Skip(1).First();
            Assert.Multiple(() =>
            {
                Assert.That(secondComment.BookId, Is.EqualTo(bookId));
                Assert.That(secondComment.BookComment, Is.EqualTo(SecondRating!.Comment));
                Assert.That(secondComment.BookRating, Is.EqualTo(4.5));
                Assert.That(firstComment.Username, Is.EqualTo(ThirdRating!.User.UserName!));
            });
        }

        [Test]
        public async Task GetAverageRatingForBookAsync_ReturnsAverageRating()
        {
            string bookId = FirstBook!.Id.ToString();
            var ratings = new List<Rating>
            {
                FirstRating!,
                ThirdRating!,
            };

            var result = await this.ratingService.GetAverageRatingForBookAsync(bookId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(4.25m));
        }

        [Test]
        public async Task GetAverageRatingForBookAsync_NoRatings_ReturnsNull()
        {
            var bookId = FifthBook!.Id.ToString();

            var result = await this.ratingService.GetAverageRatingForBookAsync(bookId);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public async Task HasUserGaveRatingToBookAsync_UserHasRatedBook_ReturnsTrue()
        {
            string userId = ThirdUser!.Id.ToString();
            string bookId = FirstBook!.Id.ToString();

            var result = await this.ratingService.HasUserGaveRatingToBookAsync(userId, bookId);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task HasUserGaveRatingToBookAsync_UserHasNotRatedBook_ReturnsFalse()
        {
            string userId = ThirdUser!.Id.ToString();
            string bookId = NinthBook!.Id.ToString();

            var result = await this.ratingService.HasUserGaveRatingToBookAsync(userId, bookId);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetCountOfRatingsAsync_ReturnsCountOfRatings()
        {
            var ratings = new List<Rating>
            {
                FirstRating!,
                SecondRating!,
                ThirdRating!,
            };

            var result = await ratingService.GetCountOfRatingsAsync();

            Assert.That(result, Is.EqualTo(ratings.Count));
        }
    }
}
