using Microsoft.AspNetCore.Mvc.Rendering;

namespace WhiteLagoon.Application.Services.Interface
{
	public interface IRoleService
    {
        Task<bool> EnsureRolesExistAsync();
        IEnumerable<SelectListItem> GetRoleList();
    }

}
