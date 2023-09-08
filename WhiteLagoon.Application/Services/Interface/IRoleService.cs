using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Application.Services.Interface
{
    public interface IRoleService
    {
        IEnumerable<SelectListItem> GetRoleList();
    }
}
