using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Respositories.UserRepository
{
    public interface IUserService
    {
        Task<bool> Register(User u);
        Task<User> Authenticate(User u);
        Task<bool> IsUserExists(string EmailAddress);
    }

}
