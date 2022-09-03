using ApiWeb.Database;
using ApiWeb.Models;
using ApiWeb.Repositories.TheRepository;
using ApiWeb.Respositories.UserRepository;
using Logic.Efficacy;
using Microsoft.EntityFrameworkCore;


//using bl = BusinessLayer;

namespace ApiWeb.Respositories.UserRepository
{
    public class UserService : Repository<User>, IUserService
    {
        private readonly DbContext context;
        public UserService(TheContext context) : base(context)
        {
            this.context = context;
        }


        public async Task<bool> RegisterDirectViaMock(User u)
        {
            //l = BusinessLayer.BusinessLogic.CreateLogic();

            var user = new User
            {
                Password = Hashing.PasswordHash(u.Password),
                EmailAddress = u.EmailAddress,
                Roles = u.Roles,
            };
            await context.AddAsync(user);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Register(User u)
        {
            //l = BusinessLayer.BusinessLogic.CreateLogic();

            var user = new User
            {
                Password = Hashing.PasswordHash(u.Password),
                EmailAddress = u.EmailAddress,
                Roles = u.Roles,
            };
            await AddAsync(user);
            await SaveAsync();
            return true;
        }

        public async Task<bool> UpdatePassword(User u)
        {
            var result = await DbSet.SingleOrDefaultAsync(x => x.UserId == u.UserId);
            if (result != null)
            {
                result.Password = Hashing.PasswordHash(u.Password);
            }
            await SaveAsync();
            return true;
        }

        public async Task<Guid> GetUserIdOnEmail(string email) => (await DbSet.SingleOrDefaultAsync(x => x.EmailAddress == email)).UserId;

        public async Task<User> Authenticate(User u)
        {

            var user = await DbSet.SingleOrDefaultAsync(x => x.EmailAddress == u.EmailAddress);
            if (user != null && Hashing.PasswordVerify(u.Password, user.Password)) return user;
            return null;
        }


        public async Task<bool> IsUserExists(string EmailAddress) => await DbSet.SingleOrDefaultAsync(x => x.EmailAddress == EmailAddress) != null;

        public string GetResetPasswordUrl(int length) => Randomisor.GenerateRandomCryptoString(length);
    }
}
