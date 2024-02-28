using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;
using QLRHM7.ViewModel;

namespace QLTQNK.Controllers
{
    [AllowAnonymous]
    public class loginController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public loginController(DatnqlrhmContext context)
        {
            _context = context;
        }


        public async Task<IActionResult>Login()
        {


            return View();
        }
    
            //[AllowAnonymous]
            [HttpGet]
            [Route("/login")]
            public IActionResult Index(string returnUrl)
            {
                if (HttpContext.User.Identity.Name == null)
                    return View("Login");
                else
                {
                    if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith("/"))
                    {
                        returnUrl = "/";
                    }
                    return Redirect(returnUrl);
                }
            }


        public bool VerifyPassword(string plainText, string hashedPassword)
        {
            SHA512Encryption sha = new SHA512Encryption();
            return sha.Verify(plainText, hashedPassword);
        }

        
        [HttpPost]
        [Route("/login")]
		public async Task<IActionResult> LoginUser(TaiKhoan taiKhoan, string returnUrl)
		{
            TaiKhoan a = _context.TaiKhoans.Include(x => x.IdbsNavigation).FirstOrDefault(x => x.TenTaiKhoan == taiKhoan.TenTaiKhoan);

			if (a == null)
			{
				TempData["LoginFailed"] = true;
				return RedirectToAction("Index");
			}
            // Trong hàm LoginUser:
            string hashedPasswordInput = taiKhoan.MatKhau; // Đây là mật khẩu nhập vào từ người dùng


            if (VerifyPassword(taiKhoan.MatKhau, a.MatKhau))
            {
                // Mật khẩu đúng
                await SignInUser(a);

                if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith("/"))
                {
                    returnUrl = "/Home/Index";
                }
                return Redirect(returnUrl);
            }
            else
            {
                // Mật khẩu không đúng
                TempData["LoginFailed"] = true;
                return RedirectToAction("Index");
            }
        }

		[HttpGet]
            public async Task<IActionResult> Logout()
            {
            TempData["AlertMessaget_logout"] = "";
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return RedirectToAction("Index");
            }
        private async Task SignInUser(TaiKhoan accounts)
        {
            using (var context = new DatnqlrhmContext())
            {
                TaiKhoan user = await context.TaiKhoans
                    .Include(x=>x.IdbsNavigation)
                    .Include(x=>x.IdbsNavigation.IdnbsNavigation)
                    .Where(x => x.Idtk == accounts.Idtk )
                    .FirstOrDefaultAsync();

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Idtk.ToString(), user.IdbsNavigation.TenBs , user.IdbsNavigation.AnhBs),
            new Claim("BacSiID", user.IdbsNavigation.Idbs.ToString() , user.IdbsNavigation.IdnbsNavigation.TenNbs),
            new Claim(ClaimTypes.Role, user.Quyen.ToString()),
        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var authenticationProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Đặt thành true để lưu cookie sau khi đóng trình duyệt
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(14500)
                };
                //await HttpContext.SignInAsync(
                //    CookieAuthenticationDefaults.AuthenticationScheme,
                //    new ClaimsPrincipal(claimsIdentity));
                await HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       principal,
                       authenticationProperties);

            }
        }





    }
    }

