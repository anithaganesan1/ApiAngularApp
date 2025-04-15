using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ApiAngularApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionhandlerController : ControllerBase
    {
    [HttpGet("No Exception")]
    public ActionResult NoException()      
     {  return Ok("Real time example");
     }
    [HttpGet("NotImplementedException")]
    public ActionResult GetNotImplementedException()
        {
            throw new NotImplementedException();
            return Ok();

        }
    [HttpGet ("TimeoutException")]
    public ActionResult GetTimeoutException()
        {
            throw new TimeoutException();
            return Ok();
        }
    }
}
