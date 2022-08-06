using Api.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Service.TokenService
{
    public class AccessToken
    {
        private readonly JwtOptions JwtAccessOptions;
        private readonly IConfiguration configuration;
        private readonly IToken itoken;
        public AccessToken(IOptionsSnapshot<JwtOptions> namedOptionAccessor, IConfiguration configuration, IToken itoken)
        {
            this.itoken = itoken;
            this.JwtAccessOptions = namedOptionAccessor.Value;
            this.configuration = configuration;
        }
        public string GenerateAccessToken(string UserName, Guid UserId, string Roles)
        {
            var settings = configuration.GetSection("jwt:jwtAccess").Get<TokenConfig>();
            return itoken.GenerateToken(UserName, settings.SecretKey, UserId.ToString(), Roles,
                settings.Audience, settings.Issuer, settings.ExpiryMinutes, settings.ExpiryDays
                );
        }
    }
}
