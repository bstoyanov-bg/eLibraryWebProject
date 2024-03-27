using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Web.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Common.NotificationMessageConstants;

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
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Country = model.Country,
                City = model.City,
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            await this.userManager.SetEmailAsync(user, model.Email);
            await this.userManager.SetUserNameAsync(user, model.Email);

            IdentityResult result = await this.userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.View(model);
            }

            await this.signInManager.SignInAsync(user, isPersistent: false);

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            LoginFormModel model = new LoginFormModel()
            {
                ReturnUrl = returnUrl,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                this.TempData[ErrorMessage] = "There was an error while logging you in!";
                return this.View(model);
            }

            var user = await this.userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                // Update Security Stamp upon successful login
                await this.userManager.UpdateSecurityStampAsync(user);
            }
            else
            {
                this.TempData[ErrorMessage] = "There was an error while logging you in!";
            }

            return Redirect(model.ReturnUrl ?? "/Home/Index");
        }

        [HttpPost]
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
                this.TempData[ErrorMessage] = "There was an error while logging you out!";
            }

            return this.RedirectToAction("Index", "Home");
        }
    }
}
