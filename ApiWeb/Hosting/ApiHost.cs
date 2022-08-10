using ApiWeb.Models;
using ApiWeb.Trigger;

namespace ApiWeb.Hosting
{
    public sealed class ApiHost
    {
        private readonly string[] args;

        public ApiHost(string[] args)
        {
            this.args = args;
        }
        public void BuildHost()
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            services.AddControllers();
            services.AddDistributedMemoryCache();
            //Gets configuration from appsettings.json files (Connects)
            services.Configure<JwtOptions>(
                builder.Configuration.GetSection("jwt"));

            var settings = builder.Configuration.GetSection("jwt").Get<JwtOptions>();
            var config = builder.Configuration.GetSection("ConnectionStrings:ConnectionString");
            string[] origins = { "http://localhost:4200", "http://www.hansdeep.com", "http://hansdeep.com" };
            services.Database(config.Value);
            services.Services();
            services.HttpCalls();
            services.Authentication("This_is_mySecret_key_whats_yourswouldyouliektobefriendsweithme");
            services.Cors(origins);
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {

            }
            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            // app.UseMiddleware<TokenManager>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            app.Run();
        }

    }
}
