using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.UserRoleNames;
using static LibraryManagementSystem.Common.NotificationMessageConstants;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;

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

                var checkBook = this.lendedBooksService.CkeckIfBookIsAlreadyAddedToUserCollection(userId, book);

                // I remove the button from UI if the book is added to collection, but i will leave this check.
                if (await checkBook == true)
                {
                    TempData[WarningMessage] = "Book is already added to user collection!";
                }

                var userActiveBooks = this.lendedBooksService.GetCountOfActiveBooksForUserAsync(userId);

                if (await userActiveBooks <= MaxNumberOfBooksAllowed)
                {
                    TempData[WarningMessage] = "You have reached the maximum number of book that you can add to your collection!";
                    return RedirectToAction("All", "Book");
                }

                await lendedBooksService.AddBookToCollectionAsync(userId, book);
                TempData[SuccessMessage] = "You have succesfully added book to your collection!";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with adding the book to collection!";
            }

            return this.RedirectToAction("Mine", "LendedBooks");
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var model = await lendedBooksService.GetMyBooksAsync(GetUserId());

            return View(model);
        }
    }
}
