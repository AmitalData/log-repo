using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GlobalDomainController : ControllerBase
    {
        [HttpGet("{AccountingSystemCode?}")]
        public HttpResponseMessage GetAccountingSystem(string AccountingSystemCode)
        {
            try
            {
                AccountingSystemPM result = !string.IsNullOrEmpty(AccountingSystemCode) && AccountingSystemCode != "null" ? new AccountingSystemQueryService(AmitalCloudSecurityUtility.AuthenticationOnTenant()).GetSingle(AccountingSystemCode, true, true) : null;
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCheckInttraAddsOn()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new TenantAddOnQueryService(AmitalCloudSecurityUtility.AuthenticationOnTenant()).GetSingle("INTTR", true, true));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage GetGlobalSetting()
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
                JSGlobalSettings myResult;
                //todo: 
                //SettingRepository mySettingRepository = new SettingRepository();
                Setting mySetting = null;// mySettingRepository.GetSingleSetting("1");
                if (mySetting == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new JSGlobalSettings(new Setting()));
                }
                myResult = new JSGlobalSettings(mySetting);
                if (AmitalCloudSettings.IsCostomsDeploy)
                {
                    myResult.ProductInfo = AmitalCloudSettings.ProductInfo;//.Replace(Environment.NewLine ,"<br>") ;
                    myResult.ProductMessage = AmitalCloudSettings.ProductMessage;
                }
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet("{Id?}")]
        public HttpResponseMessage GetPrivateLableById(string Id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new TenantManagmentPrivateLabelsQueryService(AmitalCloudSecurityUtility.AuthenticationOnTenant()).GetSingle(Id, true, true));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTenantSetting()
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
                return Request.CreateResponse(HttpStatusCode.OK, new TenantSettingQueryService(tenant).GetMulti(a => a.Tenant == tenant, "ObjectTable").ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet("{loggeduserid?}")]
        public HttpResponseMessage GetTenantManagementJS(string loggeduserid)
        {
            try
            {
                int tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
                TenantManagementPM entityPM = new TenantManagementQueryService(tenant).GetSingle(tenant, true, true);
                //bool isLogboxSystem = CheckIsLogboxSystem();
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
    }
}