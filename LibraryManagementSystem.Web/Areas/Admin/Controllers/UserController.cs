using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.Areas.Admin.Services.Interfaces;
using LibraryManagementSystem.Web.Areas.Admin.ViewModels;
using LibraryManagementSystem.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static LibraryManagementSystem.Common.NotificationMessageConstants;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Web.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        private const int UsersPerPage = 2;

        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client, NoStore = false)]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<UserViewModel> users = await this.userService.GetAllUsersAsync();

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
            {
            try
            {
                ApplicationUser userToDelete = await this.userService.GetUserByIdAsync(id);

                if (userToDelete == null)
                {
                    this.TempData[ErrorMessage] = "There is no User with such Id!";
    
                    return this.RedirectToAction("Index", "Home", new { area = "" });
                }

                if (!this.User.IsInRole(AdminRole))
                {
                    this.TempData[ErrorMessage] = "You are not an Administrator!";

                    return this.RedirectToAction("Index", "Home", new { area = "" });
                }

                if (User.GetId() == userToDelete.Id.ToString())
                {
                    this.TempData[ErrorMessage] = "You cannot delete yourself!";

                    return this.RedirectToAction("All", "User", new { area = "Admin" });
                }

                await this.userService.DeleteUserAsync(id);


                this.TempData[SuccessMessage] = "You have succesfully deleted the User!";

                return this.RedirectToAction("All", "User", new { area = "Admin" });
            }
            catch
            {
                this.TempData[ErrorMessage] = "There was problem with deleting the User!";

            }

            return this.RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
