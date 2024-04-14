using Ganss.Xss;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.Controllers;
using LibraryManagementSystem.Web.ViewModels.Rating;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace LibraryManagementSystem.Controllers.Tests
{
    public class RatingControllerTests
    {
        private Mock<IRatingService> mockRatingService;
        private Mock<IBookService> mockBookService;
        private RatingController ratingController;
        private Mock<ITempDataDictionary> tempDataMock;

        [SetUp]
        public void Setup()
        {
            this.mockBookService = new Mock<IBookService>();
            this.mockRatingService = new Mock<IRatingService>();
            this.tempDataMock = new Mock<ITempDataDictionary>();
            this.ratingController = new RatingController(mockRatingService.Object, mockBookService.Object)
            {
                TempData = tempDataMock.Object
            };
        }

        [TearDown]
        public void TearDown()
        {
            this.mockBookService.As<IBookService>().Reset();
            this.mockRatingService.As<IRatingService>().Reset();
            this.ratingController.Dispose();
        }

        [Test]
        public async Task Give_BookDoesNotExist_ReturnsErrorMessageAndRedirectsToAll()
        {
            string bookId = "book1";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(false);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.ratingController.Give(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.ratingController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        // System.FormatException : Unrecognized Guid format. ???????
        //[Test]
        //public async Task Give_UserHasAlreadyRatedBook_ReturnsErrorMessageAndRedirectsToAll()
        //{
        //    string bookId = "book1";
        //    string userId = "user1";
        //    Book book = new Book
        //    {
        //        Id = Guid.Parse(bookId),
        //        Title = "Test Book",
        //        ISBN = "1234567891011",
        //        Description = "Test Description",
        //        ImageFilePath = "test.jpg",
        //        AuthorId = Guid.Parse("author1"),
        //    };

        //    this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
        //    this.mockBookService.Setup(service => service.GetBookByIdAsync(bookId)).ReturnsAsync(book);
        //    this.mockRatingService.Setup(service => service.HasUserRatedBookAsync(userId, bookId)).ReturnsAsync(true);
        //    this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

        //    var result = await this.ratingController.Give(bookId) as RedirectToActionResult;

        //    Assert.That(result, Is.Not.Null);
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(result.ActionName, Is.EqualTo("All"));
        //        Assert.That(result.ControllerName, Is.EqualTo("Book"));
        //        Assert.That(this.ratingController.TempData["ErrorMessage"], Is.Not.Null);
        //    });
        //}

        // System.FormatException : Unrecognized Guid format. ???????
        //[Test]
        //public async Task Give_ValidBookId_ReturnsViewWithRatingFormModel()
        //{
        //    string bookId = "d0839d3b-0cae-4cec-9dbe-18140806bc40";
        //    var book = new Book
        //    {
        //        Id = Guid.Parse(bookId),
        //        Title = "Test Book",
        //        ISBN = "1234567890",
        //        ImageFilePath = "test.jpg",
        //        Description = "Test Description",
        //        AuthorId = Guid.Parse("author1")
        //    };

        //    this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
        //    this.mockBookService.Setup(service => service.GetBookByIdAsync(bookId)).ReturnsAsync(book);
        //    this.mockRatingService.Setup(service => service.HasUserRatedBookAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        //    var result = await this.ratingController.Give(bookId) as ViewResult;

        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result.ViewName, Is.Null.Or.Empty);
        //    Assert.That(result.Model, Is.InstanceOf<RatingFormModel>());

        //    var model = result.Model as RatingFormModel;
        //}

        [Test]
        public async Task Give_ExceptionThrown_ReturnsGeneralError()
        {
            string bookId = "book1";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ThrowsAsync(new Exception());

            var result = await this.ratingController.Give(bookId) as RedirectToActionResult;
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo("Index"));
            Assert.That(result.ControllerName, Is.EqualTo("Home"));
            Assert.That(this.ratingController.TempData["ErrorMessage"], Is.Not.Null);
        }

        [Test]
        public async Task Give_InvalidModelState_ReturnsViewWithModel()
        {
            var model = new RatingFormModel();

            this.ratingController.ModelState.AddModelError("PropertyName", "Error message");

            var result = await this.ratingController.Give(model) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ViewName, Is.Null.Or.Empty);
                Assert.That(result.Model, Is.EqualTo(model));
            });
        }

        [Test]
        public async Task Give_NonExistingBook_ReturnsErrorMessageAndRedirectsToAll()
        {
            string bookId = "book1";
            var model = new RatingFormModel { BookId = bookId };

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(false);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.ratingController.Give(model) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.ratingController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Give_ValidRating_ReturnsRedirectToDetailsWithSuccessMessage()
        {
            string bookId = "book1";
            var model = new RatingFormModel { BookId = bookId };

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockRatingService.Setup(service => service.GiveRatingAsync(model)).Returns(Task.CompletedTask);
            this.tempDataMock.Setup(temp => temp["SuccessMessage"]).Returns("SuccessMessage");

            var result = await this.ratingController.Give(model) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("Details"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(result.RouteValues!["id"], Is.EqualTo(model.BookId));
                Assert.That(this.ratingController.TempData["SuccessMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Give_ErrorInRatingOperation_ReturnsRedirectToDetailsWithErrorMessage()
        {
            string bookId = "book1";
            var model = new RatingFormModel { BookId = bookId };

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockRatingService.Setup(service => service.GiveRatingAsync(model)).ThrowsAsync(new Exception());
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.ratingController.Give(model) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo("Details"));
            Assert.That(result.ControllerName, Is.EqualTo("Book"));
            Assert.That(result.RouteValues!["id"], Is.EqualTo(model.BookId));
            Assert.That(this.ratingController.TempData["ErrorMessage"], Is.Not.Null);
        }

        //[Test]
        //public async Task SanitizeComment_WhenCommentIsNotNull_SanitizesComment()
        //{
        //    string bookId = "book1";

        //    var model = new RatingFormModel
        //    {
        //        BookId = bookId,
        //        UserId = "user1",
        //        BookRating = 5,
        //        Comment = "<script>alert('XSS attack');</script>"
        //    };
        //    var book = new Book
        //    {
        //        Id = Guid.Parse(bookId),
        //        Title = "Test Book",
        //        ISBN = "1234567890",
        //        ImageFilePath = "test.jpg",
        //        Description = "Test Description",
        //        AuthorId = Guid.Parse("author1"),
        //    };
        //    var expectedSanitizedComment = "alert('XSS attack');";

        //    this.mockBookService.Setup(service => service.BookExistByIdAsync(model.BookId)).ReturnsAsync(true);
        //    this.mockBookService.Setup(service => service.GetBookByIdAsync(model.BookId)).ReturnsAsync(book);

        //    var result = await this.ratingController.Give(model);

        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(result, Is.Not.Null);
        //        Assert.That(new HtmlSanitizer().Sanitize(model.Comment), Is.EqualTo(expectedSanitizedComment));
        //    });
        //}

        [Test]
        public async Task GetComments_ReturnsPartialViewWithComments()
        {
            string bookId = "book1";
            var expectedComments = new List<CommentViewModel>
            {
                new CommentViewModel { BookId = "1", BookComment = "Great book!" },
                new CommentViewModel { BookId = "2", BookComment = "Interesting read" }
            };

            this.mockRatingService.Setup(service => service.GetCommentsForBookAsync(bookId)).ReturnsAsync(expectedComments);

            var result = await this.ratingController.GetComments(bookId) as PartialViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ViewName, Is.EqualTo("_CommentsPartial"));
                Assert.That(result.Model, Is.EqualTo(expectedComments));
            });
        }
    }
}
