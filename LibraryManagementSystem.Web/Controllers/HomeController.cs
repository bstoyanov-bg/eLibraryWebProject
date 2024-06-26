using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService bookService;

        public HomeController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (this.User.IsInRole(AdminRole))
            {
                return this.RedirectToAction("Index", "Home", new { Area = AdminAreaName});
            }

            IEnumerable<IndexViewModel> viewModel = await this.bookService.LastNineBooksAsync();

            return this.View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 400 || statusCode == 404)
            {
                return this.View("Error404");
            }
            else if (statusCode == 401)
            {
                return this.View("Error401");
            }
            else if (statusCode == 403)
            {
                return this.View("Error403");
            }
            else if (statusCode == 500)
            {
                return this.View("Error500");
            }

            return this.View();
        }
    }
}
