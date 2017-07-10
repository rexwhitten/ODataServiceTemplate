using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Owin;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using System.Configuration;

namespace RootDomain.Host.App_Start
{
    public class ODataConfig
    {
        internal static void Config(IAppBuilder app, HttpConfiguration config)
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            // Root Domain Model
            // builder.EntitySet<WorkflowTask>("WorkflowTask");
            config.Routes.MapODataServiceRoute(ConfigurationManager.AppSettings["odata:serviceRoute:name"],
                                               ConfigurationManager.AppSettings["odata:serviceRoute:prefix"],
                                               builder.GetEdmModel()
                                               );
        }
    }
}