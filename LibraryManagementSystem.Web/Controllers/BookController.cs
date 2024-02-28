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
            IEnumerable<AllBooksViewModel> viewModel =
                await bookService.GetAllBooksAsync();

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add()
        {
            BookFormModel model = await bookService.GetNewCreateBookModelAsync();

            return View(model);
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
                //TempData[ErrorMessage] = "";
            }

            return this.RedirectToAction("All", "Book");
        }
    }
}



