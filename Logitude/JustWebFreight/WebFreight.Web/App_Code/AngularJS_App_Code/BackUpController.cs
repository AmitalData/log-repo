using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class BackUpController : ApiController
    {
        public HttpResponseMessage GetBackUpForClientData(int tenant, bool s)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                QueueMessagesWebHelper queueMessagesWebHelper = new QueueMessagesWebHelper();

                queueMessagesWebHelper.BackUpForClientData(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetCheckIfDatabaseBackupIsBuilt(string a, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            TenantRepository tenantRepository = new TenantRepository(tenant);
            return Request.CreateResponse(HttpStatusCode.OK, tenantRepository.CheckIfDatabaseBackupBuilt(tenant));

        }


        public HttpResponseMessage GetSetDatabaseDataBackupNotReady(int id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                TenantRepository tenantRepository = new TenantRepository(id);
                Tenant tenant = tenantRepository.GetSingleTenantWithOutIncluded(id);
                tenant.IsDataBackupBuilt = false;
                tenantRepository.Update(tenant);
                tenantRepository.SubmitChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}