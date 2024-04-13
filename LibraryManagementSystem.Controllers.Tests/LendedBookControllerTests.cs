using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.Controllers;
using LibraryManagementSystem.Web.ViewModels.LendedBook;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Security.Claims;

namespace LibraryManagementSystem.Controllers.Tests
{
    [TestFixture]
    public class LendedBookControllerTests
    {
        private Mock<ILendedBookService> mockLendedBookService;
        private Mock<IBookService> mockBookService;
        private LendedBookController lendedBookController;
        private Mock<ITempDataDictionary> tempDataMock;

        [SetUp]
        public void Setup()
        {
            this.mockBookService = new Mock<IBookService>();
            this.mockLendedBookService = new Mock<ILendedBookService>();
            this.tempDataMock = new Mock<ITempDataDictionary>();
            this.lendedBookController = new LendedBookController(mockLendedBookService.Object, mockBookService.Object)
            {
                TempData = tempDataMock.Object
            };
        }

        [TearDown]
        public void TearDown()
        {
            this.mockBookService.As<IBookService>().Reset();
            this.mockLendedBookService.As<ILendedBookService>().Reset();
            this.lendedBookController.Dispose();
        }

        [Test]
        public async Task GetBook_BookExistsAndNotInCollection_AddsBookToCollectionAndRedirectsToMineWithSuccessMessage()
        {
            string userId = "user1";
            string bookId = "book1";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockLendedBookService.Setup(service => service.IsBookActiveInUserCollectionAsync(userId, bookId)).ReturnsAsync(false);
            this.mockLendedBookService.Setup(service => service.GetCountOfActiveBooksForUserAsync(userId)).ReturnsAsync(0);
            this.tempDataMock.Setup(temp => temp["SuccessMessage"]).Returns("SuccessMessage");

            var result = await this.lendedBookController.GetBook(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("Mine"));
                Assert.That(result.ControllerName, Is.EqualTo("LendedBook"));
                Assert.That(this.lendedBookController.TempData["SuccessMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task GetBook_BookExistsAndAlreadyInCollection_ReturnsErrorMessageAndRedirectsToAll()
        {
            string userId = "user1";
            string bookId = "book1";

            // Simulate authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            // Assign authenticated user to controller context
            lendedBookController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockLendedBookService.Setup(service => service.IsBookActiveInUserCollectionAsync(userId, bookId)).ReturnsAsync(true);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.lendedBookController.GetBook(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("Mine"));
                Assert.That(result.ControllerName, Is.EqualTo("LendedBook"));
                Assert.That(this.lendedBookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task GetBook_UserReachedMaxBooks_ReturnsErrorMessageAndRedirectsToAll()
        {
            string userId = "user1";
            string bookId = "book1";

            // Simulate authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            // Assign authenticated user to controller context
            lendedBookController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockLendedBookService.Setup(service => service.IsBookActiveInUserCollectionAsync(userId, bookId)).ReturnsAsync(false);
            this.mockLendedBookService.Setup(service => service.GetCountOfActiveBooksForUserAsync(userId)).ReturnsAsync(5); // The user has reached max books
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.lendedBookController.GetBook(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.lendedBookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task GetBook_BookDoesNotExist_ReturnsErrorMessageAndRedirectsToAll()
        {
            string bookId = "nonExistingBook";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(false);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.lendedBookController.GetBook(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.lendedBookController.TempData["ErrorMessage"], Is.Not.Null);
            });

        }

        [Test]
        public async Task GetBook_ServiceThrowsException_ReturnsErrorMessageAndRedirectsToAll()
        {
            string bookId = "book1";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ThrowsAsync(new Exception());
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.lendedBookController.GetBook(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("Mine"));
                Assert.That(result.ControllerName, Is.EqualTo("LendedBook"));
                Assert.That(this.lendedBookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Mine_ReturnsViewWithModel()
        {
            string userId = "user1";
            var expectedModel = new List<MyBooksViewModel>();

            // Simulate authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            // Assign authenticated user to controller context
            lendedBookController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            this.mockLendedBookService.Setup(service => service.GetMyBooksAsync(userId)).ReturnsAsync(expectedModel);

            var result = await this.lendedBookController.Mine() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(expectedModel));
        }

        [Test]
        public async Task ReturnBook_BookExists_ReturnsSuccessMessageAndRedirectsToMine()
        {
            string userId = "user1";
            string bookId = "book1";

            // Simulate authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            // Assign authenticated user to controller context
            lendedBookController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockLendedBookService.Setup(service => service.IsBookReturnedAsync(userId, bookId)).ReturnsAsync(true);
            this.tempDataMock.Setup(temp => temp["SuccessMessage"]).Returns("SuccessMessage");

            var result = await this.lendedBookController.ReturnBook(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("Mine"));
                Assert.That(result.ControllerName, Is.EqualTo("LendedBook"));
                Assert.That(this.lendedBookController.TempData["SuccessMessage"], Is.Not.Null);
            });

        }

        [Test]
        public async Task ReturnBook_BookDoesNotExist_ReturnsErrorMessageAndRedirectsToAll()
        {
            string bookId = "book1";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(false);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.lendedBookController.ReturnBook(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.lendedBookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task ReturnBook_BookAlreadyReturned_ReturnsErrorMessageAndRedirectsToAll()
        {
            string userId = "user1";
            string bookId = "book1";

            // Simulate authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            // Assign authenticated user to controller context
            lendedBookController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockLendedBookService.Setup(service => service.IsBookReturnedAsync(userId, bookId)).ReturnsAsync(false);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.lendedBookController.ReturnBook(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.lendedBookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task ReturnBook_ExceptionThrown_ReturnsErrorMessageAndRedirectsToMine()
        {
            string userId = "user1";
            string bookId = "book1";

            // Simulate authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            // Assign authenticated user to controller context
            lendedBookController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockLendedBookService.Setup(service => service.IsBookReturnedAsync(userId, bookId)).ReturnsAsync(true);
            this.mockLendedBookService.Setup(service => service.ReturnBookAsync(userId, bookId)).ThrowsAsync(new Exception());
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.lendedBookController.ReturnBook(bookId) as RedirectToActionResult;
 
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("Mine"));
                Assert.That(result.ControllerName, Is.EqualTo("LendedBook"));
                Assert.That(this.lendedBookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task ReturnAll_Success_ReturnsSuccessMessageAndRedirectsToIndex()
        {
            string userId = "user1";

            // Simulate authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            // Assign authenticated user to controller context
            lendedBookController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            this.mockLendedBookService.Setup(service => service.ReturnAllBooksAsync(userId)).Returns(Task.CompletedTask);
            this.tempDataMock.Setup(temp => temp["SuccessMessage"]).Returns("SuccessMessage");

            var result = await this.lendedBookController.ReturnAll() as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo("Index"));
            Assert.That(result.ControllerName, Is.EqualTo("Home"));
            Assert.That(this.lendedBookController.TempData["SuccessMessage"], Is.Not.Null);
        }

        [Test]
        public async Task ReturnAll_ExceptionThrown_ReturnsErrorMessageAndRedirectsToIndex()
        {
            string userId = "user1";

            // Simulate authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            // Assign authenticated user to controller context
            lendedBookController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            this.mockLendedBookService.Setup(service => service.ReturnAllBooksAsync(userId)).ThrowsAsync(new Exception("Simulated exception"));
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.lendedBookController.ReturnAll() as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("Index"));
                Assert.That(result.ControllerName, Is.EqualTo("Home"));
                Assert.That(this.lendedBookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

    }
}
