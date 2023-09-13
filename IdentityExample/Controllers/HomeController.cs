using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Globalization;
using System.Security.Claims;

namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailService emailService)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

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

        [Authorize(Policy = "Claim.DoB")]
        // used to gaurd an action (.) Are you allowed to come here?
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            // Login functionality work 

            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                // Sign in functionality
                var SignInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (SignInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password)
        {
            // Register functionality work 

            var user = new IdentityUser()
            {
                UserName = userName,
                Email=""
            };

            var result = await _userManager.CreateAsync(user,password);

            if (result.Succeeded)
            {
                //Generation of email token

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());

                await _emailService.SendAsync("Test@test.com", "email Verification", $"<a href=\"{link}\">Verify Email</a>", true);

                return RedirectToAction("EmailVerification");


                //// Sign in functionality
                //var SignInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                //if (SignInResult.Succeeded)
                //{
                //    return RedirectToAction("Index");
                //}
            }

            return RedirectToAction("Index");
        }

        public IActionResult EmailVerification() => View();

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return BadRequest();
             
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return View();
            }

            return BadRequest();
        }
        public IActionResult Register()
        {
            return View();
        }


        public IActionResult Authenticate()
        {// user will be created here

            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Mubi"),
                new Claim(ClaimTypes.Email,"Mubi@123"),
                new Claim(ClaimTypes.DateOfBirth,"11/11/2000"),
                new Claim(ClaimTypes.Role,"Admin2"),
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

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
