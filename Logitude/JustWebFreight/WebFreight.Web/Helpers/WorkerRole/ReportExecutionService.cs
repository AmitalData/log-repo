using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.SystemLogs;
using WebFreight.Web.DataContracts;
using Logitude.Server.Tools.Helpers;
using System.Threading.Tasks;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.Helpers.WorkerRoleHelpers
{
    public class ReportExecutionService
    {
        private DbQueueService queueService = null;
        private QueueResponse queueResponse = null;
        private int? tenant = null;
        private string reportExecutionLogId = string.Empty;
        private ReportExecutionLogRepository reportExecutionLogRepository = null;
        private ReportExecutionLog reportExecutionLog = null;
        private DateTime startDate = DateTime.Now;

        public ReportExecutionService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;
            if (queueService != null && queueResponse != null)
            {
                reportExecutionLogId = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("ReportExecutionLogId") ? queueResponse.MessageValues["ReportExecutionLogId"].ToString() : "";
                tenant = GetTenantValueFromQueueResponse(queueResponse);
            }

            ReportHelper.AddStimulsoftLicenseKey();
        }

        public void ExecuteReportExecutionQueue()
        {
            try
            {
                if (queueService != null && queueResponse != null)
                {
                    reportExecutionLog = GetReportExecutionLog();
                    if (reportExecutionLog != null && reportExecutionLog.RetryNumber < 2 && (reportExecutionLog.StatusCode == "W" || reportExecutionLog.StatusCode == "P"))
                    {
                        UpdateReportExecutionLog(new ReportExecutionLogArgs() { StartDate = startDate, StatusCode = "P", ExecutedByServerName = System.Environment.MachineName });
                        BuildStimulReport();
                    }
                    else
                    {
                        queueService.Complete();
                        if (reportExecutionLog != null && reportExecutionLog.RetryNumber >= 2 && (reportExecutionLog.StatusCode == "W" || reportExecutionLog.StatusCode == "P"))
                            UpdateReportExecutionLog(new ReportExecutionLogArgs() { Exception = new Exception(reportExecutionLog.ExceptionMessage +
                                " Report Exc failed - Removed from queue and mark as failed the exc"), DoneDate = DateTime.Now, StartDate = startDate, StatusCode = "F" });
                    }
                }
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"Failed to build stimula report for: {reportExecutionLog.Id}, error: {ex.Message}");
                try
                {
                    DatabaseInitializer.RunOnSeconderyDB = false;
                    HandleReportExecutionException(ex);
                }
                catch (Exception exception)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError($"failed to handle report execution exception: {exception.Message}");
                    UpdateReportExecutionLog(new ReportExecutionLogArgs() { Exception = exception });

                }
            }
        }

        public void ExecuteReportExecutionV2Queue()
        {
            try
            {
                if (queueService != null && queueResponse != null)
                {
                    reportExecutionLog = GetReportExecutionLog();
                    if (reportExecutionLog != null && reportExecutionLog.RetryNumber < 2 && (reportExecutionLog.StatusCode == "W" || reportExecutionLog.StatusCode == "P"))
                    {
                        UpdateReportExecutionLog(new ReportExecutionLogArgs() { StartDate = startDate, StatusCode = "P", ExecutedByServerName = System.Environment.MachineName });
                        BuildStimulReport(true);
                    }
                }
            }
            catch (Exception exception)
            {
                DatabaseInitializer.RunOnSeconderyDB = false;
                UpdateReportExecutionLog(new ReportExecutionLogArgs() { Exception = exception });
                throw new ApplicationException(exception.Message, exception.InnerException);
            }
        }

        private void BuildStimulReport(bool isVersion2 = false)
        {
            ReportFliter reportFliter = !string.IsNullOrEmpty(reportExecutionLog.ReportFilterXML) ? LogitudeXmlSerializer.DeserializeObject<ReportFliter>(reportExecutionLog.ReportFilterXML) : null;
            if (reportFliter != null)
            {
                AuthenticationUtil.AuthenticatedUserEmail = GetContactEmailByContactId(reportFliter.UserId, reportFliter.tenant);
                ReportHelper reportHelper = new ReportHelper();
                DatabaseInitializer.RunOnSeconderyDB = true;
                if(reportFliter.ProcessType == "ExportToExcel")
                {
                    reportHelper.CreateExcelOfReport(reportFliter);
                }
                else
                {
                    reportHelper.BuildStimulReport(reportFliter);

                }
                UpdateReportExecutionLog(new ReportExecutionLogArgs() { StatusCode = "D", DoneDate = DateTime.Now });
                if (!isVersion2) queueService.Complete();
            }
            else
            {
                UpdateReportExecutionLog(new ReportExecutionLogArgs() { Exception = new Exception("Report Fliter is null"), DoneDate = DateTime.Now, StartDate = startDate, StatusCode = "F" });
                if (!isVersion2) queueService.Complete();
            }
        }

        private void UpdateReportExecutionLog(ReportExecutionLogArgs reportExecutionLogArgs)
        {
            if (reportExecutionLog != null)
            {
                reportExecutionLog = GetReportExecutionLog();
                reportExecutionLog.StatusCode = !string.IsNullOrEmpty(reportExecutionLogArgs.StatusCode) ? reportExecutionLogArgs.StatusCode : reportExecutionLog.StatusCode;
                reportExecutionLog.RetryNumber = queueResponse != null ? queueResponse.RetryNumber : reportExecutionLog.RetryNumber;
                reportExecutionLog.StartDate = reportExecutionLogArgs.StartDate != null ? reportExecutionLogArgs.StartDate : reportExecutionLog.StartDate;
                reportExecutionLog.ExecutedByServerName = reportExecutionLogArgs.ExecutedByServerName != null ? reportExecutionLogArgs.ExecutedByServerName : reportExecutionLog.ExecutedByServerName;
                reportExecutionLog.ExceptionMessage = reportExecutionLogArgs.Exception != null ? GetFullExceptionMessageFromException(reportExecutionLogArgs.Exception) : reportExecutionLog.ExceptionMessage;
                reportExecutionLog.DoneDate = reportExecutionLogArgs.DoneDate != null ? reportExecutionLogArgs.DoneDate : reportExecutionLog.DoneDate;
                if ( reportExecutionLog.StatusCode != "D" && reportExecutionLogArgs.Exception != null)
                {
                    reportExecutionLog.StatusCode = "F";
                    reportExecutionLog.DoneDate = DateTime.Now;
                }
                reportExecutionLogRepository.Update(reportExecutionLog);
                reportExecutionLogRepository.SubmitChanges();
            }
        }

        private void HandleReportExecutionException(Exception exception)
        {
            ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Report execution log queue worker role start", null, null);
            if (queueResponse != null)
         {
          if (queueResponse.RetryNumber <= 1)
         {
               queueService.DelayAndReturnBackToQueue(new TimeSpan(0, 0, 0, 5), queueResponse.MessageId);
           }
             if (queueResponse.RetryNumber >= 2)
            {
              queueService.CompleteAsFailed();
            }
           }
          else queueService.CompleteAsFailed();

            UpdateReportExecutionLog(new ReportExecutionLogArgs() { Exception = exception});
        }

        private ReportExecutionLog GetReportExecutionLog()
        {
            ReportExecutionLog reportExecutionLog = null;
            if (!string.IsNullOrEmpty(reportExecutionLogId) && tenant != null)
            {
                reportExecutionLogRepository = new ReportExecutionLogRepository((int)tenant);
                reportExecutionLog = reportExecutionLogRepository.GetReportExecutionLog(reportExecutionLogId, (int)tenant);
            }

            return reportExecutionLog;
        }

        private string GetContactEmailByContactId(string loggedContactId, int tenant)
        {
            string contactEmail = string.Empty;
            if (!string.IsNullOrEmpty(loggedContactId))
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                contactEmail = contactQuery.GetContactEmailById(loggedContactId, tenant);
                if (contactEmail == null && tenant !=0) contactEmail = contactQuery.GetContactEmailById(loggedContactId, 0);
            }
            return contactEmail;
        }

        private string GetFullExceptionMessageFromException(Exception exception)
        {
            var exceptionMessage = string.Empty;

            if (exception != null)
            {
                exceptionMessage = exception.Message;
                if (exception.InnerException != null)
                {
                    exceptionMessage = exceptionMessage + Environment.NewLine + exception.InnerException;
                }
                if (exception.StackTrace != null)
                {
                    exceptionMessage = exceptionMessage + Environment.NewLine + "Stack trace: " + exception.StackTrace;
                }
            }
            return exceptionMessage;
        }

        private int? GetTenantValueFromQueueResponse(QueueResponse queueResponse)
        {
            int? tenant = null;
            if (queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("Tenant"))
            {
                string tenantString = queueResponse.MessageValues["Tenant"].ToString();
                if (!string.IsNullOrEmpty(tenantString)) tenant = int.Parse(tenantString);
            }
            return tenant;
        }

    }


    public class ReportExecutionLogArgs
    {
        public string ExceptionMessage { get; set; }
        public string ExecutedByServerName { get; set; }
        public string StatusCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public Exception Exception { get; set; }

    }
}