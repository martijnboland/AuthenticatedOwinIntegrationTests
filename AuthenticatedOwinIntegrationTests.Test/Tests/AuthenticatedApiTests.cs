using AuthenticatedOwinIntegrationTests.Models;
using System.Net;
using System.Net.Http;
using Xunit;

namespace AuthenticatedOwinIntegrationTests.Test.Tests
{
    public class AuthenticatedApiTests : BaseAuthenticatedApiTestFixture
    {
        private string _uri;

        protected override string Uri
        {
            get { return _uri; }
        }

        [Fact]
        public async void Get_UserInfo_Returns_200_And_UserInfo()
        {
            // Arrange
            _uri = "userinfo";

            // Act
            var response = await GetAsync();
            var result = await response.Content.ReadAsAsync<UserInfoDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(result.Name, "WebApiUser");
            Assert.Equal(2, result.Roles.Length);
        }

        [Fact]
        public async void Get_PowerUserHello_Returns_200_And_UserInfo()
        {
            // Arrange
            _uri = "poweruserhello";

            // Act
            var response = await GetAsync();
            var result = await response.Content.ReadAsAsync<string>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("hello poweruser", result);
        }
    }
}
