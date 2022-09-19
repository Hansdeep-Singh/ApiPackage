namespace ApiWeb.Constants
{
    public static class AppConsts
    {
        public static readonly string[] CORSOrigins = { "http://localhost:4200", "http://www.hansdeep.com", "http://hansdeep.com", "http://127.0.0.1:5500" };
        
        public enum TokenType
        {
            Access,
            Refresh,
            Reset
        }
    }
}
