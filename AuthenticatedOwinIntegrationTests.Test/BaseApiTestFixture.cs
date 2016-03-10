using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Testing;

namespace AuthenticatedOwinIntegrationTests.Test
{
    public abstract class BaseApiTestFixture : IDisposable
    {
        private TestServer Server { get; set; }

        protected HttpClient HttpsClient
        {
            get
            {
                var client = new HttpClient(Server.Handler) { BaseAddress = new Uri("https://localhost:12345") };
                return client;
            }
        }

        protected abstract string Uri { get; }

        protected string UriParameters { get; set; }

        protected string QueryString { get; set; }

        protected IDataProtector DataProtector { get; set; }

        protected BaseApiTestFixture()
        {
            // Normally you'd create the server with:
            //
            //    Server = TestServer.Create<Startup>();
            //
            // but in this case we need to get hold of a DataProtector that can be 
            // used to generate compatible OAuth tokens.

            Server = TestServer.Create(app =>
            {
                var apiStartup = new Startup();
                apiStartup.Configuration(app);
                DataProtector = app.CreateDataProtector(typeof(OAuthAuthorizationServerMiddleware).Namespace, "Access_Token", "v1");
            });
            AfterServerSetup();
        }

        protected virtual async Task<HttpResponseMessage> GetAsync()
        {
            return await HttpsClient.SendAsync(CreateRequest(HttpMethod.Get, null));
        }

        protected virtual async Task<TResult> GetAsync<TResult>()
        {
            var response = await GetAsync();
            return await response.Content.ReadAsAsync<TResult>();
        }

        protected virtual async Task<HttpResponseMessage> PostAsync<TModel>(TModel model)
        {
            return await HttpsClient.SendAsync(CreateRequest(HttpMethod.Post, model));
        }

        protected virtual async Task<HttpResponseMessage> PutAsync<TModel>(TModel model)
        {
            return await HttpsClient.SendAsync(CreateRequest(HttpMethod.Put, model));
        }

        protected virtual async Task<HttpResponseMessage> DeleteAsync()
        {
            return await HttpsClient.SendAsync(CreateRequest(HttpMethod.Delete, null));
        }

        protected virtual HttpRequestMessage CreateRequest(HttpMethod method, object data)
        {
            var request = new HttpRequestMessage(method, Uri + UriParameters + QueryString);
            if (data != null)
            {
                request.Content = new ObjectContent(data.GetType(), data, new JsonMediaTypeFormatter());
            }

            Console.WriteLine("Created request: {0} {1} {2} {3} {4} ", method, HttpsClient.BaseAddress, Uri, UriParameters, QueryString);

            return request;
        }

        protected virtual void AfterServerSetup()
        {
            // Nothing
        }

        public virtual void Dispose()
        {
            if (Server != null)
            {
                Server.Dispose();
            }
        }
    }
}
