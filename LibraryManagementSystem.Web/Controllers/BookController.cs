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

        public BookController(IBookService bookService, ICategoryService categoryService) 
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
        }

        // ready
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

        // ready
        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add()
        {
            try
            {
                BookFormModel model = await bookService.GetCreateNewBookModelAsync();

                return View(model);
            }
            catch
            {
                return GeneralError();
            }
        }

        // ready
        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add(BookFormModel model)
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
                    TempData[ErrorMessage] = "Book with the same Title and Author already exists!";

                    return this.RedirectToAction("All", "Book");
                }

                await bookService.AddBookAsync(model);

                TempData[SuccessMessage] = $"Successfully added Book.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with adding the Book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        // ready
        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                bool bookExists = await this.bookService.BookExistByIdAsync(id);

                if (!bookExists)
                {
                    TempData[ErrorMessage] = "Such Book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                var book = await bookService.GetBookForEditByIdAsync(id);

                return View(book);
            }
            catch
            {
                return GeneralError();
            }
        }

        // ready
        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id, BookFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                bool bookExists = await this.bookService.BookExistByIdAsync(id);

                if (!bookExists)
                {
                    TempData[ErrorMessage] = "Such Book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                await bookService.EditBookAsync(id, model);

                TempData[SuccessMessage] = "Succesfully edited Book.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with editing the Book!";
            }

            return this.RedirectToAction("Details", "Book", new { id });
        }

        // ready
        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Delete(string id)
        {
            bool bookExists = await this.bookService.BookExistByIdAsync(id);

            if (!bookExists)
            {
                TempData[ErrorMessage] = "Such Book does not exists!";

                return this.RedirectToAction("All", "Book");
            }

            try
            {
                await bookService.DeleteBookAsync(id);

                TempData[SuccessMessage] = "Succesfully deleted Book.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with deleting the book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        // ready
        [HttpGet]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Details(string id)
        {
            bool bookExists = await this.bookService.BookExistByIdAsync(id);

            if (!bookExists)
            {
                TempData[ErrorMessage] = "Such Book does not exists!";

                return RedirectToAction("All", "Book");
            }

            try
            {
                BookDetailsViewModel book = await bookService.GetBookDetailsForUserAsync(id);

                return View(book);
            }
            catch
            {
                return GeneralError();
            }
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return RedirectToAction("Index", "Home");
        }
    }
}
