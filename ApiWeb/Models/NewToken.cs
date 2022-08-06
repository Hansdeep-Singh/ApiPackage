namespace Api.Models
{
    public class NewTokenRequest
    {
        public User user { get; set; }
        public string refreshToken { get; set; }
    }

    public class NewTokenResponse
    {
        public bool IsRefreshTokenValid { get; set; } = false;
        public string RefreshToken { get; set; } = string.Empty;
        public string AccessToken { get; set; }

    }
}
