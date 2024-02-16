using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementSystem.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected string GetUserId()
        {
            string userId = string.Empty;

            if (this.User!.Identity!.IsAuthenticated)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            }

            return userId;
        }
    }
}
