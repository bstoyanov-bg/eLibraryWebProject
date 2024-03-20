using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Rating;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.NotificationMessageConstants;

namespace LibraryManagementSystem.Web.Controllers
{
    public class RatingController : BaseController
    {
        private readonly IRatingService ratingService;
        private readonly IBookService bookService;

        public RatingController(IRatingService ratingService, IBookService bookService)
        {
            this.ratingService = ratingService;
            this.bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> Give(string id)
        {
            try
            {
                bool bookExists = await this.bookService.BookExistByIdAsync(id);

                if (!bookExists)
                {
                    TempData[ErrorMessage] = "Such Book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                var book = await this.bookService.GetBookByIdAsync(id);

                ViewBag.BookTitle = book!.Title;
                ViewBag.BookImage = book!.CoverImagePathUrl;
            }
            catch
            {
                GeneralError();
            }

            RatingFormModel model = new RatingFormModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Give(RatingFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                bool bookExists = await this.bookService.BookExistByIdAsync(model.BookId);

                if (bookExists)
                {
                    TempData[ErrorMessage] = "Such Book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                var book = await this.bookService.GetBookByIdAsync(model.BookId);

                ViewBag.BookTitle = book!.Title;

                var userId = GetUserId();

                bool userRatedBook = await this.bookService.HasUserRatedBookAsync(userId);

                if (userRatedBook)
                {
                    TempData[ErrorMessage] = "You have already Rated the Book!";
                }

                await this.ratingService.GiveRatingAsync(model);

                TempData[SuccessMessage] = "Successfully gave rating to the Book.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with giving rating to the Book!";
            }

            return this.RedirectToAction("Details", "Book", new { id = model.BookId });
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return RedirectToAction("Index", "Home");
        }
    }
}
