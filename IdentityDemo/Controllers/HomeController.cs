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
        private readonly IAuthorizationService _authenticationService;

        public HomeController(IAuthorizationService authenticationService)
        {
            this._authenticationService = authenticationService;
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

        [Authorize(Policy = "DoB")]
        [Route("/dob")]
        public IActionResult Dob()
        {
            return Ok("Hello Dob");
        }

        [Authorize(Roles = "Admin")]
        [Route("/admin")]
        public IActionResult Admin()
        {
            return Ok("Hello Admin");
        }

        [Route("/logindob")]
        public async Task<IActionResult> LoginDob()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Fred"),
                new Claim(ClaimTypes.Email, "fred@123.com"),
                new Claim(ClaimTypes.DateOfBirth, "17/2/2019"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var ci = new ClaimsIdentity(claims, "My Claim");
            var cp = new ClaimsPrincipal(new[] { ci });
            await HttpContext.SignInAsync(cp);
            return Ok("Login Success");
        }

        public async Task<IActionResult> DoStuff()
        {
            // we are doing stuff here
            var builder = new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("Hello").Build();
            var authResult = await _authenticationService.AuthorizeAsync(User, customPolicy);

            if (authResult.Succeeded)
            {
                return View("Index");
            }

            return View("Error");
        }
    }
}
