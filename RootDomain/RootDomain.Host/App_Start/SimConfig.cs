using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System.Text;
using Microsoft.AspNet.Identity;
using IdentityServer3.AccessTokenValidation;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RootDomain.Host.App_Start
{
    public class SimConfig
    {
        internal static void Config(IAppBuilder app, HttpConfiguration config)
        {
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(DefaultAuthenticationTypes.ExternalBearer));

            var issuer = "https://devapi.sompo-us.com/SimIdentity/core";
            //var clientId = "Javascript";
            //var clientSecret = TextEncodings.Base64Url.Decode("SmF2YXNjcmlwdDppZHNydjN0ZXN0");

            app.UseClaimsTransformation(incoming =>
            {
                var appPrincipal = new ClaimsPrincipal(incoming);
                return Task.FromResult(appPrincipal);
            });

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = issuer
            });
        }
    }
}