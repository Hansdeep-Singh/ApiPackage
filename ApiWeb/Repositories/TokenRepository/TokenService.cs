using Api.Database;
using Api.Models;
using Api.Repositories.TheRepository;
using System.Collections;

namespace Api.Repositories.TokenRepository
{
    public class TokenService : Repository<TheToken>, ITokenService
    {
        public TokenService(TheContext context) : base(context)
        {
        }
        public async Task<bool> SaveRefreshToken(Guid userId, string refreshToken)
        {
            var TheToken = new TheToken
            {
                UserId = userId,
                RefreshToken = refreshToken
            };
            await AddAsync(TheToken);
            await SaveAsync();
            return true;
        }
        public async Task<bool> IsTokenExpired(string token) => (await GetAllAsync()).Where(t => t.RefreshToken == token).FirstOrDefault().ExpireDate <= DateTime.Now;
        public async Task<bool> IsTokenValid(string token) => (await GetAllAsync()).Where(t => t.RefreshToken == token).FirstOrDefault()!=null;
        public async Task<TheToken> OneRefreshToken(Guid id) => (await GetOneGuidIdAsync(id));
        public async Task<TheToken> OneRefreshTokenOnUserId(Guid userId) => (await GetAllAsync()).Where(id => id.UserId == userId).FirstOrDefault();
        public async Task<List<TheToken>> AllRefreshTokens(Guid userId) => (await GetAllAsync()).Where(id => id.UserId == userId).ToList();

    }
}
