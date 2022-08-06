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
using Api.Models;
using System.Security.Cryptography;

namespace Api.Service.TokenService
{
    //https://piotrgankiewicz.com/2017/12/07/jwt-refresh-tokens-and-net-core/
    public class Token : IToken
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IDistributedCache distributedCache;
        private readonly JwtOptions jwtAccessOptions;
        public Token(IOptionsSnapshot<JwtOptions> namedOptionAccessor, IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IOptions<JwtOptions> jwtOptions)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.distributedCache = distributedCache;
            jwtAccessOptions = namedOptionAccessor.Get(JwtOptions.JwtAccess);
        }
        public string GenerateToken(string UserName, string Key, string UserId, string Roles,
            string Audience, string Issuer,
            int ExpiryMinutes, int ExpiryDays )
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
        public string GenerateRefreshToken()
        {

            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task<bool> IsCurrentActiveToken() => await IsActiveAcync(GetCurrentAsync());
        public async Task DeactivateCurrentAsync() => await DeactivateAsync(GetCurrentAsync());
        public async Task<bool> IsActiveAcync(string token) => await distributedCache.GetStringAsync(GetKey(token)) == null;
        public async Task DeactivateAsync(string token) => await distributedCache.SetStringAsync(GetKey(token), " ", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(jwtAccessOptions.JwtAccessConfig.ExpiryMinutes)
        });
        private static string GetKey(string token) => $"tokens:{token}:deactivated";
        private string GetCurrentAsync()
        {
            var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["authorization"];
            return authorizationHeader == StringValues.Empty ? string.Empty : authorizationHeader.Single().Split(" ").Last();
        }

    }
}