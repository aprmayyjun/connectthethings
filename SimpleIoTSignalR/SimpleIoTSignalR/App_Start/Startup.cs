using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SimpleIoTSignalR.App_Start.Startup))]
namespace SimpleIoTSignalR.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }

    }
}
