using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class JwtOptions
    {
        public const string JwtAccess = "JwtAccess";
        public const string JwtRefresh = "JwtRefresh";
        public TokenConfig JwtAccessConfig { get; set; }
        public TokenConfig JwtRefreshConfig { get; set; }
    }
    public class TokenConfig
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; }
        public int ExpiryDays { get; set; }
        public bool ValidationLifetime { get; set; }
    }
   
}
