using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Efficacy.EncryptDecrypt;
using Microsoft.Extensions.DependencyInjection;


namespace Logic.ConfigureServices
{
    public static class ServiceExtention
    {
        public static IServiceCollection ConfigureHashingServices(this IServiceCollection services)
        {
            return services.AddTransient<IHashingService, HashingService>();
        }
    }
}
