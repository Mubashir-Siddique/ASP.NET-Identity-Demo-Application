using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(
            SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }
        [HttpPost ]
        public IActionResult Login(LoginViewModel vm)
        {
            // Check if the model is valid
            return View();
        }
    }
}
