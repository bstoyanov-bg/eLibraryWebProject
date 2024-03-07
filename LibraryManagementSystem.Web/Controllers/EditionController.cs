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

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return RedirectToAction("Index", "Home");
        }
    }
}
