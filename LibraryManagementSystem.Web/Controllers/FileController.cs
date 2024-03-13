using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
