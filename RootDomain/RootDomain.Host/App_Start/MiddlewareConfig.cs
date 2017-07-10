using Microsoft.Owin;
using Owin;
using RootDomain.Host.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace RootDomain.Host.App_Start
{
    public class MiddlewareConfig
    {
        internal static void Config(IAppBuilder app, HttpConfiguration config)
        {
            app.Use<ExceptionHandlerMiddleware>();
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWelcomePage(new PathString("/"));
        }
    }
}