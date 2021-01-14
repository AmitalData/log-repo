using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
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
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;

namespace WebFreight.Web.Helpers.WorkerRole
{
    public class BIReportExecutionService
    {
        private DbQueueService queueService = null;
        private QueueResponse queueResponse = null;
        private int tenant = 0;
        private string reportExecutionLogId = string.Empty;
        private BIReportsExecutionLogRepository reportExecutionLogRepository = null;
        private BIReportsExecutionLog reportExecutionLog = null;
        private DateTime startDate = DateTime.Now;

        public BIReportExecutionService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;
            if (queueService != null && queueResponse != null)
            {
                reportExecutionLogId = queueResponse.MessageValues.Keys.Contains("BIReportExecutionLogId") ? queueResponse.MessageValues["BIReportExecutionLogId"].ToString() : "";
                if (queueResponse.MessageValues.Keys.Contains("Tenant"))
                {
                    var tenantString = queueResponse.MessageValues["Tenant"].ToString();
                    if (!string.IsNullOrEmpty(tenantString)) tenant = int.Parse(tenantString);
                }
            }
        }

        public void ExecuteReportExecutionQueue()
        {
            try
            {
                if (queueService != null && queueResponse != null)
                {
                    reportExecutionLog = GetReportExecutionLog();
                    if (reportExecutionLog != null && (reportExecutionLog.StatusCode != "D" || reportExecutionLog.StatusCode != "F"))
                    {
                        UpdateReportExecutionLog(new BIReportExecutionLogArgs() { StatusCode = "P" });
                        BuildReport();
                    }
                    else queueService.Complete();
                }
            }
            catch (Exception ex)
            {
                HandleReportExecutionException(ex);
            }
        }

        private BIReportsExecutionLog GetReportExecutionLog()
        {
            BIReportsExecutionLog reportExecutionLog = null;
            if (!string.IsNullOrEmpty(reportExecutionLogId))
            {
                reportExecutionLogRepository = new BIReportsExecutionLogRepository((int)tenant);
                reportExecutionLog = reportExecutionLogRepository.GetSingleBIReportExecutionLog(reportExecutionLogId, (int)tenant);
            }
            return reportExecutionLog;
        }

        private void BuildReport()
        {
            BIReportXMLData reportFliter = !string.IsNullOrEmpty(reportExecutionLog.ReportFilterXML) ? LogitudeXmlSerializer.DeserializeObject<BIReportXMLData>(reportExecutionLog.ReportFilterXML) : null;
            if (reportFliter != null)
            {
                //AuthenticationUtil.AuthenticatedUserEmail = GetContactEmailByContactId(reportFliter.UserId, tenant);
                DownloadExcel(reportFliter);
                UpdateReportExecutionLog(new BIReportExecutionLogArgs() { StatusCode = "D" });
                queueService.Complete();
            }
            else
            {
                UpdateReportExecutionLog(new BIReportExecutionLogArgs() { Exception = new Exception("BI Report Fliter is null"), StatusCode = "F" });
                queueService.Complete();
            }
        }

        private void DownloadExcel(BIReportXMLData bIReportXMLData)
        {
            var data = new ExportToExcelHelper().ExportBIQuery(bIReportXMLData, tenant);
            BIReportExecutionLogArgs handleReportExecutionLogArgs = new BIReportExecutionLogArgs() { ReportExecutionLog = reportExecutionLog, ReportExecutionLogRepository = reportExecutionLogRepository, StatusCode = "F", response = queueResponse };
            if (data != null)
            {
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = bIReportXMLData.BIReportKey,
                    FolderName = "others",
                    Extension =!string.IsNullOrEmpty(bIReportXMLData.ExportDataType)? bIReportXMLData.ExportDataType : "xlsx",
                    Tenant = tenant,
                    FileSize = data.Length,
                };
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                storageservice.Write(data, fileInfo);
                handleReportExecutionLogArgs.StatusCode = "D";
                handleReportExecutionLogArgs.FileName = fileInfo.FileName;
                this.UpdateReportExecutionLog(handleReportExecutionLogArgs);
            }
            else
            {
                UpdateReportExecutionLog(new BIReportExecutionLogArgs() { Exception = new Exception("BI Report Excel file Data is null"), StatusCode = "F" });
                queueService.Complete();
            }
        }

        private void HandleReportExecutionException(Exception exception)
        {
            ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "BI Report execution log queue worker role start", null, null);
            if (queueResponse != null && queueResponse.MessageValues.Keys.Contains("BIReportExecutionLogId"))
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

            UpdateReportExecutionLog(new BIReportExecutionLogArgs() { Exception = exception });
        }

        private void UpdateReportExecutionLog(BIReportExecutionLogArgs ReportExecutionLogArgs)
        {
            if (reportExecutionLog != null)
            {
                reportExecutionLog.StatusCode = !string.IsNullOrEmpty(ReportExecutionLogArgs.StatusCode) ? ReportExecutionLogArgs.StatusCode : reportExecutionLog.StatusCode;
                reportExecutionLog.ExceptionMessage = ReportExecutionLogArgs.Exception != null ? GetFullExceptionMessageFromException(ReportExecutionLogArgs.Exception) : reportExecutionLog.ExceptionMessage;
                reportExecutionLog.DoneDate = DateTime.Now;
                reportExecutionLogRepository.Update(reportExecutionLog);
                reportExecutionLogRepository.SubmitChanges();
            }
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

        private string GetContactEmailByContactId(string loggedContactId, int tenant)
        {
            string contactEmail = string.Empty;
            if (!string.IsNullOrEmpty(loggedContactId))
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                contactEmail = contactQuery.GetContactEmailById(loggedContactId, tenant);
            }
            return contactEmail;
        }
    }

    public class BIReportExecutionLogArgs
    {
        public Exception Exception { get; set; }
        public BIReportsExecutionLog ReportExecutionLog { get; set; }
        public BIReportsExecutionLogRepository ReportExecutionLogRepository { get; set; }
        public string StatusCode { get; set; }
        public string FileName { get; set; }
        public string ExceptionMessage { get; set; }
        public DbQueueService queueService { get; set; }
        public bool IsInternalException { get; set; }
        public QueueResponse response { get; set; }
    }
}