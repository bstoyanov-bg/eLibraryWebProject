using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;
using static LibraryManagementSystem.Common.NotificationMessageConstants;
using static LibraryManagementSystem.Common.UserRoleNames;

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

        // ready
        [HttpPost]
        [Authorize(Roles = UserRole)]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                bool bookExists = await this.bookService.BookExistByIdAsync(id);

                if (!bookExists)
                {
                    TempData[ErrorMessage] = "Such Book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                var userId = GetUserId();

                var bookExistsInCollection = await this.lendedBooksService.IsBookActiveInUserCollectionAsync(userId, id);

                // I remove the button from UI if the book is added to collection. Double ckeck!
                if (bookExistsInCollection == true)
                {
                    TempData[WarningMessage] = "Book is already added to user collection!";
                }

                var userActiveBooks = await this.lendedBooksService.GetCountOfActiveBooksForUserAsync(userId);

                if (userActiveBooks >= MaxNumberOfBooksAllowed)
                {
                    TempData[WarningMessage] = "You have reached the maximum number of book that you can add to your collection!";

                    return RedirectToAction("All", "Book");
                }

                await lendedBooksService.AddBookToCollectionAsync(userId, id);

                TempData[SuccessMessage] = "You have succesfully added book to your collection.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with adding the book to collection!";
            }

            return this.RedirectToAction("Mine", "LendedBooks");
        }

        // ready
        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var model = await lendedBooksService.GetMyBooksAsync(GetUserId());

            return View(model);
        }

        // ready
        [HttpPost]
        public async Task<IActionResult> Return(string id)
        {
            bool bookExists = await this.bookService.BookExistByIdAsync(id);

            if (!bookExists)
            {
                TempData[ErrorMessage] = "Such Book does not exists!";

                return this.RedirectToAction("All", "Book");
            }

            var userId = GetUserId();

            bool isBookReturned = await this.lendedBooksService.IsBookReturnedAsync(userId, id);

            if (!isBookReturned)
            {
                TempData[ErrorMessage] = "The Book is already returned.";

                return this.RedirectToAction("All", "Book");
            }

            try
            {
                await lendedBooksService.ReturnBookAsync(userId, id);

                TempData[SuccessMessage] = "You have succesfully returned Book.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with returning the Book!";
            }

            return RedirectToAction("All", "Book");
        }

        // ready
        [HttpPost]
        public async Task<IActionResult> ReturnAll()
        {
            var userId = GetUserId();

            try
            {
                await lendedBooksService.ReturnAllBooksAsync(userId);

                TempData[SuccessMessage] = "You have succesfully returned all Books.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with returning the Book!";
            }

            return RedirectToAction("All", "Book");
        }
    }
}
