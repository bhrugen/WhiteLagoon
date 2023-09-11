using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Application.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AccountController : Controller
    {         
        private readonly IAuthService _authService;
        private readonly IRoleService _roleService;

        public AccountController(IAuthService authService, IRoleService roleService)
        {
            _authService = authService;
            _roleService = roleService;
        }
        public IActionResult Login(string? returnUrl)
        {
            returnUrl??= Url.Content("~/");

            LoginVM loginVM = new ()
            {
                RedirectUrl = returnUrl
            };

            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Register(string? returnUrl)
        {
            returnUrl ??= Url.Content("~/");

            // Ensure that required roles exist (admin and customer)
            _roleService.EnsureRolesExistAsync().Wait();

            var registerVM = new RegisterVM
            {
                RoleList = _roleService.GetRoleList(),
                RedirectUrl = returnUrl
            };

            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                var returnUrl = registerVM.RedirectUrl ?? Url.Content("~/");

                if (await _authService.RegisterAsync(registerVM))
                {
                    return LocalRedirect(returnUrl); // Registration successful
                }
            }
           
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var returnUrl = loginVM.RedirectUrl ?? Url.Content("~/");

                if (await _authService.LoginAsync(loginVM, returnUrl))
                {
                    return RedirectToAction("Index", "Dashboard"); // Redirect to Dashboard for admins
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }           

            return View(loginVM);
        }
    }
}
