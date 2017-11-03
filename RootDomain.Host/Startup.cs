using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Net;
using System.Web.Http;
using RootDomain.Host.App_Start;

[assembly: OwinStartup(typeof(RootDomain.Host.Startup))]

namespace RootDomain.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(new WebApiConfig());//added support for webapi

            //ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;  //Disable Invalid Certificates in Development

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
