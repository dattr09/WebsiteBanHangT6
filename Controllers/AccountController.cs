using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebsiteBanHang.Models;
using WebsiteBanHang.ViewModels;

namespace WebsiteBanHang.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        // GET: Hiển thị trang đăng ký
        // [HttpGet]
        // public IActionResult Register()
        // {
        //     return View();
        // }

        // // POST: Xử lý đăng ký
        // // POST: Xử lý đăng nhập
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Login(LoginViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //         return View(model);

        //     var user = await _userManager.FindByEmailAsync(model.Email);
        //     if (user == null)
        //     {
        //         ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
        //         return View(model);
        //     }

        //     // Nếu email chưa xác thực, chuyển hướng đến trang chờ xác thực
        //     if (!await _userManager.IsEmailConfirmedAsync(user))
        //     {
        //         await _signInManager.SignOutAsync(); // Đảm bảo chưa đăng nhập
        //         return RedirectToAction("EmailVerificationPending", new { email = user.Email });
        //     }

        //     var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
        //     if (result.Succeeded)
        //     {
        //         return RedirectToAction("Index", "Home");
        //     }

        //     ModelState.AddModelError("", "Đăng nhập thất bại. Kiểm tra lại thông tin.");
        //     return View(model);
        // }


        // GET: Hiển thị trang đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Xử lý đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return RedirectToAction("EmailVerificationPending", new { email = user.Email });
            }

#pragma warning disable CS8604 // Possible null reference argument.
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
#pragma warning restore CS8604 // Possible null reference argument.
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Đăng nhập thất bại. Kiểm tra lại thông tin.");
            return View(model);
        }

        // GET: Xác nhận email
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Yêu cầu xác thực email không hợp lệ.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Không tìm thấy người dùng.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["Message"] = "Xác thực email thành công. Bạn có thể đăng nhập ngay!";
                return RedirectToAction("Login");
            }

            return BadRequest("Xác thực email thất bại.");
        }

        // POST: Đăng xuất
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // GET: Trang chờ xác thực email
        [HttpGet]
        public IActionResult EmailVerificationPending(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        // POST: Gửi lại email xác nhận
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || await _userManager.IsEmailConfirmedAsync(user))
            {
                return RedirectToAction("Login");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);

            string subject = "Xác nhận lại tài khoản của bạn";
            string message = $"Nhấn vào link sau để xác thực tài khoản: <a href='{confirmationLink}'>Xác nhận email</a>";

#pragma warning disable CS8604 // Possible null reference argument.
            await _emailSender.SendEmailAsync(user.Email, subject, message);
#pragma warning restore CS8604 // Possible null reference argument.

            TempData["Message"] = "Email xác nhận đã được gửi lại. Vui lòng kiểm tra hộp thư của bạn.";
            return RedirectToAction("EmailVerificationPending", new { email = user.Email });
        }
    }
}
