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

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add()
        {
            try
            {
                EditionFormModel model = await this.editionService.GetCreateNewEditionModelAsync();

                return this.View(model);
            }
            catch
            {
                return this.GeneralError();
            }
        }

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
                bool bookExists = await this.bookService.BookExistByIdAsync(model.BookId);

                if (!bookExists)
                {
                    this.TempData[ErrorMessage] = "Such book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                bool editionExists = await this.editionService.EditionExistByVersionPublisherAndBookIdAsync(model.Version, model.Publisher, model.BookId);

                if (editionExists)
                {
                    this.TempData[ErrorMessage] = "Edition with the same Book, Version and Publisher already exists!";

                    return this.RedirectToAction("Details", "Book", new { id = model.BookId });
                }

                await this.editionService.AddEditionAsync(model);

                this.TempData[SuccessMessage] = $"Successfully added edition to the Book.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with adding the edition to the book!";
            }

            return this.RedirectToAction("Details", "Book", new { id = model.BookId });
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                bool editionExists = await this.editionService.EditionExistByIdAsync(id);

                if (!editionExists)
                {
                    this.TempData[ErrorMessage] = "Such Book-Edition does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                var edition = await this.editionService.GetEditionForEditByIdAsync(id);

                return this.View(edition);
            }
            catch
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id, EditionFormModel model)
        {
            var bookId = await this.editionService.GetBookIdByEditionIdAsync(id);

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                bool bookExists = await this.bookService.BookExistByIdAsync(model.BookId);

                if (!bookExists)
                {
                    this.TempData[ErrorMessage] = "Such book does not exists!";

                    return this.RedirectToAction("All", "Book");
                }

                var editionExists = await this.editionService.EditionExistByIdAsync(id);

                if (!editionExists)
                {
                    this.TempData[ErrorMessage] = "There is no edition with such id!";

                    return this.RedirectToAction("All", "Book");
                }

                await this.editionService.EditBookEditionAsync(id, model);

                this.TempData[SuccessMessage] = "Succesfully edited Book-Edition";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with editing the Book-Edition!";
            }

            return this.RedirectToAction("Details", "Book", new { id = bookId });
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Delete(string id)
        {
            var bookId = await this.editionService.GetBookIdByEditionIdAsync(id);
                
            try
            {
                await this.editionService.DeleteEditionAsync(id);

                this.TempData[SuccessMessage] = "Succesfully deleted Book-Edition";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with deleting the Book-Edition!";
            }

            return this.RedirectToAction("Details", "Book", new { id = bookId });
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] = "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
