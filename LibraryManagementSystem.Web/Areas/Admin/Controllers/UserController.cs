using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.Areas.Admin.Services.Interfaces;
using LibraryManagementSystem.Web.Areas.Admin.ViewModels;
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



        //[HttpPost]
        //public async Task<IActionResult> Delete(string userId)
        //{
        //    try
        //    {
        //        ApplicationUser userToDelete = await this.userService.GetUserByIdAsync(userId);

        //        if (userToDelete == null)
        //        {
        //            this.TempData[ErrorMessage] = "There is no user with that Id!";

        //            return this.RedirectToAction("Index", "Home", new { area = "" });
        //        }

        //        if (!this.User.IsInRole(AdminRole))
        //        {
        //            this.TempData[ErrorMessage] = "You are not an administrator!";

        //            return this.RedirectToAction("Index", "Home", new { area = "" });
        //        }

        //        await this.userService.DeleteUserAsync(userId);


        //        this.TempData[SuccessMessage] = "You have succesfully deleted the user!";

        //        //return this.RedirectToAction(nameof(Active));
        //        return this.RedirectToAction("Index", "Home", new { area = "" });
        //    }
        //    catch (Exception ex)
        //    {
        //        this.TempData[ErrorMessage] = ex.Message;

        //        return this.RedirectToAction("Index", "Home", new { area = "" });
        //    }

        //}
    }
}
