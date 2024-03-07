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

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return RedirectToAction("Index", "Home");
        }
    }
}
