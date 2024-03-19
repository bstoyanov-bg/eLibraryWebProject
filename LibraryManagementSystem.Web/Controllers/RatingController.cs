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
        public IActionResult Add()
        {
            RatingFormModel rating = new RatingFormModel();

            return View(rating);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RatingFormModel model)
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

                var userId = GetUserId();

                await authorService.AddAuthorAsync(model);

                TempData[SuccessMessage] = "Successfully added author.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with adding the author!";
            }

            return this.RedirectToAction("All", "Author");
        }
    }
}
