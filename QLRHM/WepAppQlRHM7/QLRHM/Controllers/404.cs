using Microsoft.AspNetCore.Mvc;

namespace QLRHM7.Controllers
{
    public class _404 : Controller
    {
        public IActionResult Bug()
        {
            return View();
        }
         public IActionResult AccessDenied()
        {
            return View();
        }



    }
}
