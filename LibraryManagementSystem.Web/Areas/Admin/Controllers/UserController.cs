using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Models.User;
using LibraryManagementSystem.Web.Areas.Admin.Services.Interfaces;
using LibraryManagementSystem.Web.Areas.Admin.ViewModels.User;
using LibraryManagementSystem.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.NotificationMessageConstants;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Web.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllUsersQueryModel queryModel)
        {
            AllUsersFilteredAndPagedServiceModel serviceModel = await this.userService.GetAllUsersFilteredAndPagedAsync(queryModel);

            queryModel.Users = serviceModel.Users;
            queryModel.TotalUsers = serviceModel.TotalUsersCount;

            return this.View(queryModel);
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

        [HttpPost]
        public async Task<IActionResult> PromoteAdmin(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Assign the "Admin" role to the user
                await userManager.AddToRoleAsync(user, AdminRole);
                // Remove the user from other role
                await userManager.RemoveFromRoleAsync(user, UserRole);

                this.TempData[SuccessMessage] = "The user successfully became an Administrator!";

                return RedirectToAction("All", "User", new { area = "Admin" });
            }
            else
            {
                this.TempData[ErrorMessage] = "There is no User with such Id!";
            }

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> DemoteAdmin(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Assign the "User" role to the user
                await userManager.AddToRoleAsync(user, UserRole);
                // Remove the user from other role
                await userManager.RemoveFromRoleAsync(user, AdminRole);

                this.TempData[SuccessMessage] = "The user is no more an Administrator!";

                return RedirectToAction("All", "User", new { area = "Admin" });
            }
            else
            {
                this.TempData[ErrorMessage] = "There is no User with such Id!";
            }

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}
