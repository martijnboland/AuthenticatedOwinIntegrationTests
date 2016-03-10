using System.Net;
using System.Net.Http;
using Xunit;

namespace AuthenticatedOwinIntegrationTests.Test.Tests
{
    public class ApiTests : BaseApiTestFixture
    {
        protected override string Uri
        {
            get { return "hello"; }
        }

        [Fact]
        public async void Get_Hello_Returns_200_And_Hello()
        {
            // Arrange

            // Act
            var response = await GetAsync();
            var result = await response.Content.ReadAsAsync<string>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("hello", result);
        }
    }
}
