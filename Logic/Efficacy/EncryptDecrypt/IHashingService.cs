namespace Logic.Efficacy.EncryptDecrypt
{
    public interface IHashingService
    {
        string PasswordHash(string password);
        bool PasswordVerify(string pass, string dbpass);
    }
}