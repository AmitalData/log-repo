using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Transactions;
using System.Web;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Web.Helpers;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserLastLoginsController : ControllerBase
    {
        [HttpGet("{userId?}/{tenant?}")]
        public HttpResponseMessage GetUserLastLogin(string userId, int tenant)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
                return Request.CreateResponse(HttpStatusCode.OK, new UserLastLoginQuery(tenant).GetSinglePM(userId, tenant));
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPut("{entityPM?}")]
        public HttpResponseMessage Put(UserLastLoginPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        int tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
                        AmitalCloudSecurityUtility.AuthenticationOnEntityTenant("UserLastLogin", entityPM.Tenant, tenant);
                        UserLastLoginRepository repository = new UserLastLoginRepository(AmitalCloudContext.GetContext(entityPM.Tenant));
                        // todo:
                        //bool isDSVMobileCall = false;
                        //using (TransactionScope globalScope = TransactionFactory.GetNewTransaction())
                        //{
                        //    string Url = HttpContext.Current.Request.UrlReferrer.ToString();
                        //    GlobalTenantRepository globalTenantRepository = new GlobalTenantRepository();
                        //    GlobalTenant globalTenant = globalTenantRepository.GetGlobalTenantsByTenant(entityPM.Tenant);
                        //    isDSVMobileCall = !string.IsNullOrEmpty(globalTenant.PrivateLabelId) && HttpContext.Current.Request.Browser.IsMobileDevice && Url.Contains("Menu=DAPP");
                        //}
                        //if (!isDSVMobileCall)
                        //{
                        UserLastLogin entity = repository.GetSingleUserLastLogin(entityPM.Id, entityPM.Tenant, false);
                        entity.ComputerId = entityPM.ComputerId;
                        entity.WorkEnvironment = GetWorkEnvironment();
                        repository.Update(entity);
                        repository.SubmitChanges();
                        //}
                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        private string GetWorkEnvironment()
        {
            if (string.IsNullOrEmpty(AmitalCloudSettings.WorkEnvironment)) return "logitude";
            return SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Logbox) ? HttpContext.Current.Request.Url.Host.ToLower().Contains(".logbox.") ? "logbox" : "privatelabel" : AmitalCloudSettings.WorkEnvironment;

        }
    }
}