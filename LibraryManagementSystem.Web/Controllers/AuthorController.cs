using AngleSharp.Dom.Events;
using Ganss.Xss;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Services.Data.Models.Author;
using LibraryManagementSystem.Web.ViewModels.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.NotificationMessageConstants;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Web.Controllers
{
    public class AuthorController : BaseController
    {
        private readonly IAuthorService authorService;
        private readonly IFileService fileService;

        public AuthorController(IAuthorService authorService, IFileService fileService)
        {
            this.authorService = authorService;
            this.fileService = fileService;
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public IActionResult Add()
        {
            AuthorFormModel author = new AuthorFormModel();

            return this.View(author);
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add(AuthorFormModel model, IFormFile? authorImage)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                bool authorExists = await this.authorService.AuthorExistByNameAndNationalityAsync(model.FirstName, model.LastName, model.Nationality);

                if (authorExists)
                {
                    this.TempData[ErrorMessage] = "Author with the same Name and Nationality already exists!";

                    return this.RedirectToAction("All", "Author");
                }

                if (model.Biography != null)
                {
                    model.Biography = new HtmlSanitizer().Sanitize(model.Biography);
                }

                var addedAuthor = await this.authorService.AddAuthorAsync(model);

                if (authorImage != null) 
                {
                    await this.fileService.UploadImageFileAsync(addedAuthor.Id.ToString(), authorImage, "Author");
                }

                this.TempData[SuccessMessage] = "Successfully added Author.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with adding the Author!";
            }

            return this.RedirectToAction("All", "Author");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllAuthorsQueryModel queryModel)
        {
            AllAuthorsFilteredAndPagedServiceModel serviceModel = await this.authorService.GetAllAuthorsFilteredAndPagedAsync(queryModel);

            queryModel.Authors = serviceModel.Authors;
            queryModel.TotalAuthors = serviceModel.TotalAuthorsCount;

            return this.View(queryModel);
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                bool authorExists = await this.authorService.AuthorExistByIdAsync(id);

                if (!authorExists)
                {
                    this.TempData[ErrorMessage] = "Such Author does not exists!";

                    return RedirectToAction("All", "Author");
                }

                AuthorFormModel? author = await this.authorService.GetAuthorForEditByIdAsync(id);

                return this.View(author);
            }
            catch
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id, AuthorFormModel model, IFormFile? authorImage)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            bool authorExists = await this.authorService.AuthorExistByIdAsync(id);

            if (!authorExists)
            {
                this.TempData[ErrorMessage] = "Such Author does not exists!";

                return this.RedirectToAction("All", "Author");
            }

            if (model.Biography != null)
            {
                model.Biography = new HtmlSanitizer().Sanitize(model.Biography);
            }

            try
            {
                var authorToEdit = await this.authorService.EditAuthorAsync(id, model);

                if (authorImage != null)
                {
                    await this.fileService.UploadImageFileAsync(authorToEdit.Id.ToString(), authorImage, "Author");
                }

                this.TempData[SuccessMessage] = "Succesfully edited Author.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with editing the Author!";
            }

            return this.RedirectToAction("All", "Author");
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Delete(string id)
        {
            bool authorExists = await this.authorService.AuthorExistByIdAsync(id);

            if (!authorExists)
            {
                this.TempData[ErrorMessage] = "Such Author does not exists!";

                return this.RedirectToAction("All", "Author");
            }

            try
            {
                await this.authorService.DeleteAuthorAsync(id);

                this.TempData[SuccessMessage] = "Succesfully deleted Author.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with deleting the Author!";
            }

            return this.RedirectToAction("All", "Author");
        }
    
        [HttpGet]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> Details(string id)
        {
            bool authorExists = await this.authorService.AuthorExistByIdAsync(id);

            if (!authorExists)
            {
                this.TempData[ErrorMessage] = "Such Author does not exists!";

                return this.RedirectToAction("All", "Author");
            }

            try
            {
                AuthorDetailsViewModel author = await this.authorService.GetAuthorDetailsForUserAsync(id);

                return this.View(author);
            }
            catch
            {
                return this.GeneralError();
            }
        }

        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] = "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
