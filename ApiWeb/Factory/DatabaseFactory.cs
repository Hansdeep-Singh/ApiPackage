using ApiWeb.Database;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Factory
{
    public static class DatabaseFactory 
    {
        public static TheContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TheContext>();
            optionsBuilder.UseSqlServer(@"Data Source=Hansdeep;Initial Catalog=Api;Integrated Security=True");
            var ctx = new TheContext(optionsBuilder.Options);
            return ctx;
        }
    }
}
