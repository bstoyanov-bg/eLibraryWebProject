using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
