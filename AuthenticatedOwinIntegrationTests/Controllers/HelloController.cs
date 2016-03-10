using System.Web.Http;

namespace AuthenticatedOwinIntegrationTests.Controllers
{
    public class HelloController : ApiController
    {
        [HttpGet]
        [Route("hello")]
        public IHttpActionResult Hello()
        {
            return Ok("hello");
        }
    }
}
