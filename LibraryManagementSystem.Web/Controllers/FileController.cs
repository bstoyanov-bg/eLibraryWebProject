﻿using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.NotificationMessageConstants;
using static LibraryManagementSystem.Common.UserRoleNames;

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
        public async Task<IActionResult> UploadFile(string id, IFormFile file, string entityType)
        {
            try
            {
                // Check file Length
                if (file == null || file.Length == 0)
                {
                    this.TempData[ErrorMessage] = "No file is chosen for upload!";

                    return this.RedirectToAction("Edit", "Book", new { id });
                }

                // Check File extension
                if (!file.FileName.EndsWith(".txt"))
                {
                    this.TempData[ErrorMessage] = "Only .txt files are allowed!";

                    return this.RedirectToAction("Edit", "Book", new { id });
                }

                // Check if entityType is valid
                if (entityType != nameof(Book) && entityType != nameof(Edition))
                {
                    this.TempData[ErrorMessage] = "Invalid entity type!";

                    return this.RedirectToAction("Edit", "Book", new {id});
                }

                string filePath = await this.fileService.UploadFileAsync(id, file, entityType);

                this.TempData[SuccessMessage] = $"Successfully uploaded {entityType} file.";

                return this.RedirectToAction("All", "Book");
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with uploding the file!";
            }

            return this.RedirectToAction("All", "Book");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(string id, string entityType)
        {
            try
            {
                // Check if entityType is valid
                if (entityType != nameof(Book) && entityType != nameof(Edition))
                {
                    this.TempData[ErrorMessage] = "Invalid entity type.!";

                    return this.RedirectToAction("All", "Book", new { id });
                }

                FileStreamResult fileStreamResult = await this.fileService.DownloadFileAsync(id, entityType);
                return fileStreamResult;
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with downloading the file!";
            }

            return this.RedirectToAction("All", "Book", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> GetTextFileContent(string id, string entityType)
        {
            try
            {
                string filePath = await this.fileService.GetFilePathAsync(id, entityType);

                string fileContent = await this.fileService.GetFileContentAsync(filePath);

                return this.Content(fileContent);
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with getting the text file!";
            }

            return this.RedirectToAction("All", "Book", new { id });
        }
    }
}
