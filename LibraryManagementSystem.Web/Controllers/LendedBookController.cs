﻿using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;
using static LibraryManagementSystem.Common.NotificationMessageConstants;

namespace LibraryManagementSystem.Web.Controllers
{
    public class LendedBookController : BaseController
    {
        private readonly ILendedBookService lendedBookService;
        private readonly IBookService bookService;

        public LendedBookController(ILendedBookService lendedBookService, IBookService bookService)
        {
            this.lendedBookService = lendedBookService;
            this.bookService = bookService;
        }

        [HttpPost]
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

                bool bookExistsInCollection = await this.lendedBookService.IsBookActiveInUserCollectionAsync(userId, id);

                // I remove the button from UI if the book is added to collection. But Double ckeck!
                if (bookExistsInCollection == true)
                {
                    this.TempData[ErrorMessage] = "Book is already added to user collection!";
                }

                int userActiveBooks = await this.lendedBookService.GetCountOfActiveBooksForUserAsync(userId);

                if (userActiveBooks >= MaxNumberOfBooksAllowed)
                {
                    this.TempData[ErrorMessage] = "You have reached the maximum number of Books that you can add to your collection!";

                    return this.RedirectToAction("All", "Book");
                }

                await this.lendedBookService.AddBookToCollectionAsync(userId, id);

                this.TempData[SuccessMessage] = "You have succesfully added Book to your collection.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with adding the Book to collection!";
            }

            return this.RedirectToAction("Mine", "LendedBook");
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var model = await this.lendedBookService.GetMyBooksAsync(GetUserId());

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

            bool isBookReturned = await this.lendedBookService.IsBookReturnedAsync(userId, id);

            if (!isBookReturned)
            {
                this.TempData[ErrorMessage] = "The Book is already returned.";

                return this.RedirectToAction("All", "Book");
            }

            try
            {
                await this.lendedBookService.ReturnBookAsync(userId, id);

                this.TempData[SuccessMessage] = "You have succesfully returned Book.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with returning the Book!";
            }

            return this.RedirectToAction("Mine", "LendedBook");
        }

        [HttpPost]
        public async Task<IActionResult> ReturnAll()
        {
            string userId = GetUserId();

            try
            {
                await this.lendedBookService.ReturnAllBooksAsync(userId);

                this.TempData[SuccessMessage] = "You have succesfully returned all Books.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with returning the Books!";
            }

            return this.RedirectToAction("Index", "Home");
        }
    }
}
