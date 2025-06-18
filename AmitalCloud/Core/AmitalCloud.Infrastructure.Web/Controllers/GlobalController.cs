using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GlobalController : ControllerBase
    {


        [HttpGet("GetTenantSetting")]
        public IActionResult GetTenantSetting()
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                var tenantSettings = new TenantSettingQueryService(tenant).GetMulti(a => a.Tenant == tenant);
                return Ok(tenantSettings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet("GetTenantManagementStatus")]
        public IActionResult GetTenantManagementStatus(string loggeduserid)
        {

            try
            {
                int tenantId = AmitalCloudSecurityUtility.AuthenticateTenant();
                var tenantStatus = new TenantManagementQueryService(tenantId).GetTenantStatusPM(tenantId, loggeduserid);

                return Ok(tenantStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }




    }
}