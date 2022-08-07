using ApiWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeb.Respositories.UserRepository
{
    public interface IUserService
    {
        Task<bool> Register(User u);
        Task<User> Authenticate(User u);
        Task<bool> IsUserExists(string EmailAddress);
    }

}
