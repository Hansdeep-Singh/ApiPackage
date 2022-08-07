namespace ApiWeb.Models
{
    public class OpenIdConnectConfig
    {
        public string Issuer { get; set; } = string.Empty;
        public string Authorization_Endpoint { get; set; } = string.Empty;
        public string Token_Endpoint { get; set; } = string.Empty;
    }
}
