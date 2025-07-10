using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonDomainController : ControllerBase
    {
        [HttpGet("GetLoggedTenantDB")]
        public IActionResult GetLoggedTenantDB()
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                TenantPM myResult = new TenantQueryService(tenant).GetSingle(tenant, true, true);
                return Ok(myResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}