﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RootDomain.Host
{
    public static class WebApiConfig
    {
        internal static void Config(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
