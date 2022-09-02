using BC = BCrypt.Net.BCrypt;
namespace Logic.Efficacy
{
    public static class Hashing
    {
        public static string PasswordHash(string password)
        {
            return BC.HashPassword(password);
        }
        public static bool PasswordVerify(string pass, string dbpass)
        {
            return BC.Verify(pass, dbpass);
        }

    }
}