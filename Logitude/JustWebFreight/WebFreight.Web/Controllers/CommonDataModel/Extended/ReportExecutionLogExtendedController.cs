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
using Simplog.Data.CommonDataModel.EntityPOCOs;
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


    }
}