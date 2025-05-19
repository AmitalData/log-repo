using AmitalCloud.Infrastructure.Application.EntityQueryServices;
 using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Web.Helpers;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GlobalDomainController : ControllerBase
    {
        [HttpGet("GetAccountingSystem")]
        public IActionResult GetAccountingSystem(string AccountingSystemCode)
        {
            try
            {
                AccountingSystemPM? result = !string.IsNullOrEmpty(AccountingSystemCode) && AccountingSystemCode != "null" ? new AccountingSystemQueryService(AmitalCloudSecurityUtility.AuthenticateTenant()).GetSingle(AccountingSystemCode, true, true) : null;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet("GetCheckInttraAddsOn")]
        public IActionResult GetCheckInttraAddsOn()
        {
            try
            {
                var tenantAddOn = new TenantAddOnQueryService(AmitalCloudSecurityUtility.AuthenticateTenant()).GetSingle("INTTR", true, true);
                return Ok(tenantAddOn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet("GetGlobalSetting")]
        public IActionResult GetGlobalSetting()
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                SettingQueryService settingQueryService = new SettingQueryService(tenant);

                SettingPM mySetting = settingQueryService.GetSingle(SettingQuery.GetDefaultSettingId(), false, true);

                JSGlobalSettings myResult = new JSGlobalSettings(mySetting);
                if (AmitalCloudSettings.IsCostomsDeploy)
                {
                    myResult.ProductInfo = AmitalCloudSettings.ProductInfo;
                    myResult.ProductMessage = AmitalCloudSettings.ProductMessage;
                }
                return Ok(myResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet("GetPrivateLableById")]
        public IActionResult GetPrivateLableById(string Id)
        {
            try
            {
                var tenantManagementPrivateLabels = new TenantManagmentPrivateLabelsQueryService(AmitalCloudSecurityUtility.AuthenticateTenant()).GetSingle(Id, true, true);
                return Ok(tenantManagementPrivateLabels);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

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

        [HttpGet("GetTenantManagementJS")]
        public IActionResult GetTenantManagementJS(string loggeduserid)
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                TenantManagementPM entityPM = new TenantManagementQueryService(tenant).GetSingle(tenant, true, true);
                bool isLogboxSystem = CheckIsLogboxSystem();
                if (entityPM == null)
                {
                    return Ok(new TenantManagementJS(new TenantManagementPM()));
                }
                TenantManagementJS myResult = new TenantManagementJS(entityPM)
                {
                };

                if (entityPM.PaymentFailure)
                {
                    if (entityPM.SuspendDate != null && entityPM.SuspendDate.Value.Date < DateTime.Now.Date)
                    {
                        myResult.DoBlocking = true;
                        myResult.BlockType = "suspend";
                    }
                    else if (entityPM.SuspendDate != null && entityPM.SuspendDate.Value.Date == DateTime.Now.Date)
                    {
                        myResult.SuspendDaysLeft = 0;
                    }
                    else
                    {
                        myResult.SuspendDaysLeft = 100; // todo tenantManagementQuery.ComputeDaysLeft(entityPM.SuspendDate);
                    }
                }

                if (entityPM.IsTrial)
                {
                    if (entityPM.TrialEndDate != null && entityPM.TrialEndDate.Value.Date < DateTime.Now.Date)
                    {
                        myResult.DoBlocking = true;
                        myResult.BlockType = "company";
                    }
                    else if (entityPM.TrialEndDate != null && entityPM.TrialEndDate.Value.Date == DateTime.Now.Date)
                    {
                        myResult.TrailDaysLeft = 0;
                    }
                    else
                    {
                        myResult.TrailDaysLeft = 100; // todo  tenantManagementQuery.ComputeDaysLeft(entityPM.TrialEndDate);
                    }
                }
                else if (entityPM.PaidUntilDate != null)
                {
                    if (!entityPM.IsRecurring)
                    {
                        if ((entityPM.PaidUntilDate - DateTime.Now).Value.Days < 0)
                        {
                            myResult.DoBlocking = true;
                            myResult.BlockType = "company";
                        }
                        else
                        {
                            myResult.PaidDaysLeft = 100; // todo tenantManagementQuery.ComputeDaysLeft(entityPM.PaidUntilDate);
                        }
                    }
                }
                UserQueryService service = new UserQueryService(tenant);
                UserPM user = service.GetSingle(loggeduserid, true, true);
                if (user != null)
                {
                    myResult.ExpirationDate = user.ExpirationDate;
                    if (user.ExpirationDate != null)
                    {
                        if (user.ExpirationDate.Value.Date < DateTime.Now.Date)
                        {
                            myResult.DoBlocking = true;
                            myResult.BlockType = "user";
                        }
                        else if (user.ExpirationDate.Value.Date == DateTime.Now.Date)
                        {
                            myResult.ExpirationDaysLeft = 0;
                        }
                        else
                        {
                            myResult.ExpirationDaysLeft = 100; // todo tenantManagementQuery.ComputeDaysLeft(user.ExpirationDate);
                        }
                    }
                }

                return Ok(myResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        private static bool CheckIsLogboxSystem()
        {
            bool isLogboxSystem = false;
            string url = AmitalCloudSecurityUtility.getLoggedDomain();
            if (url.Contains("system.logbox.co.il") || url.Contains("pre.logbox.co.il") || url.Contains("test.logitudeworld.com")) //Test env acts Like Logbox
                isLogboxSystem = true;
            return isLogboxSystem;
        }
    }
}