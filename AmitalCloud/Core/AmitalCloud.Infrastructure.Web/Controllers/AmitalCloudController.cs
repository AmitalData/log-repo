using AmitalCloud.Infrastructure.Web.Helpers;
using Microsoft.AspNetCore.Mvc;


using AmitalCloud.Infrastructure.Application.Helpers;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AmitalCloudController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCheckIsupgradingSystem()
        {
            try
            {
                return Ok(Authentication.GetIsBlockingFromDB(HttpContext));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }



    }
}