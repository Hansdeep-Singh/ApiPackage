using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using ApiWeb.Models;
using System.Security.Cryptography;

namespace ApiWeb.Service.TokenService
{
    public class Token : TokenBase
    {
        public Token(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IDistributedCache distributedCache) : base(httpContextAccessor,configuration,distributedCache)
        {

        }

        public override string GenerateToken(string UserName, string Key, string UserId, string Roles,
            string Audience, string Issuer,
            int ExpiryMinutes, int ExpiryDays)
        {
            var tokenKey = Encoding.ASCII.GetBytes(Key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                (new Claim[] {
                    new Claim(ClaimTypes.Name, UserName),
                    new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
                    new Claim(ClaimTypes.Role, Roles)
                }),
                Audience = Audience,
                Issuer = Issuer,
                //Expires = DateTime.Now.AddMinutes(ExpiryMinutes),
                Expires = DateTime.Now.AddDays(ExpiryDays),
                SigningCredentials = new SigningCredentials(
                  new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public override string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
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

        public abstract string GenerateToken(string UserName, string Key, string UserId, string Roles,
           string Audience, string Issuer,
           int ExpiryMinutes, int ExpiryDays);

        public abstract string GenerateRefreshToken();

        public async Task<bool> IsCurrentActiveToken() => await IsActiveAcync(GetCurrentAsync());
        public  async Task DeactivateCurrentAsync() => await DeactivateAsync(GetCurrentAsync());
        public  async Task<bool> IsActiveAcync(string token) => await distributedCache.GetStringAsync(GetKey(token)) == null;

        private string GetCurrentAsync()
        {
            var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["authorization"];
            return authorizationHeader == StringValues.Empty ? string.Empty : authorizationHeader.Single().Split(" ").Last();
        }
        public  async Task DeactivateAsync(string token) => await distributedCache.SetStringAsync(GetKey(token), " ", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(configuration.GetSection("jwt:jwtAccess").Get<TokenConfig>().ExpiryMinutes)
        });
        protected static string GetKey(string token) => $"tokens:{token}:deactivated";
    }
}