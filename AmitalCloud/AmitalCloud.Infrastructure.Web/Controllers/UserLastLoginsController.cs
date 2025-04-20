using AmitalCloud.Infrastructure.Application.EntityUpdateServices;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.Helpers;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    public class UserLastLoginsController : ApiController
    {
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

        public HttpResponseMessage Put(UserLastLoginPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
                    AmitalCloudSecurityUtility.AuthenticationOnEntityTenant("UserLastLogin", entityPM.Tenant, tenant);
                    entityPM.ComputerId = entityPM.ComputerId;
                    entityPM.WorkEnvironment = GetWorkEnvironment();
                    new UserLastLoginUpdateService(0).Update(entityPM, true);
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);


                    //using (TransactionScope scope = TransactionFactory.GetTransaction())
                    //{
                    //    UserLastLoginRepository repository = new UserLastLoginRepository(AmitalCloudContext.GetContext(entityPM.Tenant));
                    //    // todo:
                    //    //bool isDSVMobileCall = false;
                    //    //using (TransactionScope globalScope = TransactionFactory.GetNewTransaction())
                    //    //{
                    //    //    string Url = HttpContext.Current.Request.UrlReferrer.ToString();
                    //    //    GlobalTenantRepository globalTenantRepository = new GlobalTenantRepository();
                    //    //    GlobalTenant globalTenant = globalTenantRepository.GetGlobalTenantsByTenant(entityPM.Tenant);
                    //    //    isDSVMobileCall = !string.IsNullOrEmpty(globalTenant.PrivateLabelId) && HttpContext.Current.Request.Browser.IsMobileDevice && Url.Contains("Menu=DAPP");
                    //    //}
                    //    //if (!isDSVMobileCall)
                    //    //{
                    //    UserLastLogin entity = repository.GetSingleUserLastLogin(entityPM.Id, entityPM.Tenant, false);
                    //    entity.ComputerId = entityPM.ComputerId;
                    //    entity.WorkEnvironment = GetWorkEnvironment();
                    //    repository.Update(entity);
                    //    repository.SubmitChanges();
                    //    //}
                    //    scope.Complete();
                    //    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    //}
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
            if (string.IsNullOrEmpty(AmitalCloudSettings.WorkEnvironment)) return "Amital";
            return SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Logbox) ? HttpContext.Current.Request.Url.Host.ToLower().Contains(".logbox.") ? "logbox" : "privatelabel" : AmitalCloudSettings.WorkEnvironment;

        }
    }
}