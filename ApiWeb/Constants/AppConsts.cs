namespace ApiWeb.Constants
{
    public static class AppConsts
    {
        public static readonly string[] CORSOrigins = { "http://localhost:4200", "http://www.hansdeep.com", "http://hansdeep.com" };
        
        public enum TokenType
        {
            Access,
            Refresh,
            Reset
        }
    }
}
