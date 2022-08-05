using BC = BCrypt.Net.BCrypt;
namespace Logic
{
    public class Hashing
    {
        public string PasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public bool PasswordVerify(string pass, string dbpass)
        {
            return BC.Verify(pass, dbpass);
        }

    }
}