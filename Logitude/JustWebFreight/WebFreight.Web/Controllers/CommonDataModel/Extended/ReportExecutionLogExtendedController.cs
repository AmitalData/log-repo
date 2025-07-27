using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.AmitalMessaging.Utils;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Customs.BL.Messaging.Customs;
using System.IO;
using System.Net.Http.Headers;
using WebFreight.Web.CustomWebServices.BL.XLSExport;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using System.Data.SqlClient;
using System.Data;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class ReportExecutionLogExtendedController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                SecurityUtility.CheckContactFeature("ReportExecutionLog", "READ", authToken.Tenant);
                ReportExecutionLogQuery reportExecutionLogQuery = new ReportExecutionLogQuery(authToken.Tenant);
                ReportExecutionLogPM reportExecutionLogPM = reportExecutionLogQuery.GetSinglePM(id, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, reportExecutionLogPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostCancel(string reportId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                var tenant= authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                SecurityUtility.CheckContactFeature("ReportExecutionLog", "READ", tenant);
                ReportExecutionLogQuery reportExecutionLogQuery = new ReportExecutionLogQuery(authToken.Tenant);
                reportExecutionLogQuery.CancelStuckReports(authToken.Tenant, reportId);


                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostSendToBackground(string reportId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                var tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                SecurityUtility.CheckContactFeature("ReportExecutionLog", "READ", tenant);
                ReportExecutionLogQuery reportExecutionLogQuery = new ReportExecutionLogQuery(authToken.Tenant);
                reportExecutionLogQuery.SendReportToBackground(reportId, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}
