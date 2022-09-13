using AppContext.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppContext.Extentions
{
    public static class AppContextExtentions
    {
        public static TService Create<TService>(this IApplicationContext appCtx)
        {
            var ans = typeof(TService);
            var service = appCtx.SessionService.ServiceProvider.GetService(typeof(TService));
            if (service == null)
            {
                throw new Exception($"Service {typeof(TService)} not found");
            }
            return (TService)service;

        }

         

    }
}
