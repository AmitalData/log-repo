using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Web.DataContracts;
using AmitalCloud.Infrastructure.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationController(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public string? GetSettingsLoginCode(int myDummyInteger, string myDummyString)
        {
            var logoCode = new SettingQueryService(0).GetMulti(a => true, a => a.LogoCode).FirstOrDefault();
            return logoCode;
        }

        [HttpPost("PostUserValidation")]
        public IActionResult PostUserValidation(LoginParameters loginParameters)
        {
            try
            {
                Authentication authentication = new Authentication(loginParameters.Tenant, _httpContextAccessor, _memoryCache);
                UserData data = authentication.AuthenticateUser(loginParameters);
                return Ok(data);
            }
            catch (Exception ex)
            {
                LogException(ex, loginParameters.Email, "PostUserValidation");
                return StatusCode(StatusCodes.Status500InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        public IActionResult PostLoginData(LoginParameters parameters, int tenant, bool? isFromCTool = false)
        {
            try
            {
                Authentication authentication = new Authentication(tenant, _httpContextAccessor, _memoryCache);
                UserData user = authentication.LoginUser(parameters, tenant, isFromCTool);
                return Ok(user);
            }
            catch (Exception ex)
            {
                LogException(ex, parameters.Email, "PostLoginData", tenant);
                return StatusCode(StatusCodes.Status500InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        private void LogException(Exception ex, string email, string methodName, int tenant = 0)
        {
            ExceptionHandler.HandleException(ex, DateTime.Now, tenant, email, "", $"AuthenticationController : {methodName}", null);
            string message = ex.InnerException?.Message + Environment.NewLine + ex.Message;
            NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, message);
        }
    }
}