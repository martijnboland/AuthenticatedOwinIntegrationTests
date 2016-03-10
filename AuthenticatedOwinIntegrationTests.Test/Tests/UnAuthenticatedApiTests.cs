using System.Net;
using Xunit;

namespace AuthenticatedOwinIntegrationTests.Test.Tests
{
    public class UnAuthenticatedApiTests : BaseApiTestFixture
    {
        protected override string Uri
        {
            get { return "userinfo"; }
        }

        [Fact]
        public async void Get_UserInfo_Returns_401()
        {
            // Arrange

            // Act
            var response = await GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
