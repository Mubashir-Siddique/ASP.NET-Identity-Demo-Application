using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Login(LoginViewModel vm)
        {
            return View();
        }
    }
}
