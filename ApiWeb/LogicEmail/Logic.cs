using BC = BCrypt.Net.BCrypt;
namespace ApiWeb.LogicEmail

{
    public class Logic 
    {
        public string PasswordHash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        public bool PasswordVerify(string pass, string dbpass) => BC.Verify(pass, dbpass);
    }
}
