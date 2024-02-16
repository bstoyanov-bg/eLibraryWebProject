using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Web.Areas.Admin.Controllers
{
    public class BookController : BaseAdminController
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
