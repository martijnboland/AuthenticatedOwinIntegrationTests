using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(AuthenticatedOwinIntegrationTests.Startup))]
namespace AuthenticatedOwinIntegrationTests
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);

            // Owin Middleware
            // We use token middleware for requests that require authorization.
            var oAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(oAuthBearerOptions);
            app.UseWebApi(config);
        }
    }
}
