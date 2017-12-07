using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Net;
using System.Web.Http;
using RootDomain.App_Start;

[assembly: OwinStartup(typeof(RootDomain.Startup))]

namespace RootDomain
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            LogConfig.Config(app, config);
            SimConfig.Config(app, config);
            MiddlewareConfig.Config(app, config);
            WebApiConfig.Config(config);
            ODataConfig.Config(app, config);

            app.UseWebApi(config);
            config.EnsureInitialized();
        }
    }
}
