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

            return this.View(category);
        }


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
                    this.TempData[ErrorMessage] = "Category with the same name already exists!";

                    return this.RedirectToAction("All", "Category");
                }

                await this.categoryService.AddCategoryAsync(model);

                this.TempData[SuccessMessage] = "Successfully added category.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with adding the category!";
            }

            return this.RedirectToAction("All", "Category");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            IEnumerable<AllCategoriesViewModel> allcategories = await this.categoryService.GetAllCategoriesAsync();

            return this.View(allcategories);
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                bool categoryExists = await this.categoryService.CategoryExistByIdAsync(id);

                if (!categoryExists)
                {
                    this.TempData[ErrorMessage] = "Category with the provided id does not exist!";

                    return this.RedirectToAction("All", "Category");
                }

                CategoryFormModel? category = await this.categoryService.GetCategoryForEditByIdAsync(id);

                return this.View(category);
            }
            catch
            {
                return this.GeneralError();
            }
        }

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
                this.TempData[ErrorMessage] = "Category with the provided id does not exist!";

                return this.RedirectToAction("All", "Category");
            }

            try
            {
                await this.categoryService.EditCategoryAsync(id, model);

                this.TempData[SuccessMessage] = "Succesfully edited category";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with editing the category!";
            }

            return this.RedirectToAction("All", "Category");
        }

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> Delete(int id)
        {
            bool categoryExists = await this.categoryService.CategoryExistByIdAsync(id);

            if (!categoryExists)
            {
                this.TempData[ErrorMessage] = "Category with the provided id does not exist!";

                return this.RedirectToAction("All", "Category");
            }

            try
            {
                await this.categoryService.DeleteCategoryAsync(id);

                this.TempData[SuccessMessage] = "Succesfully deleted category.";
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with deleting the category!";
            }

            return this.RedirectToAction("All", "Category");
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] = "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
