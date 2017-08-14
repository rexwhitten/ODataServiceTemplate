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
using System.IdentityModel.Tokens;
using System.Configuration;

namespace RootDomain.Host.App_Start
{
    public class SimConfig
    {
        internal static void Config(IAppBuilder app, HttpConfiguration config)
        {
            var issuer = ConfigurationManager.AppSettings["SIM:Issuer"];
            JwtSecurityTokenHandler.InboundClaimTypeMap.Clear();
            app.UseClaimsTransformation(incoming =>
            {
                var appPrincipal = new ClaimsPrincipal(incoming);
                return Task.FromResult(appPrincipal);
            });

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = issuer,
                RequiredScopes = new[] { "openid", "policy_service" }, //need to rename policy_service to newly created name from SIM side
                ValidationMode = ValidationMode.ValidationEndpoint, //ValiationMode.Local -> this will prevent it from token revocation
                RoleClaimType = "role",
                NameClaimType = "preferred_username"
            });
        }
    }
}