using Api.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Api.Service.TokenService
{
    public class RefreshToken
    {
        private readonly JwtOptions jwtRefreshOptions;
        private readonly IConfiguration configuration;
        private readonly IToken itoken;
        public RefreshToken(IOptionsSnapshot<JwtOptions> namedOptionAccessor, IConfiguration configuration, IToken itoken)
        {
            this.itoken = itoken;
            jwtRefreshOptions = namedOptionAccessor.Value;
            this.configuration = configuration;
        }
        public string GenerateRefreshToken(string UserName, Guid UserId, string Roles)
        {
            var settings = configuration.GetSection("jwt:jwtRefresh").Get<TokenConfig>();

            return itoken.GenerateToken(UserName, settings.SecretKey, UserId.ToString(), Roles,
                settings.Audience, settings.Issuer, settings.ExpiryMinutes, settings.ExpiryDays
                );

        }

        //Options Snap Shot
        //public string GenerateRefreshToken(string UserName, Guid UserId, string Roles)
        //{
        //    return itoken.GenerateToken(UserName, jwtRefreshOptions.JwtRefreshConfig.SecretKey, UserId.ToString(), Roles,
        //        jwtRefreshOptions.JwtRefreshConfig.Audience, jwtRefreshOptions.JwtRefreshConfig.Issuer, jwtRefreshOptions.JwtRefreshConfig.ExpiryMinutes
        //  );
        //}

        public string GenerateSimpleRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));


    }
}
