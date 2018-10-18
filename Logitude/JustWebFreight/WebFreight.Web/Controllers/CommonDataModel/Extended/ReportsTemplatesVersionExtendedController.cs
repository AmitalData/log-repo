using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
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

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class ReportsTemplatesVersionExtendedController : ApiController
    {
        public HttpResponseMessage GetReportsTemplatesVersionListsByReportTemplateId(string reportTemplateId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                ReportsTemplatesVersionQuery reportsTemplatesVersionQuery = new ReportsTemplatesVersionQuery(tenant);
                List<ReportsTemplatesVersionList> myResult = reportsTemplatesVersionQuery.GetReportsTemplatesVersionListsByTenant(reportTemplateId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetRestoreReportsTemplatesVersion(string reportsTemplatesVersionId, string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                ReportHelper reportHelper = new ReportHelper();
                ReportsTemplatesVersion reportsTemplatesVersion =  reportHelper.RestoreReportsTemplatesVersion(reportsTemplatesVersionId, userId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, reportsTemplatesVersion);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}