using Griesoft.AspNetCore.ReCaptcha;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Services.Data.Interfaces;
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
        private readonly IUserService userService;

        public UserController(SignInManager<ApplicationUser> signInManager,
                                UserManager<ApplicationUser> userManager,
                                IUserService userService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateRecaptcha(Action = nameof(Register), ValidationFailedAction = ValidationFailedAction.ContinueRequest)]
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

            this.TempData[SuccessMessage] = "You have registered successfully.";

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
            var user = await this.userManager.FindByNameAsync(model.Username);

            bool isDeleted = await this.userService.IsUserDeletedAsync(user!.Id.ToString());
            if (isDeleted)
            {
                this.TempData[ErrorMessage] = "The account you are trying to login is deleted!";

                return this.RedirectToAction("Index", "Home");
            }

            var result = await this.signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                this.TempData[ErrorMessage] = "There was an error while logging you in!";
                return this.View(model);
            }

            this.TempData[SuccessMessage] = "You have logged in successfully.";
            
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
