﻿using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.GeneralApplicationConstants;
using static LibraryManagementSystem.Common.NotificationMessageConstants;
using static LibraryManagementSystem.Common.UserRoleNames;

namespace LibraryManagementSystem.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(SignInManager<ApplicationUser> signInManager,
                                UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Country = model.Country,
                City = model.City,
                DateOfBirth = model.DateOfBirth,
                MaxLoanedBooks = MaxNumberOfBooksAllowed,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            await this.userManager.SetEmailAsync(user, model.Email);
            await this.userManager.SetUserNameAsync(user, model.Username);

            IdentityResult result = await this.userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.View(model);
            }

            await this.userManager.AddToRoleAsync(user, UserRole);
            await this.signInManager.SignInAsync(user, isPersistent: false);

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            LoginFormModel model = new LoginFormModel();

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                this.TempData[ErrorMessage] = "There was an error while logging you in!";
                return this.View(model);
            }

            var user = await this.userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                // Update Security Stamp upon successful login
                await this.userManager.UpdateSecurityStampAsync(user);
            }
            else
            {
                this.TempData[ErrorMessage] = "There was an error while logging you in!";
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            // Update Security Stamp upon logout to invalidate any existing tokens
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                await this.userManager.UpdateSecurityStampAsync(user);
            }
            else
            {
                this.TempData[ErrorMessage] = "There was a problem with the Update Security Stamp!";
            }

            return this.RedirectToAction("Index", "Home");
        }
    }
}
