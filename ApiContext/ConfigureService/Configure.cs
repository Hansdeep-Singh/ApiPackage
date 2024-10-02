using AppContext.Interface;
using AppContext.Service;
using Logic.Efficacy.EncryptDecrypt;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppContext.ConfigureService
{
    public static class Configure
    {
        public static IServiceCollection ConfigureAppContext(this IServiceCollection services)
        {
            return services.AddScoped<ISessionService, SessionService>()
                .AddScoped<IHashingService, HashingService>()
                .AddScoped<IApplicationContext, ApplicationContext>();

        }
    }
}
