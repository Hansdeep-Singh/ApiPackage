
using System.IdentityModel.Tokens.Jwt;

using System.Text;

using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using ApiWeb.Models;
using System.Security.Cryptography;
using Logic.Efficacy;

namespace ApiWeb.Service.TokenService
{
    public class Token : TokenBase
    {
        private readonly IConfiguration configuration;
        public Token(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IDistributedCache distributedCache) : base(httpContextAccessor,configuration,distributedCache)
        {
            this.configuration = configuration;
        }

        public override string GenerateToken(Guid UserId, string Role, string Type)
        {
            var settings = configuration.GetSection($"Jwt:{Type}").Get<TokenConfig>();
            var tokenKey = Encoding.ASCII.GetBytes(settings.SecretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                (new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
                    new Claim(ClaimTypes.Role, Role)
                }),
                Audience = settings.Audience,
                Issuer = settings.Issuer,
                //Expires = DateTime.Now.AddMinutes(ExpiryMinutes),
                Expires = DateTime.Now.AddDays(settings.ExpiryDays),
                SigningCredentials = new SigningCredentials(
                  new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public override string GenerateRefreshToken(int length)
        {
            var randomNumber = new byte[length];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

       // public string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public abstract class TokenBase : IToken
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly IDistributedCache distributedCache;
        public TokenBase(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IDistributedCache distributedCache)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.distributedCache = distributedCache;
            this.configuration = configuration;

        }

        public abstract string GenerateToken(Guid UserId, string Role, string Type);

        public abstract string GenerateRefreshToken(int length);

        public bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(token);
            var timestamp = decodedValue?.Payload?.Exp;
            DateTime date = Misc.ConvertFromUnixTimeStamp(timestamp);
            return (date<DateTimeOffset.UtcNow);
        }

        public Guid GetUserId() {
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(GetCurrentAsync());
            return Guid.Parse(decodedValue.Claims.ElementAt(0).Value);
        }

        public async Task<bool> IsCurrentActiveToken() => await IsActiveAcync(GetCurrentAsync());
        public  async Task DeactivateCurrentAsync() => await DeactivateAsync(GetCurrentAsync());
        public  async Task<bool> IsActiveAcync(string token) => await distributedCache.GetStringAsync(GetKey(token)) == null;

        public string GetCurrentAsync()
        {
            var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["authorization"];
            return authorizationHeader == StringValues.Empty ? string.Empty : authorizationHeader.Single().Split(" ").Last();
        }
        public  async Task DeactivateAsync(string token) => await distributedCache.SetStringAsync(GetKey(token), " ", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(configuration.GetSection("Jwt:Access").Get<TokenConfig>().ExpiryMinutes)
        });
        protected static string GetKey(string token) => $"tokens:{token}:deactivated";
    }
}