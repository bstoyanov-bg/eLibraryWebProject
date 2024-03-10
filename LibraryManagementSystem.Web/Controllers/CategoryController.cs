using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.NotificationMessageConstants;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Web.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public IActionResult Add()
        {
            CategoryFormModel category = new CategoryFormModel();

            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add(CategoryFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await categoryService.AddCategoryAsync(model);
                TempData[SuccessMessage] = "Succestully created category";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with adding the category!";
            }

            return this.RedirectToAction("All", "Category");
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> All()
        {
            IEnumerable<AllCategoriesViewModel> viewModel = await categoryService.GetAllCategoriesAsync();

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await categoryService.GetCategoryForEditByIdAsync(id);

                if (category == null)
                {
                    return RedirectToAction("All", "Category");
                }

                return View(category);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(int id, CategoryFormModel model)
        {
            var category = await categoryService.GetCategoryForEditByIdAsync(id);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            try
            {
                category.Name = model.Name;
                await categoryService.EditCategoryAsync(id, model);
                TempData[SuccessMessage] = "Succesfully edited category";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with editing the category!";
            }

            return this.RedirectToAction("All", "Category");
        }

        //[HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await categoryService.DeleteCategoryAsync(id);
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with deleting the category!";
            }

            return this.RedirectToAction("All", "Category");
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return RedirectToAction("Index", "Home");
        }
    }
}
