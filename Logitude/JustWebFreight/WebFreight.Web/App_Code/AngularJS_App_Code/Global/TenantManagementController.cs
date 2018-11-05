using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
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

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Global
{
    public class TenantManagementController : ApiController
    {
        public HttpResponseMessage GetSingleTenantManagementPM(int id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                bool isAuthentication = id == authToken.Tenant ? true : false;
                if (!isAuthentication)
                {
                    isAuthentication = SecurityUtility.CheckIsUserCustomerCare(authToken.Email);
                }

                if (isAuthentication)
                {
                   // SecurityUtility.CheckContactFeature("TenantManagement", "READ", authToken.Tenant);
                    TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(id);
                    TenantManagementPM entityPM = tenantManagementQuery.GetSinglePM(id);

                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
                else
                {
                    throw new Exception("Sorry you’re not authenticated to view company info.");
                }

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}