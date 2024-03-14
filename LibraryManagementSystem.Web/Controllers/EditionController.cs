using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Edition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.UserRoleNames;
using static LibraryManagementSystem.Common.NotificationMessageConstants;

namespace LibraryManagementSystem.Web.Controllers
{
    public class EditionController : BaseController
    {
        private readonly IEditionService editionService;

        public EditionController(IEditionService editionService)
        {
            this.editionService = editionService;
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add()
        {
            try
            {
                EditionFormModel model = await editionService.GetNewCreateEditionModelAsync();

                return View(model);
            }
            catch
            {
                return GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add(EditionFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await editionService.AddEditionAsync(model);
                TempData[SuccessMessage] = "Succestully added edition to the book";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with adding the edition to the book!";
            }

            return this.RedirectToAction("All", "Book");
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var edition = await editionService.GetBookEditionForEditByIdAsync(id);

                if (edition == null)
                {
                    return RedirectToAction("All", "Book");
                }

                return View(edition);
            }
            catch
            {
                return GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id, EditionFormModel model)
        {
            var bookId = await editionService.GetBookIdByEditionIdAsync(id);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var bookEdition = await editionService.GetBookEditionForEditByIdAsync(id);

                if (bookEdition == null)
                {
                    TempData[ErrorMessage] = "There is no edition with such id!";
                }

                await editionService.EditBookEditionAsync(id, model);
                TempData[SuccessMessage] = "Succesfully edited book edition";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with editing the book edition!";
            }

            return this.RedirectToAction("Details", "Book", new { id = bookId });
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Delete(string id)
        {
            var bookId = await editionService.GetBookIdByEditionIdAsync(id);

            try
            {
                await editionService.DeleteBookEditionAsync(id);
                TempData[SuccessMessage] = "Succesfully deleted book edition";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with deleting the edition!";
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
