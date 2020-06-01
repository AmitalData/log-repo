using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class InterestReportController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetDraftInterestReportsByDates(DateTime fromDate, DateTime toDate)
        {
            try
            {
                int tenant = AuthinticateTenant();          
                var accountingContext = AccountingContext.GetContext(tenant);
             
                ServiceResponse response = new ServiceResponse();
              InterestReportListQueryService 

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        private static int AuthinticateTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("InterestReport", "READ", authToken.Tenant);
            int tenant = authToken.Tenant;
            return tenant;
        }
    }
}