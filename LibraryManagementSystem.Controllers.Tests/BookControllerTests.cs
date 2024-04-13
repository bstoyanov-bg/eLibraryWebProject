using Ganss.Xss;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.Controllers;
using LibraryManagementSystem.Web.ViewModels.Book;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace LibraryManagementSystem.Controllers.Tests
{
    [TestFixture]
    public class BookControllerTests
    {
        private Mock<IBookService> mockBookService;
        private Mock<IFileService> mockFileService;
        private Mock<IMemoryCache> mockMemoryCache;
        private BookController bookController;
        private Mock<ITempDataDictionary> tempDataMock;

        [SetUp]
        public void Setup()
        {
            mockBookService = new Mock<IBookService>();
            mockFileService = new Mock<IFileService>();
            mockMemoryCache = new Mock<IMemoryCache>();
            tempDataMock = new Mock<ITempDataDictionary>();
            bookController = new BookController(mockBookService.Object, mockFileService.Object, mockMemoryCache.Object)
            {
                TempData = tempDataMock.Object
            };
        }

        [TearDown]
        public void TearDown()
        {
            mockBookService.As<IBookService>().Reset();
            mockFileService.As<IFileService>().Reset();
            mockMemoryCache.As<IMemoryCache>().Reset();
            bookController.Dispose();
        }

        // Some problem with the mockMemoryCache ???
        //[Test]
        //public async Task All_DataInCache_ReturnsViewWithCachedData()
        //{
        //    var queryModel = new AllBooksQueryModel { CurrentPage = 1, BooksPerPage = 10 };
        //    var cachedData = new AllBooksFilteredAndPagedServiceModel
        //    {
        //        Books = new List<AllBooksViewModel>()
        //        {
        //            new AllBooksViewModel { Title = "Glina", AuthorName = "Victoria Beshakliiska", Category = "Nice", EditionsCount = 0, YearPublished = new DateOnly(2000, 1, 1) }
        //        },
        //        TotalBooksCount = 1
        //    };

        //    string cacheKey = $"BooksCache_{queryModel.CurrentPage}_{queryModel.BooksPerPage}";

        //    mockMemoryCache.Setup(cache => cache.TryGetValue(cacheKey, out It.Ref<AllAuthorsFilteredAndPagedServiceModel>.IsAny!))
        //       .Returns(true)
        //       .Callback((string cacheKey, out object value) =>
        //       {
        //           value = new AllAuthorsFilteredAndPagedServiceModel();
        //       });


        //    var result = await this.bookController.All(queryModel) as ViewResult;

        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result.Model, Is.EqualTo(queryModel));
        //}

        [Test]
        public async Task Add_BookFormModelRetrievedSuccessfully_ReturnsViewWithModel()
        {
            var expectedModel = new BookFormModel();

            this.mockBookService.Setup(service => service.GetCreateNewBookModelAsync()).ReturnsAsync(expectedModel);

            var result = await this.bookController.Add() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(expectedModel));
        }

        [Test]
        public async Task Add_ExceptionOccursDuringModelRetrieval_ReturnsGeneralError()
        {
            this.mockBookService.Setup(service => service.GetCreateNewBookModelAsync()).ThrowsAsync(new Exception());

            var result = await bookController.Add() as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("Index"));
                Assert.That(result.ControllerName, Is.EqualTo("Home"));
            });
        }

        [Test]
        public async Task Add_InvalidModelState_ReturnsViewWithModel()
        {
            var model = new BookFormModel();

            this.bookController.ModelState.AddModelError("Title", "Title is required.");

            var result = await this.bookController.Add(model, null) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Add_BookWithSameTitleAndAuthorExists_ReturnsRedirectToAllWithErrorMessage()
        {
            var model = new BookFormModel
            {
                Title = "Glina",
                YearPublished = new DateOnly(2000, 1, 1),
                Description = "Test Description",
                Publisher = "Test Publisher",
                AuthorId = "1",
                CategoryId = 1,
            };

            this.mockBookService.Setup(service => service.BookExistByTitleAndAuthorIdAsync(model.Title, model.AuthorId)).ReturnsAsync(true);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.bookController.Add(model, null) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.bookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Add_SuccessfullyAddsBook_ReturnsRedirectToEditWithSuccessMessage()
        {
            var model = new BookFormModel
            {
                Title = "Glina",
                YearPublished = new DateOnly(2000, 1, 1),
                Description = "Test Description",
                Publisher = "Test Publisher",
                AuthorId = "1",
                CategoryId = 1,
            };

            this.mockBookService.Setup(service => service.BookExistByTitleAndAuthorIdAsync(model.Title, model.AuthorId)).ReturnsAsync(false);
            this.mockBookService.Setup(service => service.AddBookAsync(model)).ReturnsAsync(new Book());
            this.tempDataMock.Setup(temp => temp["SuccessMessage"]).Returns("SuccessMessage");

            var result = await this.bookController.Add(model, null) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("Edit"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.bookController.TempData["SuccessMessage"], Is.Not.Null);
            });
        }


        [Test]
        public async Task Add_BookWithSameISBNExists_ReturnsRedirectToAllWithErrorMessage()
        {
            var model = new BookFormModel { ISBN = "978-4-0743-2365-4" };

            this.mockBookService.Setup(service => service.BookExistByISBNAsync(model.ISBN)).ReturnsAsync(true);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.bookController.Add(model, null) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.bookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Add_WithBookImageUpload()
        {
            var model = new BookFormModel();
            var bookImage = new FormFile(null!, 0, 0, "bookImage", "bookImage.jpg");
            var addedBook = new Book
            {
                Title = "Glina",
                ISBN = "978-4-0743-2365-4",
                YearPublished = new DateOnly(2000, 1, 1),
                Description = "Test Description",
                Publisher = "Test Publisher",
                AuthorId = Guid.Parse("1394B818-A8A6-474F-B123-5AD206C5F49D"),
                ImageFilePath = "path/to/image/file",
            };

            this.mockBookService.Setup(service => service.BookExistByTitleAndAuthorIdAsync(model.Title, model.AuthorId)).ReturnsAsync(false);
            this.mockBookService.Setup(service => service.AddBookAsync(model)).ReturnsAsync(addedBook);
            this.tempDataMock.Setup(temp => temp["SuccessMessage"]).Returns("SuccessMessage");

            var result = await this.bookController.Add(model, bookImage) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("Edit"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.bookController.TempData["SuccessMessage"], Is.Not.Null);
            });
            this.mockFileService.Verify(service => service.UploadImageFileAsync(addedBook.Id.ToString(), bookImage, "Book"), Times.Once);
        }

        [Test]
        public async Task Add_ExceptionDuringBookAddition_ReturnsRedirectToAllWithErrorMessage()
        {
            var model = new BookFormModel();
            var bookImage = new FormFile(null!, 0, 0, "bookImage", "bookImage.jpg");

            this.mockBookService.Setup(service => service.BookExistByTitleAndAuthorIdAsync(model.Title, model.AuthorId)).ReturnsAsync(false);
            this.mockBookService.Setup(service => service.AddBookAsync(model)).ThrowsAsync(new Exception("Error adding book"));
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.bookController.Add(model, bookImage) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.bookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Edit_BookExists_ReturnsViewWithBookModel()
        {
            string bookId = "1";
            var bookModel = new BookFormModel();

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockBookService.Setup(service => service.GetBookForEditByIdAsync(bookId)).ReturnsAsync(bookModel);

            var result = await this.bookController.Edit(bookId) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(bookModel));
        }

        [Test]
        public async Task Edit_BookDoesNotExist_ReturnsRedirectToAllWithErrorMessage()
        {
            string bookId = "1";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(false);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.bookController.Edit(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.bookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Edit_ExceptionOccurs_ReturnsGeneralError()
        {
            string bookId = "1";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ThrowsAsync(new Exception("Error checking book existence"));
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.bookController.Edit(bookId) as ViewResult;

            Assert.That(this.bookController.TempData["ErrorMessage"], Is.Not.Null);
        }

        [Test]
        public async Task Edit_ValidModelState_UpdatesBookAndRedirectsToDetails()
        {
            string bookId = "1";
            var model = new BookFormModel();
            var editedBook = new Book();
            var bookImage = new FormFile(null!, 0, 0, "bookImage", "bookImage.jpg");

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockBookService.Setup(service => service.EditBookAsync(bookId, model)).ReturnsAsync(editedBook);

            var result = await this.bookController.Edit(bookId, model, bookImage) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("Details"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(result.RouteValues!["id"], Is.EqualTo(bookId));
            });
        }

        [Test]
        public async Task Edit_InvalidModelState_ReturnsViewWithModel()
        {
            string bookId = "1";
            var model = new BookFormModel();

            this.bookController.ModelState.AddModelError("Title", "Title is required");

            var result = await this.bookController.Edit(bookId, model, null) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Edit_ExceptionOccurs_ReturnsRedirectToAllWithErrorMessage()
        {
            string bookId = "1";
            var model = new BookFormModel();

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ThrowsAsync(new Exception());
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.bookController.Edit(bookId, model, null) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("Details"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.bookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Add_ValidInputWithDescription_SanitizesDescription()
        {
            var model = new BookFormModel
            {
                Title = "Glina",
                YearPublished = new DateOnly(2000, 1, 1),
                Description = "<script>alert('xss');</script>",
                Publisher = "Test Publisher",
                AuthorId = "1",
                CategoryId = 1,
            };
            var bookImage = new Mock<IFormFile>().Object;

            var result = await this.bookController.Add(model, bookImage) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(new HtmlSanitizer().Sanitize(model.Description), Is.EqualTo(model.Description));
            });
        }

        [Test]
        public async Task Delete_BookExists_DeletesBookAndRedirectsToAllWithSuccessMessage()
        {
            string bookId = "1";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.tempDataMock.Setup(temp => temp["SuccessMessage"]).Returns("SuccessMessage");

            var result = await this.bookController.Delete(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.bookController.TempData["SuccessMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Delete_BookDoesNotExist_ReturnsRedirectToAllWithErrorMessage()
        {
            string bookId = "1";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(false);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.bookController.Delete(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.bookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Delete_BookDeletionFails_ReturnsRedirectToAllWithErrorMessage()
        {
            string bookId = "1";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockBookService.Setup(service => service.DeleteBookAsync(bookId)).ThrowsAsync(new Exception());
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.bookController.Delete(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.bookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Details_BookExists_ReturnsViewWithBookDetails()
        {
            string bookId = "1";
            var bookDetails = new BookDetailsViewModel();

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockBookService.Setup(service => service.GetBookDetailsForUserAsync(bookId)).ReturnsAsync(bookDetails);

            var result = await this.bookController.Details(bookId) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(bookDetails));
        }

        [Test]
        public async Task Details_BookDoesNotExist_ReturnsRedirectToAllWithErrorMessage()
        {
            string bookId = "1";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(false);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.bookController.Details(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Book"));
                Assert.That(this.bookController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Details_GetBookDetailsFails_ReturnsGeneralError()
        {
            string bookId = "1";

            this.mockBookService.Setup(service => service.BookExistByIdAsync(bookId)).ReturnsAsync(true);
            this.mockBookService.Setup(service => service.GetBookDetailsForUserAsync(bookId)).ThrowsAsync(new Exception("Simulated error"));

            var result = await this.bookController.Details(bookId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo("Index"));
            Assert.That(result.ControllerName, Is.EqualTo("Home"));
        }
    }
}
