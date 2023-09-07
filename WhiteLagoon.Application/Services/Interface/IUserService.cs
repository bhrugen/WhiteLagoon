using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Interface
{
	public interface IUserService
	{
        Task<ApplicationUser> GetUserById(string id);
        Task<ApplicationUser> GetUserByEmail(string email);
		IEnumerable<ApplicationUser> GetAllUsers();
	}
}
