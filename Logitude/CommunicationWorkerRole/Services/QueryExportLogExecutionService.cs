using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole.Services
{
    public class QueryExportLogExecutionService
    {
        QueryExportExecutionLogRepository queryExecutionLogRepository;
        private DbQueueService queueService = null;
        private QueueResponse queueResponse = null;
        QueryExportExecutionLog executionLog;
        public QueryExportLogExecutionService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;

        }
        public void ExecuteQueryExportExecutionLog()
        {
            try
            {


                var logId = queueResponse.MessageValues.Keys.Contains("LogId") ? queueResponse.MessageValues["LogId"].ToString() : "";
                var tenant = int.Parse(queueResponse.MessageValues["Tenant"].ToString());
                var fileName = queueResponse.MessageValues["FileName"].ToString();
                var loggedUserEmail = queueResponse.MessageValues["LoggedUserEmail"].ToString();
                AuthenticationUtil.AuthenticatedUserEmail = loggedUserEmail;

                //var loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
                //HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(loggedContact?.Email), new string[0]);

                queryExecutionLogRepository = new QueryExportExecutionLogRepository(tenant);
                executionLog = queryExecutionLogRepository.GetSingle(logId, tenant);
                if (executionLog != null && executionLog.StatusCode == "W")
                {
                    UpdateExecutionLogStatus("P");
                    var queryFilters = LogitudeXmlSerializer.DeserializeObject<CustomApiQueryFilters>(executionLog.QueryFilterXML);
                    var queryToExcelExportService = new QueryToExcelExportService();
                    var queryArgs = new ExportQueryToExcelArgs()
                    {
                        QueryFilters = queryFilters,
                        IsWorkerRoleCall = true, 
                        OutputFileName = fileName,
                        LoggedUserEmail = loggedUserEmail,
                    };

                    queryToExcelExportService.ExportQueryDataToStorage(queryArgs);
                    UpdateExecutionLogStatus("D");
                }
            }
            catch (Exception ex)
            {
                HandleReportExecutionException(ex);
            }


        }

        private void UpdateExecutionLogStatus(string status, string exception = null)
        {
            if (executionLog != null)
            {
                executionLog.StatusCode = status;
                if (exception != null)
                    executionLog.ExceptionMessage = exception;
                if (status == "F" || status == "D")
                    executionLog.DoneDate = DateTime.Now;

                queryExecutionLogRepository.Update(executionLog);
                queryExecutionLogRepository.SubmitChanges();
            }
        }
        private void HandleReportExecutionException(Exception ex)
        {
            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Query to excel execution log queue worker role start", null, null);
            if (queueResponse != null && queueResponse.MessageValues.Keys.Contains("LogId"))
            {
                if (queueResponse.RetryNumber <= 1)
                {
                    queueService.DelayAndReturnBackToQueue(new TimeSpan(0, 0, 0, 5), queueResponse.MessageId);
                }
                if (queueResponse.RetryNumber >= 2)
                {
                    queueService.CompleteAsFailed();
                    UpdateExecutionLogStatus("F");
                }
            }
            else queueService.CompleteAsFailed();
        }
    }
}
