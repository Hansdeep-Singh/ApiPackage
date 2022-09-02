
using ApiWeb.Database;
using ApiWeb.MiddleWare;
using ApiWeb.Repositories.TheRepository;
using ApiWeb.Repositories.TokenRepository;
using ApiWeb.Respositories.UserRepository;
using ApiWeb.Service.oAuthService;
using ApiWeb.Service.TokenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace ApiWeb.Trigger
{
    public static class ServiceExtention
    {
        public static IServiceCollection Services (this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IToken, Token>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<TokenManager>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<GoogleApi>();
            return services;
        }
        public static IServiceCollection HttpCalls(this IServiceCollection services)
        {
            return services.AddHttpClient();
        }
        public static IServiceCollection Authentication(this IServiceCollection services , string secret )
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudiences = new[] { "http://localhost:4200/" }, // Link of where the site is hosted (could be anything really)
                    ValidIssuer = "https://localhost:44385/" // Link of where the API server is hosted (could be anything really)
                };


            });
            return services;
        }
        public static IServiceCollection Cors(this IServiceCollection services, string[] origins)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(origins)
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                    }
                    );
            });
            return services;
        }

        public static IServiceCollection Database(this IServiceCollection services, string conn)
        {
            services.AddDbContext<TheContext>(options => options.UseSqlServer(conn));
            return services;
        }
    }
}
