using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAuth.Authorization;

namespace TestAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        [Route("PublicAction")]
        public async Task<IActionResult> PublicAction()
        {
            return Ok();
        }

        [HttpGet]
        [Route("PrivateAction")]
        [CustomAuthorize]
        public async Task<IActionResult> PrivateAction()
        {
            return Ok();
        }
    }
}
