using BC = BCrypt.Net.BCrypt;
namespace Logic.Efficacy.EncryptDecrypt
{
    public class HashingService : IHashingService
    {
        public string PasswordHash(string password)
        {
            return BC.HashPassword(password);
        }
        public bool PasswordVerify(string pass, string dbpass)
        {
            return BC.Verify(pass, dbpass);
        }

    }
}