using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static LibraryManagementSystem.Common.UserRoleNames;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;

namespace LibraryManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService bookService;

        public HomeController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            if (this.User.IsInRole(AdminRole))
            {
                return this.RedirectToAction("Index", "Home", new { Area = AdminAreaName });
            }

            IEnumerable<IndexViewModel> viewModel = await this.bookService.LastTenBooksAsync();

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
