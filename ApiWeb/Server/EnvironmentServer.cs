﻿using ApiContext.Context.Interface;
using ApiWeb.Service.EnvironmentService;
using ApiWeb.Trigger;

using ApiContext.Context.Extentions;


namespace ApiWeb.Server
{
    public static class EnvironmentServer
    {
        //Hans configure nullables

        public static IEnvironmentService GetEnvironmentService()
        {
            var appCtx = GetCtx();
            return appCtx.Create<IEnvironmentService>();
        }

        public static IApplicationContext GetCtx()
        {
            var services = new ServiceCollection()
                .ConfigureEnvironmentService();
                 
            var provider = services.BuildServiceProvider();
            var appCtx = provider.GetService<IApplicationContext>();
            return appCtx;
        }
    }


}
