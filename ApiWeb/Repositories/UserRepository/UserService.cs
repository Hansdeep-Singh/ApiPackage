using ApiWeb.Database;
using ApiWeb.Models;
using ApiWeb.Repositories.TheRepository;
using ApiWeb.Respositories.UserRepository;
using Logic;
using Microsoft.EntityFrameworkCore;


//using bl = BusinessLayer;

namespace ApiWeb.Respositories.UserRepository
{
    public class UserService : Repository<User>, IUserService
    {
        private readonly DbContext context;
        public UserService(TheContext context): base(context)
        {
            this.context = context;
        }


        public async Task<bool> Registert(User u)
        {
            //l = BusinessLayer.BusinessLogic.CreateLogic();
            Hashing h = new();
            var user = new User
            {
                Password = h.PasswordHash(u.Password),
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
            Hashing h = new();
            var user = new User
            {
                Password =  h.PasswordHash(u.Password),
                EmailAddress = u.EmailAddress,
                Roles = u.Roles,
            };
            await AddAsync(user);
            await SaveAsync();
            return true;
        }
        public async Task<User> Authenticate(User u)
        {
            Hashing h = new();
            var user =await DbSet.SingleOrDefaultAsync(x => x.EmailAddress == u.EmailAddress);
            if (user != null && h.PasswordVerify(u.Password, user.Password)) return user;
            return null;
        }
        public async Task<bool> IsUserExists(string EmailAddress) => await DbSet.SingleOrDefaultAsync(x => x.EmailAddress == EmailAddress) != null;
    }
}
