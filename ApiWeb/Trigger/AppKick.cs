using ApiWeb.MiddleWare;
using ApiWeb.Service.EnvironmentService;
namespace ApiWeb.Trigger
{
    public class AppKick
    {
        public AppKick(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                EnvironmentService e = new()
                { 
                    IsDevelopment = true
                };
            }
            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<TokenManager>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            app.Run();
        }
    }
}
