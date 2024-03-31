using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Services.Data.Models.Book;
using LibraryManagementSystem.Web.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.NotificationMessageConstants;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Web.Controllers
{
    public class BookController : BaseController
    {
        private readonly IBookService bookService;
        private readonly ICategoryService categoryService;
        private readonly IFileService fileService;

        public BookController(IBookService bookService, ICategoryService categoryService, IFileService fileService)
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
            this.fileService = fileService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AllBooksQueryModel queryModel)
        {
            AllBooksFilteredAndPagedServiceModel serviceModel = await this.bookService.GetAllBooksFilteredAndPagedAsync(queryModel);

            queryModel.Books = serviceModel.Books;
            queryModel.TotalBooks = serviceModel.TotalBooksCount;
            queryModel.Categories = await this.categoryService.GetAllCategoriesNamesAsync();

            return this.View(queryModel);
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add()
        {
            try
            {
                BookFormModel model = await bookService.GetCreateNewBookModelAsync();

                return this.View(model);
            }
            catch
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add(BookFormModel model, IFormFile bookImage)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                bool bookExists = await this.bookService.BookExistByTitleAndAuthorIdAsync(model.Title, model.AuthorId);

                if (bookExists)
                {
                    this.TempData[ErrorMessage] = "Book with the same Title and Author already exists!";

                    return this.RedirectToAction("All", "Book");
                }

                var addedBook = await this.bookService.AddBookAsync(model);

                // Here should be executed BOOK IMAGE UPLOAD

                await this.fileService.UploadFileAsync(addedBook.Id.ToString(), bookImage, "BookImage");




                this.TempData[SuccessMessage] = $"Successfully added Book.";

                return this.RedirectToAction("Edit", "Book", new { id = addedBook.Id });
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with adding the Book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                bool bookExists = await this.bookService.BookExistByIdAsync(id);

                if (!bookExists)
                {
                    this.TempData[ErrorMessage] = "Such Book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                BookFormModel? book = await this.bookService.GetBookForEditByIdAsync(id);

                return this.View(book);
            }
            catch
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id, BookFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                bool bookExists = await this.bookService.BookExistByIdAsync(id);

                if (!bookExists)
                {
                    this.TempData[ErrorMessage] = "Such Book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                await this.bookService.EditBookAsync(id, model);

                this.TempData[SuccessMessage] = "Succesfully edited Book.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with editing the Book!";
            }

            return this.RedirectToAction("Details", "Book", new { id });
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Delete(string id)
        {
            bool bookExists = await this.bookService.BookExistByIdAsync(id);

            if (!bookExists)
            {
                this.TempData[ErrorMessage] = "Such Book does not exists!";

                return this.RedirectToAction("All", "Book");
            }

            try
            {
                await this.bookService.DeleteBookAsync(id);

                this.TempData[SuccessMessage] = "Succesfully deleted Book.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with deleting the book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Details(string id)
        {
            bool bookExists = await this.bookService.BookExistByIdAsync(id);

            if (!bookExists)
            {
                this.TempData[ErrorMessage] = "Such Book does not exists!";

                return this.RedirectToAction("All", "Book");
            }

            try
            {
                BookDetailsViewModel book = await this.bookService.GetBookDetailsForUserAsync(id);

                return this.View(book);
            }
            catch
            {
                return this.GeneralError();
            }
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] = "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
