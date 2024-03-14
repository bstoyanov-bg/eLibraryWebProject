using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.NotificationMessageConstants;

namespace LibraryManagementSystem.Web.Controllers
{
    public class FileController : BaseController
    {
        private readonly IFileService fileService;

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(string id, IFormFile file, string entityType)
        {
            try
            {
                // Check if entityType is valid
                if (entityType != nameof(Book) && entityType != nameof(Edition))
                {
                    return BadRequest("Invalid entity type.");
                }

                Type type = entityType == nameof(Book) ? typeof(Book) : typeof(Edition);

                var filePath = await fileService.UploadFile(id, file, entityType);
                TempData[SuccessMessage] = $"Successfully uploaded {entityType} file";
                return RedirectToAction("Index", "Home");
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
