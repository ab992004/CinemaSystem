using CinemaSystem.Models;
using CinemaSystem.Repositories.IRepositories;
using CinemaSystem.ViewModel;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Common;
using System.Threading.Tasks;

namespace CinemaSystem.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<ApplicationUserOTP> _applicationUserOTPRepository;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailSender emailSender, IRepository<ApplicationUserOTP> applicationUserOTPRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _applicationUserOTPRepository = applicationUserOTPRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);


            var user = new AppUser()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                UserName = registerVM.UserName,
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if(!result.Succeeded)
            {
                foreach (var item in result.Errors)
                    ModelState.AddModelError(string.Empty, item.Code);

                return View(registerVM);
            }

            // Send Email Confirmation
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var link = $"{Request.Scheme}://{Request.Host}/Identity/Account/Confirm?token={token}&id={user.Id}";
            var link = Url.Action("Confirm", "Account", new { area = "Identity", token, user.Id }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Cinema System - Please Confirm Your Email",
                $"<h1>Please confirm your email by clicking <a href='{link}'>here</a></h1>");

            TempData["success-notification"] = "Create Account Successfully, Please Confirm Your Email!";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Confirm(string token, string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                TempData["error-notification"] = String.Join(", ", result.Errors.Select(e => e.Code));
            else
                TempData["success-notification"] = "Confirm Account Successfully";

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail) ?? await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);

            if(user is null)
            {
                // Add msg in ModelState
                ModelState.AddModelError("UserNameOrEmail", "Invalid User Name / Email");
                ModelState.AddModelError("Password", "Invalid Password");
                ModelState.AddModelError(string.Empty, "Invalid User Name / Email Or Password");

                // Add msg in notification
                TempData["error-notification"] = "Invalid User Name / Email Or Password";

                return View(loginVM);
            }

            //await _userManager.CheckPasswordAsync(user, loginVM.Password);
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, isPersistent: loginVM.RememberMe, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Too Many Attempts, Please Try Again Later");
                    // Send Mail
                }
                else if (result.IsNotAllowed)
                    ModelState.AddModelError(string.Empty, "Please Confirm Your Account!!");
                else
                {
                    // Add msg in ModelState
                    ModelState.AddModelError("UserNameOrEmail", "Invalid User Name / Email");
                    ModelState.AddModelError("Password", "Invalid Password");
                    ModelState.AddModelError(string.Empty, "Invalid User Name / Email Or Password");

                    // Add msg in notification
                    TempData["error-notification"] = "Invalid User Name / Email Or Password";
                }
                return View(loginVM);
            }

            TempData["success-notification"] = "Welcome Back!";
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [HttpGet]
        public IActionResult ResendEmailConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM resendEmailConfirmationVM)
        {
            if (!ModelState.IsValid)
                return View(resendEmailConfirmationVM);

            var user = await _userManager.FindByNameAsync(resendEmailConfirmationVM.UserNameOrEmail) ?? await _userManager.FindByEmailAsync(resendEmailConfirmationVM.UserNameOrEmail);

            if (user is null)
            {
                // Add msg in ModelState
                ModelState.AddModelError("UserNameOrEmail", "Invalid User Name / Email");
                ModelState.AddModelError(string.Empty, "Invalid User Name / Email");

                return View(resendEmailConfirmationVM);
            }

            if (!user.EmailConfirmed)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action("Confirm", "Account", new { area = "Identity", token, user.Id }, Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email!, "Resend: Cinema Systme - Please Confirm Your Email",
                    $"<h1>Please confirm your email again by clicking <a href='{link}'>here</a></h1>");
            }

            TempData["success-notification"] = "Send Email Successfully if exist";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if (!ModelState.IsValid)
                return View(forgetPasswordVM);

            var user = await _userManager.FindByNameAsync(forgetPasswordVM.UserNameOrEmail) ?? await _userManager.FindByEmailAsync(forgetPasswordVM.UserNameOrEmail);

            if (user is null)
            {
                // Add msg in ModelState
                ModelState.AddModelError("UserNameOrEmail", "Invalid User Name / Email");
                ModelState.AddModelError(string.Empty, "Invalid User Name / Email");

                return View(forgetPasswordVM);
            }

            var otp = new Random().Next(1000, 9999);

            await _emailSender.SendEmailAsync(user.Email!, "Cinema Syetem - Reset Password",
                    $"<h1>Please Reset your account using otp: {otp}. Please Don't share it.");

            //// save otp in db
            //var totalOTPs = (await _applicationUserOTPRepository.GetAsync(e => e.ApplicationUserId == user.Id && (DateTime.UtcNow - e.CreatedAt).TotalHours < 24 )).Count();

            //if(totalOTPs == 3)
            //{
            //    ModelState.AddModelError(string.Empty, "Too Many Attempts, Please try again later");

            //    return View(forgetPasswordVM);
            //}
            //else
            //{
                await _applicationUserOTPRepository.CreateAsync(new()
                {
                    OTP = otp.ToString(),
                    ApplicationUserId = user.Id,
                });
                await _applicationUserOTPRepository.CommitAsync();

                TempData["success-notification"] = "Send Email Successfully if exist";
            //}

            return RedirectToAction(nameof(ValidateOTP), new { userId = user.Id });
        }

        [HttpGet]
        public IActionResult ValidateOTP(string userId)
        {
            if (userId is null)
                return NotFound();

            return View(new ValidateOTPVM()
            {
                UserId = userId
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateOTP(ValidateOTPVM validateOTPVM)
        {
            if (!ModelState.IsValid)
                return View(validateOTPVM);

            var user = await _userManager.FindByIdAsync(validateOTPVM.UserId);

            if (user is null)
                return NotFound();

            var result = await _applicationUserOTPRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.OTP == validateOTPVM.OTP && e.IsValid && e.ValidTo > DateTime.UtcNow);

            if(result is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid or Expired OTP");

                return View(validateOTPVM);
            }

            result.IsValid = false;
            await _applicationUserOTPRepository.CommitAsync();

            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //await _userManager.ConfirmEmailAsync(user, token);

            return RedirectToAction(nameof(ResetPassword), new { userId = user.Id });
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId)
        {
            if (userId is null)
                return NotFound();

            return View(new ResetPasswordVM()
            {
                UserId = userId
            });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordVM);

            var user = await _userManager.FindByIdAsync(resetPasswordVM.UserId);

            if (user is null)
                return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                    ModelState.AddModelError(string.Empty, item.Code);

                return View(resetPasswordVM);
            }

            TempData["success-notification"] = "Reset Password Successfully";

            return RedirectToAction(nameof(Login));
        }
    }
}
