﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.ViewModels;

namespace WhiteLagoon.Application.Services.Interface
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginVM loginVM, string returnUrl);
        Task LogoutAsync();
        Task<bool> RegisterAsync(RegisterVM registerVM);
    }
}