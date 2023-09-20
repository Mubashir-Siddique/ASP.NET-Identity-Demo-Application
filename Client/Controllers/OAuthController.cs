using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public IActionResult Authorize()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authorize(string username)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Token()
        {
            return View();
        }
    }
}
