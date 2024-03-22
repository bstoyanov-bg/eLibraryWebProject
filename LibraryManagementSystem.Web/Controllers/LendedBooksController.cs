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

        [HttpPost]
        [Authorize(Roles = UserRole)]
        public async Task<IActionResult> GetBook(string id)
        {
            try
            {
                bool bookExists = await this.bookService.BookExistByIdAsync(id);

                if (!bookExists)
                {
                    this.TempData[ErrorMessage] = "Such Book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                string userId = GetUserId();

                bool bookExistsInCollection = await this.lendedBooksService.IsBookActiveInUserCollectionAsync(userId, id);

                // I remove the button from UI if the book is added to collection. Double ckeck!
                if (bookExistsInCollection == true)
                {
                    this.TempData[WarningMessage] = "Book is already added to user collection!";
                }

                int userActiveBooks = await this.lendedBooksService.GetCountOfActiveBooksForUserAsync(userId);

                if (userActiveBooks >= MaxNumberOfBooksAllowed)
                {
                    this.TempData[WarningMessage] = "You have reached the maximum number of book that you can add to your collection!";

                    return this.RedirectToAction("All", "Book");
                }

                await this.lendedBooksService.AddBookToCollectionAsync(userId, id);

                this.TempData[SuccessMessage] = "You have succesfully added book to your collection.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with adding the book to collection!";
            }

            return this.RedirectToAction("Mine", "LendedBooks");
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var model = await this.lendedBooksService.GetMyBooksAsync(GetUserId());

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ReturnBook(string id)
        {
            bool bookExists = await this.bookService.BookExistByIdAsync(id);

            if (!bookExists)
            {
                this.TempData[ErrorMessage] = "Such Book does not exists!";

                return this.RedirectToAction("All", "Book");
            }

            string userId = GetUserId();

            bool isBookReturned = await this.lendedBooksService.IsBookReturnedAsync(userId, id);

            if (!isBookReturned)
            {
                this.TempData[ErrorMessage] = "The Book is already returned.";

                return this.RedirectToAction("All", "Book");
            }

            try
            {
                await this.lendedBooksService.ReturnBookAsync(userId, id);

                this.TempData[SuccessMessage] = "You have succesfully returned Book.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with returning the Book!";
            }

            return this.RedirectToAction("Mine", "LendedBooks");
        }

        [HttpPost]
        public async Task<IActionResult> ReturnAll()
        {
            string userId = GetUserId();

            try
            {
                await this.lendedBooksService.ReturnAllBooksAsync(userId);

                this.TempData[SuccessMessage] = "You have succesfully returned all Books.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with returning the Books!";
            }

            return this.RedirectToAction("All", "Book");
        }
    }
}
