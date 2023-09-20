using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
            return View("Index");
        }

        public IActionResult Authenticate()
        {// user will be created here

            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Mubi"),
                new Claim(ClaimTypes.Email,"Mubi@123"),
                new Claim(ClaimTypes.DateOfBirth,"11/11/2000"),
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

            //-----------------------------------------------------------------------------------//

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }

        [Authorize(Policy = "Claim.DoB")]
        // used to gaurd an action (.) Are you allowed to come here?
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }

        public async Task<IActionResult> DoStuff(
            [FromServices] IAuthorizationService authorizationService)
        {
            // we are doing some stuff here.

            var builder = new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("Hello").Build();

            var authResult = await authorizationService.AuthorizeAsync( User, customPolicy);

            if (authResult.Succeeded)
            {
                return View("Index");
            }
            return View("Index");
        }
    }
}
