using IdentityDemo.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityDemo.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IdentityController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Route("/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                //sign in
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
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
