using LibraryManagementSystem.Services.Data.Interfaces;
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

        public BookController(IBookService bookService) 
        {
            this.bookService = bookService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            IEnumerable<AllBooksViewModel> viewModel = await bookService.GetAllBooksAsync();

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add()
        {
            try
            {
                BookFormModel model = await bookService.GetNewCreateBookModelAsync();

                return View(model);
            }
            catch
            {
                return GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add(BookFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await bookService.AddBookAsync(model);
                TempData[SuccessMessage] = "Succestully added book";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with adding the book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var book = await bookService.GetBookForEditByIdAsync(id);

                if (book == null)
                {
                    return RedirectToAction("All", "Book");
                }

                return View(book);
            }
            catch
            {
                return GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id, BookFormModel model)
        {
            try
            {
                var book = await bookService.GetBookForEditByIdAsync(id);

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (book == null)
                {
                    throw new ArgumentNullException(nameof(book));
                }

                book.Title = model.Title;
                book.ISBN = model.ISBN;
                book.YearPublished = model.YearPublished;
                book.Description = model.Description;
                book.Publisher = model.Publisher;
                book.CoverImagePathUrl = model.CoverImagePathUrl;
                book.AuthorId = model.AuthorId;
                book.CategoryId = model.CategoryId;

                await bookService.EditBookAsync(id, model);
                TempData[SuccessMessage] = "Succesfully edited book";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with editing the book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return RedirectToAction("Index", "Home");
        }
    }
}

