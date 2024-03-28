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
                    this.TempData[ErrorMessage] = "Such Book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                var book = await this.bookService.GetBookByIdAsync(id);

                this.ViewBag.BookTitle = book!.Title;
                this.ViewBag.BookImage = book!.CoverImagePathUrl;
                this.ViewBag.BookId = book!.Id.ToString();

                string userId = GetUserId();

                this.ViewBag.UserId = userId;

                bool userRatedBook = await this.bookService.HasUserRatedBookAsync(userId, book!.Id.ToString());

                if (userRatedBook)
                {
                    this.TempData[ErrorMessage] = "You have already Rated the Book!";

                    return this.RedirectToAction("All", "Book");
                }

                RatingFormModel rating = new RatingFormModel();

                return this.View(rating);
            }
            catch
            {
                return this.GeneralError();
            }
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

                if (!bookExists)
                {
                    this.TempData[ErrorMessage] = "Such Book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                var book = await this.bookService.GetBookByIdAsync(model.BookId);

                await this.ratingService.GiveRatingAsync(model);

                this.TempData[SuccessMessage] = "Successfully rated the Book.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with giving rating to the Book!";
            }

            return this.RedirectToAction("Details", "Book", new { id = model.BookId });
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(string id)
        {
            var comments = await this.ratingService.GetCommentsForBookAsync(id);

            return this.PartialView("_CommentsPartial", comments);
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] = "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
