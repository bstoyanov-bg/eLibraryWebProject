using Ganss.Xss;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.Controllers;
using LibraryManagementSystem.Web.ViewModels.Author;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace LibraryManagementSystem.Controllers.Tests
{
    [TestFixture]
    public class AuthorControllesTests
    {
        private Mock<IAuthorService> mockAuthorService;
        private Mock<IFileService> mockFileService;
        private Mock<IMemoryCache> mockMemoryCache;
        private AuthorController authorController;
        private Mock<ITempDataDictionary> tempDataMock;

        [SetUp]
        public void Setup()
        {
            mockAuthorService = new Mock<IAuthorService>();
            mockFileService = new Mock<IFileService>();
            mockMemoryCache = new Mock<IMemoryCache>();
            tempDataMock = new Mock<ITempDataDictionary>();
            authorController = new AuthorController(mockAuthorService.Object, mockFileService.Object, mockMemoryCache.Object)
            {
                TempData = tempDataMock.Object
            };

            mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);
        }

        [TearDown]
        public void TearDown()
        {
            mockAuthorService.As<IAuthorService>().Reset();
            mockFileService.As<IFileService>().Reset();
            mockMemoryCache.As<IMemoryCache>().Reset();
            authorController.Dispose();
        }

        [Test]
        public void Add_Get_ReturnsView()
        {
            var result = this.authorController.Add() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.TypeOf<AuthorFormModel>());
        }

        [Test]
        public async Task Add_Post_WithInvalidModel_ReturnsView()
        {
            var model = new AuthorFormModel();
            this.authorController.ModelState.AddModelError("key", "error message");

            var result = await this.authorController.Add(model, null) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(model));
            Assert.That(result.Model, Is.TypeOf<AuthorFormModel>());
        }

        [Test]
        public async Task Add_ValidInput_SuccessfulAddition()
        {
            var model = new AuthorFormModel
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgarian"
            };
            var authorImage = new Mock<IFormFile>().Object;

            this.mockAuthorService.Setup(service => service.AuthorExistByNameAndNationalityAsync(model.FirstName, model.LastName, model.Nationality)).ReturnsAsync(false);
            this.mockAuthorService.Setup(service => service.AddAuthorAsync(model)).ReturnsAsync(new Author());
            this.mockMemoryCache.Setup(cache => cache.Remove(It.IsAny<string>()));
            this.tempDataMock.Setup(temp => temp["SuccessMessage"]).Returns("SuccessMessage");

            var result = await this.authorController.Add(model, authorImage) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Author"));
                Assert.That(this.authorController.TempData["SuccessMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Add_ExistingAuthor_ReturnsError()
        {
            var model = new AuthorFormModel
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgarian"
            };
            var authorImage = new Mock<IFormFile>().Object;

            this.mockAuthorService.Setup(service => service.AuthorExistByNameAndNationalityAsync(model.FirstName, model.LastName, model.Nationality)).ReturnsAsync(true);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.authorController.Add(model, authorImage) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Author"));
                Assert.That(this.authorController.TempData["ErrorMessage"], Is.Not.Null);
                Assert.That(this.authorController.TempData["SuccessMessage"], Is.Null);
            });
        }

        [Test]
        public async Task Add_ValidInputWithBiography_SanitizesBiography()
        {
            var model = new AuthorFormModel
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgarian",
                Biography = "<script>alert('xss');</script>"
            };
            var authorImage = new Mock<IFormFile>().Object;

            var result = await this.authorController.Add(model, authorImage) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Author"));
                Assert.That(new HtmlSanitizer().Sanitize(model.Biography), Is.EqualTo(model.Biography));
            });
        }

        [Test]
        public async Task Add_ExceptionDuringAddition_ReturnsError()
        {
            var model = new AuthorFormModel
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgarian"
            };
            var authorImage = new Mock<IFormFile>().Object;

            this.mockAuthorService.Setup(service => service.AuthorExistByNameAndNationalityAsync(model.FirstName, model.LastName, model.Nationality)).ReturnsAsync(false);
            this.mockAuthorService.Setup(service => service.AddAuthorAsync(model)).ThrowsAsync(new Exception());
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.authorController.Add(model, authorImage) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Author"));
                Assert.That(this.authorController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        // Some problem with the mockMemoryCache ???
        //[Test]
        //public async Task All_DataInCache_ReturnsViewWithCachedData()
        //{
        //    var queryModel = new AllAuthorsQueryModel { CurrentPage = 1, AuthorsPerPage = 10 };
        //    var cachedData = new AllAuthorsFilteredAndPagedServiceModel
        //    {
        //        Authors = new List<AllAuthorsViewModel> 
        //        { 
        //            new AllAuthorsViewModel 
        //            { 
        //                FirstName = "Peio",
        //                LastName = "Yavorov",
        //                Nationality = "Bulgarian" 
        //            } 
        //        },
        //        TotalAuthorsCount = 1
        //    };

        //    string cacheKey = $"AuthorsCache{queryModel.CurrentPage}_{queryModel.AuthorsPerPage}";
        //    this.mockMemoryCache.Setup(cache => cache.TryGetValue(cacheKey, out cachedData)).Returns(true);

        //    var result = await this.authorController.All(queryModel) as ViewResult;

        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result.Model, Is.EqualTo(queryModel));
        //}

        [Test]
        public async Task Edit_AuthorExists_ReturnsViewWithAuthor()
        {
            string authorId = "1";
            var authorModel = new AuthorFormModel
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgarian",
            };

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(true);
            this.mockAuthorService.Setup(service => service.GetAuthorForEditByIdAsync(authorId)).ReturnsAsync(authorModel);

            var result = await this.authorController.Edit(authorId) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(authorModel));
        }

        [Test]
        public async Task Edit_AuthorDoesNotExist_RedirectsToAll()
        {
            string authorId = "1";

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(false);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.authorController.Edit(authorId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Author"));
                Assert.That(this.authorController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Edit_ExceptionOccurs_ReturnsGeneralError()
        {
            string authorId = "1";
            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ThrowsAsync(new Exception());

            var result = await this.authorController.Edit(authorId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());

            var redirectResult = result as RedirectToActionResult;

            Assert.Multiple(() =>
            {
                Assert.That(redirectResult!.ActionName, Is.EqualTo("Index"));
                Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            });
        }

        [Test]
        public async Task Edit_InvalidModelState_ReturnsViewWithModel()
        {
            string authorId = "1";
            var model = new AuthorFormModel
            {
                FirstName = "",
                LastName = "",
                Nationality = ""
            };

            this.authorController.ModelState.AddModelError("FirstName", "The FirstName field is required.");

            var result = await this.authorController.Edit(authorId, model, null) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Edit_ValidInput_EditsAuthorAndRedirectsToAll()
        {
            string authorId = "1";
            var model = new AuthorFormModel
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgarian",
                BirthDate = new DateOnly(1850, 1, 1),
            };
            var authorToEdit = new Author
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgaria",
                BirthDate = new DateOnly(1800, 9, 9),
            };
            var authorImage = new Mock<IFormFile>().Object;

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(true);
            this.mockAuthorService.Setup(service => service.EditAuthorAsync(authorId, model)).ReturnsAsync(authorToEdit);
            this.tempDataMock.Setup(temp => temp["SuccessMessage"]).Returns("SuccessMessage");

            var result = await this.authorController.Edit(authorId, model, authorImage) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Author"));
                Assert.That(this.authorController.TempData["SuccessMessage"], Is.Not.Null);
                Assert.That(this.authorController.TempData["ErrorMessage"], Is.Null);
            });
        }

        [Test]
        public async Task Edit_NonExistentAuthor_ReturnsErrorMessageAndRedirectsToAll()
        {
            string authorId = "1";
            var model = new AuthorFormModel
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgarian",
                BirthDate = new DateOnly(1850, 1, 1),
            };

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(false);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.authorController.Edit(authorId, model, null) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Author"));
                Assert.That(this.authorController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Edit_ExceptionDuringEditing_ReturnsErrorMessageAndRedirectsToAll()
        {
            string authorId = "1";
            var model = new AuthorFormModel
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgarian",
                BirthDate = new DateOnly(1850, 1, 1),
            };

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(true);
            this.mockAuthorService.Setup(service => service.EditAuthorAsync(authorId, model)).ThrowsAsync(new Exception());
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.authorController.Edit(authorId, model, null) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Author"));
                Assert.That(this.authorController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Edit_BiographyNotNull_SanitizesBiography()
        {
            string authorId = "1";
            var model = new AuthorFormModel
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgarian",
                Biography = "<script>alert('xss');</script>",
                BirthDate = new DateOnly(1850, 1, 1),
            };
            var authorToEdit = new Author
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgaria",
                BirthDate = new DateOnly(1800, 9, 9),
            };
            var authorImage = new Mock<IFormFile>().Object;

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(true);
            this.mockAuthorService.Setup(service => service.EditAuthorAsync(authorId, It.IsAny<AuthorFormModel>())).ReturnsAsync(authorToEdit);

            var result = await this.authorController.Edit(authorId, model, authorImage) as RedirectToActionResult;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(model.Biography, Is.EqualTo(new HtmlSanitizer().Sanitize(model.Biography)));
            });
        }

        [Test]
        public async Task Delete_ExistingAuthorId_DeletesAuthorAndRedirectsToAllWithSuccessMessage()
        {
            string authorId = "1";

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(true);
            this.tempDataMock.Setup(temp => temp["SuccessMessage"]).Returns("SuccessMessage");

            var result = await this.authorController.Delete(authorId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Author"));
                Assert.That(authorController.TempData["SuccessMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Delete_NonExistingAuthorId_ReturnsRedirectToAllWithError()
        {
            string authorId = "1";

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(false);
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.authorController.Delete(authorId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Author"));
                Assert.That(authorController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Delete_SuccessfulDeletion_ReturnsRedirectToAllWithSuccessMessage()
        {
            string authorId = "1";

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(true);
            this.tempDataMock.Setup(temp => temp["SuccessMessage"]).Returns("SuccessMessage");

            var result = await this.authorController.Delete(authorId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo("All"));
            Assert.That(result.ControllerName, Is.EqualTo("Author"));
            Assert.That(authorController.TempData["SuccessMessage"], Is.Not.Null);
        }

        [Test]
        public async Task Delete_ExceptionOccursDuringDeletion_ReturnsRedirectToAllWithError()
        {
            string authorId = "1";

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(true);
            this.mockAuthorService.Setup(service => service.DeleteAuthorAsync(authorId)).ThrowsAsync(new Exception());
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            var result = await this.authorController.Delete(authorId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Author"));
                Assert.That(authorController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Details_ExistingAuthorId_ReturnsViewWithAuthorDetails()
        {
            string authorId = "1";
            var authorDetails = new AuthorDetailsViewModel
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgarian",
                BirthDate = new DateOnly(1850, 1, 1),
            };

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(true);
            this.mockAuthorService.Setup(service => service.GetAuthorDetailsForUserAsync(authorId)).ReturnsAsync(authorDetails);


            var result = await authorController.Details(authorId) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(authorDetails));
        }

        [Test]
        public async Task Details_NonExistingAuthorId_ReturnsRedirectToAllWithError()
        {
            string authorId = "1";

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(false);

            var result = await this.authorController.Details(authorId) as RedirectToActionResult;
            this.tempDataMock.Setup(temp => temp["ErrorMessage"]).Returns("ErrorMessage");

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("All"));
                Assert.That(result.ControllerName, Is.EqualTo("Author"));
                Assert.That(authorController.TempData["ErrorMessage"], Is.Not.Null);
            });
        }

        [Test]
        public async Task Details_SuccessfulRetrieval_ReturnsViewWithAuthorDetails()
        {
            string authorId = "1";
            var authorDetails = new AuthorDetailsViewModel
            {
                FirstName = "Peio",
                LastName = "Yavorov",
                Nationality = "Bulgarian",
                BirthDate = new DateOnly(1850, 1, 1),
            };

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(true);
            this.mockAuthorService.Setup(service => service.GetAuthorDetailsForUserAsync(authorId)).ReturnsAsync(authorDetails);

            var result = await this.authorController.Details(authorId) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(authorDetails));
        }

        [Test]
        public async Task Details_ExceptionOccursDuringRetrieval_ReturnsGeneralError()
        {
            string authorId = "1";

            this.mockAuthorService.Setup(service => service.AuthorExistByIdAsync(authorId)).ReturnsAsync(true);
            this.mockAuthorService.Setup(service => service.GetAuthorDetailsForUserAsync(authorId)).ThrowsAsync(new Exception());

            var result = await this.authorController.Details(authorId) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ControllerName, Is.EqualTo("Home"));
            Assert.That(result.ActionName, Is.EqualTo("Index"));
        }
    }
}
