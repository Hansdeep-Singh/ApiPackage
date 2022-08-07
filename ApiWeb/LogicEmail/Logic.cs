using BC = BCrypt.Net.BCrypt;
namespace ApiWeb.LogicEmail

{
    public class Logic 
    {
        public async Task<string> PasswordHash(string password)
        {
            var t = Task.Run(() =>
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            });
            return await t;
        }
        public async Task<bool> PasswordVerify(string pass, string dbpass)
        {
            var t = Task.Run(() =>
            {
                return BC.Verify(pass, dbpass);
            });
            return await t;
        }
    }
}
