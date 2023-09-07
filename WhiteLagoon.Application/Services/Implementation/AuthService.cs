using Microsoft.AspNetCore.Identity;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Application.ViewModels;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
	public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<bool> LoginAsync(LoginVM loginVM, string returnUrl)
        {
            var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginVM.Email);

                if (user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                    {
                        return true;
                    }
                    else
                    {
                        return !string.IsNullOrEmpty(returnUrl);
                    }
                }
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> RegisterAsync(RegisterVM registerVM)
        {
            ApplicationUser user = new()
            {
                // Populate user properties here...
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (result.Succeeded)
            {
                var roleName = string.IsNullOrEmpty(registerVM.Role) ? SD.Role_Customer : registerVM.Role;

                if (!await EnsureRoleExistsAsync(roleName))
                {
                    return false;
                }

                await _userManager.AddToRoleAsync(user, roleName);

                await _signInManager.SignInAsync(user, isPersistent: false);
                
                return true;
            }

            return false;
        }

        private async Task<bool> EnsureRoleExistsAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                return roleResult.Succeeded;
            }

            return true;
        }
    }


}
