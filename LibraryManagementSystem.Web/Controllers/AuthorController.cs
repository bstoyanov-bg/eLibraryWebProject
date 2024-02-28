using LibraryManagementSystem.Services.Data;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Author;
using LibraryManagementSystem.Web.ViewModels.Category;
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

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public IActionResult Add()
        {
            AuthorFormModel category = new AuthorFormModel();

            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add(AuthorFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await authorService.AddAuthorAsync(model);
                TempData[SuccessMessage] = "Succestully added author";
            }
            catch
            {
                //TempData[ErrorMessage] = "";
            }

            return this.RedirectToAction("All", "Author");
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> All()
        {
            IEnumerable<AllAuthorsViewModel> viewModel =
                await authorService.GetAllAuthorsAsync();

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id)
        {
            var author = await authorService.GetAuthorForEditByIdAsync(id);

            if (author == null)
            {
                return RedirectToAction("All", "Author");
            }

            return View(author);
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(string id, AuthorFormModel model)
        {
            var author = await authorService.GetAuthorForEditByIdAsync(id);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            try
            {
                author.FirstName = model.FirstName;
                author.LastName = model.LastName;
                author.Biography = model.Biography;
                author.BirthDate = model.BirthDate;
                author.DeathDate = model.DeathDate;
                author.Nationality = model.Nationality;

                await authorService.EditAuthorAsync(id, model);
                TempData[SuccessMessage] = "Succesfully edited author";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with editing the author!";
            }

            return this.RedirectToAction("All", "Author");
        }
    }
}
