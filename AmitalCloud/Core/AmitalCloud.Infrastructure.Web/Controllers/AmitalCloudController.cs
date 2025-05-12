using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

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
                return Ok(GetIsBlockingFromDB());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        private bool GetIsBlockingFromDB()
        {
            var result = new GlobalDBQueryService(0).GetMulti(a => true); // a.IsBlocking == true); //,a=>new GlobalDBPM() {Id = a.Id });
            bool isBlocking = result.Count > 0;
            string[]? authenticatedIPs = AmitalCloudSettings.CustomerCareIP?.Split(',');
            string? currentIP = HttpContext.Request.Headers["X-Real-IP"];
            if (string.IsNullOrEmpty(currentIP))
            {
                currentIP = HttpContext.Connection.RemoteIpAddress?.ToString();
            }
            bool isIpAuthenticated = authenticatedIPs != null && authenticatedIPs.Contains(currentIP);
            if (isIpAuthenticated)
            {
                isBlocking = false;
            }
            return isBlocking;
        }
    }
}