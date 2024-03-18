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

        // ready
        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public IActionResult Add()
        {
            CategoryFormModel category = new CategoryFormModel();

            return View(category);
        }

        // ready
        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Add(CategoryFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                bool doesCategoryExist = await this.categoryService.CategoryExistByNameAsync(model.Name);

                if (doesCategoryExist)
                {
                    TempData[ErrorMessage] = "Category with the same name already exists!";

                    return this.RedirectToAction("All", "Category");
                }

                await categoryService.AddCategoryAsync(model);

                TempData[SuccessMessage] = "Successfully added category.";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with adding the category!";
            }

            return this.RedirectToAction("All", "Category");
        }

        // ready
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            IEnumerable<AllCategoriesViewModel> allcategories = await categoryService.GetAllCategoriesAsync();

            return View(allcategories);
        }

        // ready
        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                bool categoryExists = await this.categoryService.CategoryExistByIdAsync(id);

                if (!categoryExists)
                {
                    TempData[ErrorMessage] = "Category with the provided id does not exist!";

                    return RedirectToAction("All", "Category");
                }

                var category = await categoryService.GetCategoryForEditByIdAsync(id);

                return View(category);
            }
            catch
            {
                return GeneralError();
            }
        }

        // ready
        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(int id, CategoryFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            bool categoryExists = await this.categoryService.CategoryExistByIdAsync(id);

            if (!categoryExists)
            {
                TempData[ErrorMessage] = "Category with the provided id does not exist!";

                return RedirectToAction("All", "Category");
            }

            try
            {
                await categoryService.EditCategoryAsync(id, model);

                TempData[SuccessMessage] = "Succesfully edited category";
            }
            catch
            {
                TempData[ErrorMessage] = "There was problem with editing the category!";
            }

            return this.RedirectToAction("All", "Category");
        }

        // ready
        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Delete(int id)
        {
            bool categoryExists = await this.categoryService.CategoryExistByIdAsync(id);

            if (!categoryExists)
            {
                TempData[ErrorMessage] = "Category with the provided id does not exist!";

                return RedirectToAction("All", "Category");
            }

            try
            {
                await categoryService.DeleteCategoryAsync(id);
                TempData[SuccessMessage] = "Succesfully deleted category.";
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
