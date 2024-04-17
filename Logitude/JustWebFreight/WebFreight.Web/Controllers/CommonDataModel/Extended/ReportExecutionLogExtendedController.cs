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
                SecurityUtility.CheckContactFeature("ReportExecutionLog", "UPDATE", tenant);

                var reportExecutionLogRepository = new ReportExecutionLogRepository(tenant);
                var reportExecutionLog = reportExecutionLogRepository.GetReportExecutionLog(reportId, tenant);
                var contactRep = new ContactRepository(tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, tenant);

                reportExecutionLog.StatusCode = "F";
                reportExecutionLog.ExceptionMessage =string.Format("Stopped manually by {0}", contact.Id);
                reportExecutionLog.DoneDate = DateTime.Now;


                reportExecutionLogRepository.Update(reportExecutionLog);
                reportExecutionLogRepository.SubmitChanges();

                QueueMessageRepository messagesRepository = new QueueMessageRepository(tenant);
                var message = messagesRepository.GetSingleQueueMessageByReportId(reportExecutionLog.Id,tenant);
                string strConnString = TenantServerConfigration.GetDbConnection(tenant);
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[Queue_SetStatus]", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter messageIdPar = new SqlParameter("@MessageId", SqlDbType.BigInt);
                    SqlParameter statusPar = new SqlParameter("@Statud", SqlDbType.Int);


                    messageIdPar.Direction = ParameterDirection.Input;
                    statusPar.Direction = ParameterDirection.Input;

                    messageIdPar.Value = message.Id;
                    statusPar.Value = 1;

                    cmd.Parameters.Add(messageIdPar);
                    cmd.Parameters.Add(statusPar);

                    cn.Open();
                    var output = cmd.ExecuteNonQuery();
                    cn.Close();
                }

                return Request.CreateResponse(HttpStatusCode.OK, "OK");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}