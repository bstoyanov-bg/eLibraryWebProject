using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.UserRoleNames;
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
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Upload(string id, IFormFile file, string entityType)
        {
            try
            {
                // Check file Length
                if (file == null || file.Length == 0)
                {
                    TempData[ErrorMessage] = "No file uploaded!";

                    return this.RedirectToAction("All", "Book", new { id });
                }

                // Check File extension
                if (!file.FileName.EndsWith(".txt"))
                {
                    TempData[ErrorMessage] = "Only .txt files are allowed.!";

                    return this.RedirectToAction("All", "Book", new { id });
                }

                // Check if entityType is valid
                if (entityType != nameof(Book) && entityType != nameof(Edition))
                {
                    TempData[ErrorMessage] = "Invalid entity type.!";

                    return this.RedirectToAction("All", "Book", new {id});
                }

                var filePath = await fileService.UploadFile(id, file, entityType);

                TempData[SuccessMessage] = $"Successfully uploaded {entityType} file.";

                return RedirectToAction("All", "Book");
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
