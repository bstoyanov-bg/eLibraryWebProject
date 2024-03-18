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

        public AuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        // ready
        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public IActionResult Add()
        {
            AuthorFormModel author = new AuthorFormModel();

            return View(author);
        }

        // ready
        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add(AuthorFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                bool doesAuthorExist = await this.authorService.AuthorExistByNameAndNationalityAsync(model.FirstName, model.LastName, model.Nationality);

                if (doesAuthorExist)
                {
                    TempData[ErrorMessage] = "Author with the same Name and Nationality already exists!";

                    return this.RedirectToAction("All", "Author");
                }

                await authorService.AddAuthorAsync(model);

                TempData[SuccessMessage] = "Successfully added author.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with adding the author!";
            }

            return this.RedirectToAction("All", "Author");
        }

        // NOT USED ANYMORE
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> All()
        //{
        //    IEnumerable<AllAuthorsViewModel> viewModel = await authorService.GetAllAuthorsAsync();

        //    return View(viewModel);
        //}

        // ready
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllAuthorsQueryModel queryModel)
        {
            AllAuthorsFilteredAndPagedServiceModel serviceModel = await this.authorService.GetAllAuthorsFilteredAndPagedAsync(queryModel);

            queryModel.Authors = serviceModel.Authors;
            queryModel.TotalAuthors = serviceModel.TotalAuthorsCount;

            return this.View(queryModel);
        }

        // ready
        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                bool authorExists = await this.authorService.AuthorExistByIdAsync(id);

                if (!authorExists)
                {
                    TempData[ErrorMessage] = "Such author does not exists!";

                    return RedirectToAction("All", "Author");
                }

                var author = await authorService.GetAuthorForEditByIdAsync(id);

                return View(author);
            }
            catch
            {
                return GeneralError();
            }
        }

        // ready
        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id, AuthorFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool authorExists = await this.authorService.AuthorExistByIdAsync(id);

            if (!authorExists)
            {
                TempData[ErrorMessage] = "Such author does not exists!";

                return RedirectToAction("All", "Author");
            }

            try
            {
                await authorService.EditAuthorAsync(id, model);

                TempData[SuccessMessage] = "Succesfully edited author.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with editing the author!";
            }

            return this.RedirectToAction("All", "Author");
        }

        // ready
        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Delete(string id)
        {
            bool authorExists = await this.authorService.AuthorExistByIdAsync(id);

            if (!authorExists)
            {
                TempData[ErrorMessage] = "Such author does not exists!";

                return RedirectToAction("All", "Author");
            }

            try
            {
                await authorService.DeleteAuthorAsync(id);
                TempData[SuccessMessage] = "Succesfully deleted author.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with deleting the author!";
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
                TempData[ErrorMessage] = "Such author does not exists!";

                return RedirectToAction("All", "Author");
            }

            try
            {
                AuthorDetailsViewModel? author = await authorService.GetAuthorDetailsForUserAsync(id);

                return View(author);
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
