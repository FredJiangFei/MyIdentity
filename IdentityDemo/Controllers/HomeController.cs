using IdentityDemo.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Route("/index")]
        public IActionResult Index()
        {
            return Ok("Hello Index");
        }

        [Authorize]
        [Route("/secret")]
        public IActionResult Secret()
        {
            return Ok("Hello Secret"); 
        }

        [Route("/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                //sign in
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                //var claims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, "Fred"),
                //    new Claim(ClaimTypes.Email, "fred@123.com")
                //};
                //var ci = new ClaimsIdentity(claims, "Email Claim");
                //var cp = new ClaimsPrincipal(new[] { ci });
                //await HttpContext.SignInAsync(cp);

                if (signInResult.Succeeded)
                {
                    return Ok("Login Success");
                }
            }
            
            return Ok("Login Fail");
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new AppUser
            {
                UserName = username,
                Email = "329126523@qq.com",
                Sex = 1
            };

            var result = await _userManager.CreateAsync(user, password);
            return Ok(result.Succeeded ? "Register Succeeded" : "Register Failed");
        }

        [HttpPost]
        [Route("/logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout Succeeded");
        }
    }
}
