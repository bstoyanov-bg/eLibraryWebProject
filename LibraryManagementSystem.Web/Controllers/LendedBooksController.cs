using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.UserRoleNames;
using static LibraryManagementSystem.Common.NotificationMessageConstants;

namespace LibraryManagementSystem.Web.Controllers
{
    public class LendedBooksController : BaseController
    {
        private readonly ILendedBooksService lendedBooksService;
        private readonly IBookService bookService;

        public LendedBooksController(ILendedBooksService lendedBooksService, IBookService bookService)
        {
            this.lendedBooksService = lendedBooksService;
            this.bookService = bookService;
        }

        [HttpPost]
        [Authorize(Roles = UserRole)]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var book = await bookService.GetBookByIdAsync(id);

                if (book == null)
                {
                    TempData[WarningMessage] = "There is no book with such id!";
                    return RedirectToAction("All", "Book");
                }

                var userId = GetUserId();

                await lendedBooksService.AddBookToCollectionAsync(userId, book);
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with adding the book to collection!";
            }

            return this.RedirectToAction("Mine", "LendedBooks");
        }
    }
}
