using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

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
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await categoryService.CreateCategoryAsync(model);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<AllViewModel> viewModel =
                await categoryService.GetAllCategoriesAsync();

            return View(viewModel);
        }
    }
}
