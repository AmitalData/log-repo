using AmitalCloud.Infrastructure.Application.EntityQueryServices;
 using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Data.Security;
 using AmitalCloud.Infrastructure.Application.Helpers;
 using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
 using AmitalCloud.Infrastructure.Model.EntityClasses;
 using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class GlobalDomainController : ApiController
    {
        public HttpResponseMessage GetAccountingSystem(string AccountingSystemCode)
        {
            try
            {
                AccountingSystemPM result = !string.IsNullOrEmpty(AccountingSystemCode) && AccountingSystemCode != "null" ? new AccountingSystemQueryService(AmitalCloudSecurityUtility.AuthenticateTenant()).GetSingle(AccountingSystemCode, true, true) : null;
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCheckInttraAddsOn()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new TenantAddOnQueryService(AmitalCloudSecurityUtility.AuthenticateTenant()).GetSingle("INTTR", true, true));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetGlobalSetting()
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
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetPrivateLableById(string Id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new TenantManagmentPrivateLabelsQueryService(AmitalCloudSecurityUtility.AuthenticateTenant()).GetSingle(Id, true, true));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTenantSetting()
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                return Request.CreateResponse(HttpStatusCode.OK, new TenantSettingQueryService(tenant).GetMulti(a => a.Tenant == tenant));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTenantManagementJS(string loggeduserid)
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                TenantManagementPM entityPM = new TenantManagementQueryService(tenant).GetSingle(tenant, true, true);
                bool isLogboxSystem = CheckIsLogboxSystem();
                if (entityPM == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new TenantManagementJS(new TenantManagementPM()));
                }
                TenantManagementJS myResult = new TenantManagementJS(entityPM)
                {
                };

                if (entityPM.PaymentFailure)
                {
                    if (entityPM.SuspendDate.Value.Date < DateTime.Now.Date)
                    {
                        myResult.DoBlocking = true;
                        myResult.BlockType = "suspend";
                    }
                    else if (entityPM.SuspendDate.Value.Date == DateTime.Now.Date)
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
                    if (entityPM.TrialEndDate.Value.Date < DateTime.Now.Date)
                    {
                        myResult.DoBlocking = true;
                        myResult.BlockType = "company";
                    }
                    else if (entityPM.TrialEndDate.Value.Date == DateTime.Now.Date)
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

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
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