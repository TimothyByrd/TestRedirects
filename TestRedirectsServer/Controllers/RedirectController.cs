using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestRedirectsServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedirectController : ControllerBase
    {
        private readonly ILogger<RedirectController> _logger;

        public RedirectController(ILogger<RedirectController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Given a desired response status code provide that response,
        /// along with a Location header to the final path
        /// </summary>
        /// <param name="statusCode">The desired response status code - e.g.: 300, 301, 302, 303</param>
        [HttpDelete("initial/{statusCode}")]
        [HttpGet("initial/{statusCode}")]
        [HttpHead("initial/{statusCode}")]
        [HttpOptions("initial/{statusCode}")]
        [HttpPatch("initial/{statusCode}")]
        [HttpPost("initial/{statusCode}")]
        [HttpPut("initial/{statusCode}")]
        public IActionResult Initial(int statusCode)
        {
            _logger.LogInformation($"initial {HttpContext.Request.Method} {statusCode}");
            var location = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}/Redirect/final/{statusCode}";
            Response.Headers.Add("Location", location);
            return new StatusCodeResult(statusCode);
        }

        /// <summary>
        /// Given a status return that value as a string
        /// This call will succeed (200).
        /// </summary>
        /// <param name="statusCode">The code to return</param>
        [HttpDelete("final/{statusCode}")]
        [HttpGet("final/{statusCode}")]
        [HttpHead("final/{statusCode}")]
        [HttpOptions("final/{statusCode}")]
        [HttpPatch("final/{statusCode}")]
        [HttpPost("final/{statusCode}")]
        [HttpPut("final/{statusCode}")]
        public ActionResult<string> Final(int statusCode)
        {
            var msg = $"final {HttpContext.Request.Method} {statusCode}";
            _logger.LogInformation(msg);
            return msg;
        }
    }
}
