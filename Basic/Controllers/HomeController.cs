using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Basic.Controllers
{
    public class HomeController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        // used to gaurd an action (.) Are you allowed to come here?
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authenticate()
        {// user will be created here

            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Mubi"),
                new Claim(ClaimTypes.Email,"Mubi@123"),
                new Claim("Grandma Says","Mubi")
            };
            
            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Mubashir S."),
                new Claim("License Type","Car + Bike")
            };

            // Creates an Identity based on obove claims
            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}
