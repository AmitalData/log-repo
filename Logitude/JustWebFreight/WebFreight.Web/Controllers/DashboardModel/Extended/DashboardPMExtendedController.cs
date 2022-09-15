using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.BL.EntityQueryServices;
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

namespace WebFreight.Web.Controllers.DashboardModel.Extended
{
    public class DashboardPMExtendedController : ApiController
    {
        public HttpResponseMessage GetDashboardPMs()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);                

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Dashboard", "READ", authToken.Tenant);

                string loggedContactId = this.GetLoggedContactId(authToken.Email, authToken.Tenant);

                DashboardQueryService dashboardQueryService = new DashboardQueryService(authToken.Tenant);
                IQueryable<DashboardPM> myResult = dashboardQueryService.GetDashboardPMs(authToken.Tenant, loggedContactId);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private string GetLoggedContactId(string email, int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            string loggedContactId = contactRepository.GetConactIdByemail(email, tenant);

            if (string.IsNullOrEmpty(loggedContactId)) 
                loggedContactId = contactRepository.GetConactIdByemail(email, 0);

            return loggedContactId;
        }
    }
}