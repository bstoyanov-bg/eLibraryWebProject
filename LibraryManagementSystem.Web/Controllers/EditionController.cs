using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Edition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.NotificationMessageConstants;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Web.Controllers
{
    public class EditionController : BaseController
    {
        private readonly IEditionService editionService;
        private readonly IBookService bookService;

        public EditionController(IEditionService editionService, IBookService bookService)
        {
            this.editionService = editionService;
            this.bookService = bookService;
        }

        // ready
        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add()
        {
            try
            {
                EditionFormModel model = await editionService.GetCreateNewEditionModelAsync();

                return View(model);
            }
            catch
            {
                return GeneralError();
            }
        }

        // ready
        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add(EditionFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                bool bookExists = await bookService.BookExistByIdAsync(model.BookId);

                if (!bookExists)
                {
                    TempData[ErrorMessage] = "Such book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                bool editionExists = await this.editionService.EditionExistByVersionPublisherAndBookIdAsync(model.Version, model.Publisher, model.BookId);

                if (editionExists)
                {
                    TempData[ErrorMessage] = "Edition with the same Book, Version and Publisher already exists!";

                    return this.RedirectToAction("Details", "Book", new { id = model.BookId });
                }

                await editionService.AddEditionAsync(model);

                TempData[SuccessMessage] = $"Successfully added edition to the Book.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with adding the edition to the book!";
            }

            return this.RedirectToAction("Details", "Book", new { id = model.BookId });
        }

        // ready
        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                bool editionExists = await this.editionService.EditionExistByIdAsync(id);

                if (!editionExists)
                {
                    TempData[ErrorMessage] = "Such Book-Edition does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                var edition = await editionService.GetEditionForEditByIdAsync(id);

                return View(edition);
            }
            catch
            {
                return GeneralError();
            }
        }

        // ready
        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id, EditionFormModel model)
        {
            var bookId = await editionService.GetBookIdByEditionIdAsync(id);

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                bool bookExists = await bookService.BookExistByIdAsync(model.BookId);

                if (!bookExists)
                {
                    TempData[ErrorMessage] = "Such book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                var editionExists = await editionService.EditionExistByIdAsync(id);

                if (!editionExists)
                {
                    TempData[ErrorMessage] = "There is no edition with such id!";

                    return this.RedirectToAction("All", "Book");
                }

                await editionService.EditBookEditionAsync(id, model);

                TempData[SuccessMessage] = "Succesfully edited Book-Edition";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with editing the Book-Edition!";
            }

            return this.RedirectToAction("Details", "Book", new { id = bookId });
        }

        // ready
        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Delete(string id)
        {
            var bookId = await editionService.GetBookIdByEditionIdAsync(id);
                
            try
            {
                await editionService.DeleteEditionAsync(id);
                TempData[SuccessMessage] = "Succesfully deleted Book-Edition";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with deleting the Book-Edition!";
            }

            return this.RedirectToAction("Details", "Book", new { id = bookId });
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return RedirectToAction("Index", "Home");
        }
    }
}
