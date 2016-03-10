using AuthenticatedOwinIntegrationTests.Models;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace AuthenticatedOwinIntegrationTests.Controllers
{
    public class AuthenticatedController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("userinfo")]
        public IHttpActionResult GetUserInfo()
        {
            var currentPrincipal = Request.GetOwinContext().Authentication.User;

            var userInfo = new UserInfoDto
            {
                Name = currentPrincipal.Identity.Name,
                Roles = currentPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value ).ToArray()
            };
            return Ok(userInfo);
        }

        [HttpGet]
        [Authorize(Roles = "PowerUser")]
        [Route("poweruserhello")]
        public IHttpActionResult GetPowerUserHello()
        {
            return Ok("hello poweruser");
        }
    }
}
