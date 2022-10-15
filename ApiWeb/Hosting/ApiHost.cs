using ApiWeb.Constants;
using ApiWeb.MiddleWare;
using ApiWeb.Models;
using ApiWeb.Trigger;
using Logic.ConfigureServices;

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
            services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
            var secret = builder.Configuration.GetSection("Jwt:Access").Get<TokenConfig>();
            var config = builder.Configuration.GetSection("ConnectionStrings:ConnectionString"); 
            var cors = builder.Configuration.GetSection("Cors:Origins");
            services.Database(config.Value);
            services.Services();
            services.UserService();
            services.HttpCalls();
            services.Authentication(secret.SecretKey);
            services.Cors(cors.Get<string[]>());
            services.AddSession(options =>
            {
                options.Cookie.Name = "ApiSession";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

             AppKick start = new(app);

            //if (app.Environment.IsDevelopment())
            //{

            //}
            //app.UseCors();
            //app.UseRouting();
            //app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseMiddleware<TokenManager>();
            //app.UseSession();

            ////https://www.youtube.com/watch?v=VuFQtyRmS0E&t=337s&ab_channel=NickChapsas End points

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapDefaultControllerRoute();
            //});
            //app.Run();
        }

    }
}
