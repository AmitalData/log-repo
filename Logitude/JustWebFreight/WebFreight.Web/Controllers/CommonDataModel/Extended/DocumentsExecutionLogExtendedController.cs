using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class DocumentsExecutionLogExtendedController : ApiController
    {
        public HttpResponseMessage GetDocumentsExecutionLogList(string id)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                DocumentsExecutionLogList documentsExecutionLogList = GetDocumentsExecutionLogStatusList(id, authToken);
                return Request.CreateResponse(HttpStatusCode.OK, documentsExecutionLogList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static DocumentsExecutionLogList GetDocumentsExecutionLogStatusList(string id, AuthenticationToken authToken)
        {
            DocumentsExecutionLogQuery documentsExecutionLogQuery = new DocumentsExecutionLogQuery(authToken.Tenant);
            DocumentsExecutionLogList documentsExecutionLogList = documentsExecutionLogQuery.GetDocumentsExecutionLogList(id, authToken.Tenant);
            if (documentsExecutionLogList != null && (documentsExecutionLogList.StatusCode == "P" || documentsExecutionLogList.StatusCode == "W") && documentsExecutionLogList.CreateDate < DateTime.Now.AddMinutes(-5))
            {
                documentsExecutionLogList.StatusCode = "T";
                documentsExecutionLogList.ExceptionMessage = "The document failed to build.Please try again.";
                UpdateDocumentsExecutionLogSatusToTimeOut(documentsExecutionLogList);
            }

            return documentsExecutionLogList;
        }

        private static void UpdateDocumentsExecutionLogSatusToTimeOut(DocumentsExecutionLogList documentsExecutionLogList)
        {
            DocumentsExecutionLogRepository documentsExecutionLogRepository = new DocumentsExecutionLogRepository(documentsExecutionLogList.Tenant);
            DocumentsExecutionLog documentsExecutionLog = documentsExecutionLogRepository.GetSingleDocumentsExecutionLog(documentsExecutionLogList.Id, documentsExecutionLogList.Tenant);
            if (documentsExecutionLog != null)
            {
                documentsExecutionLog.StatusCode = "T";
                documentsExecutionLog.ExceptionMessage = "The document failed to build.Please try again.";
                documentsExecutionLogRepository.Update(documentsExecutionLog);
                documentsExecutionLogRepository.SubmitChanges();
            }
        }
    }
}