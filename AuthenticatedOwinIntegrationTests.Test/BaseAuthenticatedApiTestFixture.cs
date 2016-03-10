using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using System;
using System.Net.Http;
using System.Security.Claims;

namespace AuthenticatedOwinIntegrationTests.Test
{
    /// <summary>
    /// Base class for integration tests that require authentication.
    /// </summary>
    public abstract class BaseAuthenticatedApiTestFixture : BaseApiTestFixture
    {
        private string _token;

        /// <summary>
        /// Token for authenticated requests.
        /// </summary>
        protected virtual string Token
        {
            get { return _token ?? (_token = GenerateToken()); }
        }

        protected override HttpRequestMessage CreateRequest(HttpMethod method, object data)
        {
            var request = base.CreateRequest(method, data);
            if (!String.IsNullOrEmpty(this.Token))
            {
                request.Headers.Add("Authorization", "Bearer " + this.Token);
            }
            return request;
        }

        private string GenerateToken()
        {
            // Generate an OAuth bearer token for ASP.NET/Owin Web Api service that uses the default OAuthBearer token middleware.
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "WebApiUser"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.Role, "PowerUser"),
            };
            var identity = new ClaimsIdentity(claims, "Test");

            // Use the same token generation logic as the OAuthBearer Owin middleware. 
            var tdf = new TicketDataFormat(this.DataProtector);
            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddHours(1) });
            var accessToken = tdf.Protect(ticket);

            return accessToken;
        }
    }
}
