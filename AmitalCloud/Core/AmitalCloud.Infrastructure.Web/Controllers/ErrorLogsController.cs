using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Model.EntityClasses;
using AmitalCloud.Infrastructure.Application.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorLogsController : ControllerBase
    {
        [HttpPost]
        [Route("")]
        public IActionResult Post(ErrorLog entity)
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant(entity.Tenant);
                entity = new ErrorLogQuery(tenant).AddErrorLog(entity);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}