using Api.Models;

namespace Api.Repositories.TokenRepository
{
    public interface ITokenService
    {
        Task<bool> SaveRefreshToken(Guid userId, string refreshToken);
        Task<TheToken> OneRefreshToken(Guid userId);
        Task<List<TheToken>> AllRefreshTokens(Guid userId);
        Task<TheToken> OneRefreshTokenOnUserId(Guid userId);
        Task<bool> IsTokenExpired(string token);
        Task<bool> IsTokenValid(string token);
    }
}
