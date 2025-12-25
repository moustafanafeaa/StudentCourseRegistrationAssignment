using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using StudentCourseRegistrationAssignment.BLL.AuthServices;
using StudentCourseRegistrationAssignment.BLL.ViewModels;
using StudentCourseRegistrationAssignment.DAL.Data.Entites;
using StudentCourseRegistrationAssignment.Web.Resources;
using System.Security.Claims;

namespace StudentCourseRegistrationAssignment.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AccountController(IAuthService authService, UserManager<ApplicationUser> userManager, IStringLocalizer<SharedResource> localizer)
        {
            _authService = authService;
            _userManager = userManager;
            _localizer = localizer;
        }

        [HttpPost]
        public IActionResult ChangeLanguage(string culture, string returnUrl = null)
        {
            Response.Cookies.Append(
                 CookieRequestCultureProvider.DefaultCookieName,
                 CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                 new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
             );

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authService.LoginAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }
            var role = User.Claims
                 .FirstOrDefault(c => c.Type == ClaimTypes.Role)?
                 .Value;

            if (role == "Admin")
                return RedirectToAction("Index", "AdminCourses");


            return RedirectToAction("Index", "Course");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authService.RegisterAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            return View("RegisterConfirmation");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _authService.ConfirmEmailAsync(userId, token);

            if (!result.Success)
                return View("Error");

            return View("EmailConfirmed");
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resetLinkBase = Url.Action(
                "ResetPassword",
                "Account",
                null,
                Request.Scheme
            );

            var result = await _authService.ForgetPasswordAsync(
                model.Email,
                resetLinkBase
            );

            ViewBag.Message = result.Message;
            return View("ForgetPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.ResetPasswordAsync(model);

            if (!result.Success)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error);
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }

                return View(model);
            }

            return RedirectToAction("ResetPasswordConfirmation");
        }
    }
}
