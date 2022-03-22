using System.Threading.Tasks;
using dotnet_blog_mvc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_blog_mvc.Controllers
{
    public class AuthController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager = null)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);

            if (!result.Succeeded) { return Unauthorized(); }

            var user = await _userManager.FindByNameAsync(vm.Username);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin) { return RedirectToAction("Index", "Panel"); }

            return RedirectToAction("Index", "Home");



        }
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                //await _signInManager.(vm.Email, vm.Password, false, false);
                return View(vm);
            }

            var newUser = new IdentityUser
            {
                UserName = vm.Email,
                Email = vm.Email
            };

            var result = await _userManager.CreateAsync(newUser, "password");

            if (result.Succeeded)
            {
                RedirectToAction("Login");
            }

            return View(vm);

        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Panel");
        }

    }
}