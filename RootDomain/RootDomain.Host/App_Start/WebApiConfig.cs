using Elmah.Contrib.WebApi;
using LightInject;
using RootDomain.Host.App_Start;
using RootDomain.Host.Middleware;
using RootDomain.Infrastructure.Middleware.Handlers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace RootDomain.Host
{
    public class WebApiConfig : HttpConfiguration
    {
        public WebApiConfig()
        {
            //default contructor..
        }

        internal static void Config(HttpConfiguration config)
        {
            // enable elmah
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());

            //Register LightInject container...
            var container = new ServiceContainer();
            container.RegisterApiControllers();
            // Register all the dependencies defined in composition root....
            container.RegisterFrom<CompositionRoot>();
            container.EnableWebApi(config);

            //Register the formatter
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));

            //configure to use logging message handler only when it is explicitly set based on circumstances.
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["service:UseLogManager"]))
            {
                //log HttpRequest and HttpResponse using Messagehandlers.
                config.MessageHandlers.Add(new AppLogHandler(ConfigurationManager.AppSettings["service:name"]));
            }

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                // constraint required so this route only matches valid controller names
                constraints: new { controller = GetControllerNames() }
            );
            // catch all route mapped to ErrorController so 404 errors
            // can be logged in elmah
            config.Routes.MapHttpRoute(
                name: "NotFound",
                routeTemplate: "api/{*path}",
                defaults: new { controller = "Error", action = "NotFound" }
            );
            config.Filters.Add(new ElmahHandleWebApiErrorAttribute());
        }

        // helper method that returns a string of all api controller names
        // in this solution, to be used in route constraints above
        private static string GetControllerNames()
        {
            var controllerNames = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(x =>
                    x.IsSubclassOf(typeof(ApiController)) &&
                    x.FullName.StartsWith(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ".Controllers"))
                .ToList()
                .Select(x => x.Name.Replace("Controller", ""));

            return string.Join("|", controllerNames);
        }
    }
}
