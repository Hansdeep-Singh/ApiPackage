using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeb.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool LookedOut { get; set; }
        public bool RememberMe { get; set; }
        public string Roles { get; set; }
        public string PhotoString { get; set; }
        public bool IsLockedOut { get; set; }
    }
    public class LoginResponse
    {
        public User User { get; set; }
        public Tokens Tokens { get; set; }

    }
    public class Tokens
    {

        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; }
        public bool IsRefreshTokenExpired { get; set; } = false;
        public string SimpleRefreshToken { get; set; }
    }
}


