﻿using ApiWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeb.Respositories.UserRepository
{
    public interface IUserService
    {
        Task<bool> Register(User u);
        Task<User> AuthenticateViaEmail(User u);
        Task<User> AuthenticateViaUserName(User u);
        Task<bool> IsUserExists(string EmailAddress);
        string GetResetPasswordUrl(int length);
        Task<Guid> GetUserIdOnEmail(string email);
        Task<bool> UpdatePassword(User u);
      
    }

}
