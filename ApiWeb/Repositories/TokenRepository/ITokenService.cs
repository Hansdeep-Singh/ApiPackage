using ApiWeb.Models;

namespace ApiWeb.Repositories.TokenRepository
{
    public interface ITokenService
    {
        Task<bool> SaveToken(Guid userId, string Token, string Type);
        Task<TheToken> OneRefreshToken(Guid userId);
        Task<List<TheToken>> AllRefreshTokens(Guid userId);
        Task<TheToken> OneRefreshTokenOnUserId(Guid userId);
        Task<bool> IsTokenValid(string token);
    }
}
