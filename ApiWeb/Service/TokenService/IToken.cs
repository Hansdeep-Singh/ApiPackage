using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeb.Service.TokenService
{
    public interface IToken 
    {
        string GenerateToken(string UserName, string Key, string UserId,
            string Audience, string Issuer, string Roles,
            int ExpiryMinutes, int ExpiryDays);
        string GenerateRefreshToken();
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAcync(string token);
        Task DeactivateAsync(string token);
    }
}