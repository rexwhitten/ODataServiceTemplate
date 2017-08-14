using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Owin;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Configuration;


namespace RootDomain.Host.App_Start
{
    public class ODataConfig
    {
        internal static void Config(IAppBuilder app, HttpConfiguration config)
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            // To enable OData filter to work from client side..
            config.Select().Expand().Filter().OrderBy().MaxTop(null).Count();
            // Root Domain Model
            // builder.EntitySet<WorkflowTask>("WorkflowTask");
            config.MapODataServiceRoute(ConfigurationManager.AppSettings["odata:serviceRoute:name"],
                                               ConfigurationManager.AppSettings["odata:serviceRoute:prefix"],
                                               builder.GetEdmModel()
                                               );
        }
    }
}