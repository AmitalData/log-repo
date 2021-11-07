using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class UserLastLoginsController : ApiController
    {
        public HttpResponseMessage GetUserLastLogin(string userId, int tenant)
        {
            try
            {
                UserLastLoginQuery userLastLoginQuery = new UserLastLoginQuery(tenant);
                var myResult = userLastLoginQuery.GetSinglePM(userId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Put(UserLastLoginPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                        ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                        UserLastLoginRepository repository = new UserLastLoginRepository(MyContext);
                        //http://digitaltest.dsv.co.il/test/?Menu=DAPP&ForwarderShipmentNumber=4170400334select


                        //if (!url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
                        bool isDSVMobileCall = false;
                        using (TransactionScope globalScope = TransactionFactory.GetNewTransaction())
                        {
                            string Url = HttpContext.Current.Request.UrlReferrer.ToString();
                            //string Url = HttpContext.Current.Request.Url.ToString();
                            GlobalTenantRepository globalTenantRepository = new GlobalTenantRepository();
                            GlobalTenant globalTenant = globalTenantRepository.GetGlobalTenantsByTenant(entityPM.Tenant);
                            isDSVMobileCall = !string.IsNullOrEmpty(globalTenant.PrivateLabelId) && HttpContext.Current.Request.Browser.IsMobileDevice && Url.Contains("Menu=DAPP");
                        }
                        if (!isDSVMobileCall)
                        {
                            UserLastLogin entity = repository.GetSingleUserLastLogin(entityPM.Id, entityPM.Tenant, false);
                            entity.ComputerId = entityPM.ComputerId;
                            entity.WorkEnvironment = LogitudeSettingConfigration.GetWorkEnvironment();
                            repository.Update(entity);
                            repository.SubmitChanges();
                        }

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
    }

}