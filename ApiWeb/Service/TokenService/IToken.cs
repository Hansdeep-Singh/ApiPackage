using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeb.Service.TokenService
{
    public interface IToken 
    {
        string GenerateToken(Guid UserId, string Role, string Type);
        string GenerateRefreshToken(int length);
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        bool IsTokenExpired(string token);
        Guid GetUserId();
        Task<bool> IsActiveAcync(string token);
        Task DeactivateAsync(string token);
        string GetCurrentAsync();
    }
}