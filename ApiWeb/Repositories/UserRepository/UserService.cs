using ApiWeb.Database;
using ApiWeb.Models;
using ApiWeb.Repositories.TheRepository;
using AppContext.Interface;
using EfficacySend.Models;
using Logic.Efficacy;
using Logic.Efficacy.EncryptDecrypt;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Respositories.UserRepository
{
    public class UserService : Repository<User>, IUserService
    {
        private readonly DbContext context; // Only because mock service otherwise no need. 
        private readonly IApplicationContext applicationContext;
        public UserService(TheContext context, IApplicationContext applicationContext) : base(context)
        {
            this.context = context;
            this.applicationContext = applicationContext;
        }


        public async Task<bool> RegisterDirectViaMock(User u)
        {
            //l = BusinessLayer.BusinessLogic.CreateLogic();
            var user = new User
            {
                Password = applicationContext.HashingService.PasswordHash(u.Password),
                EmailAddress = u.EmailAddress,
                Roles = u.Roles,
            };
            await context.AddAsync(user);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Register(User u)
        {
            var user = new User
            {
                Password = applicationContext.HashingService.PasswordHash(u.Password),
                EmailAddress = u.EmailAddress,
                UserName = u.UserName,
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
                result.Password = applicationContext.HashingService.PasswordHash(u.Password);
            }
            await SaveAsync();
            return true;
        }

        public async Task<Guid> GetUserIdOnEmail(string email) => (await DbSet.SingleOrDefaultAsync(x => x.EmailAddress == email)).UserId;

        public async Task<User> AuthenticateViaEmail(User u)
        {

            var user = await DbSet.SingleOrDefaultAsync(x => x.EmailAddress == u.EmailAddress);
            if (user != null && applicationContext.HashingService.PasswordVerify(u.Password, user.Password)) return user;
            return null;
        }

        public async Task<User> AuthenticateViaUserName(User u)
        {

            var user = await DbSet.SingleOrDefaultAsync(x => x.UserName== u.UserName);
            if (user != null && applicationContext.HashingService.PasswordVerify(u.Password, user.Password)) return user;
            return null;
        }
        public Task<bool> SendForgetPasswordEmail(Email em)
        {
            return applicationContext.SendEmail(em);
        }


        public async Task<bool> IsUserExists(string EmailAddress) => await DbSet.SingleOrDefaultAsync(x => x.EmailAddress == EmailAddress) != null;

        public string GetResetPasswordUrl(int length) => Randomisor.GenerateRandomCryptoString(length);
    }
}
