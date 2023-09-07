using Microsoft.AspNetCore.Identity;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
	public class UserService : IUserService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		public UserService(UserManager<ApplicationUser> userManager) {
			_userManager = userManager;

        }
		public  IEnumerable<ApplicationUser> GetAllUsers()
		{
			return _userManager.Users.ToList();

        }

		public async Task<ApplicationUser> GetUserByEmail(string email)
		{
            return await _userManager.FindByEmailAsync(email);
        }

		public async Task<ApplicationUser> GetUserById(string id)
		{
			return await _userManager.FindByIdAsync(id);
		}
	}
}
