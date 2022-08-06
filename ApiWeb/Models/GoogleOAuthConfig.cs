namespace Api.Models
{
    public class GoogleOAuthConfig
    {
        public string Discovery { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string RedirectUri { get; set; }
        public string WebAppUri { get; set; }

    }
}
