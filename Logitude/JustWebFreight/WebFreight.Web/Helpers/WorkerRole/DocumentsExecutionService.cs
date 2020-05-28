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
using System.Threading;

namespace WebFreight.Web.Helpers.WorkerRoleHelpers
{
    public class DocumentsExecutionService
    {
        private DbQueueService queueService = null;
        private QueueResponse queueResponse = null;
        private int? tenant =null;
        private string documentsExecutionLogId = string.Empty;
        private DocumentsExecutionLogRepository documentsExecutionLogRepository = null;
        private DocumentsExecutionLog documentsExecutionLog = null;
        private DateTime startDate = DateTime.Now;
 
        public DocumentsExecutionService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;
            if (queueService != null && queueResponse != null)
            {
                documentsExecutionLogId = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("DocumentsExecutionLogId") ? queueResponse.MessageValues["DocumentsExecutionLogId"].ToString() : "";
                tenant = GetTenantValueFromQueueResponse(queueResponse);
            }
        }

        public void ExecuteDocumentsExecutionQueue()
        {
            try
            {
                if (queueService != null && queueResponse!=null)
                {
                    documentsExecutionLog = GetDocumentsExecutionLog();
                    if (documentsExecutionLog != null && documentsExecutionLog.RetryNumber < 2 &&  (documentsExecutionLog.StatusCode == "W" || documentsExecutionLog.StatusCode == "P"))
                    {
                        UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() { StartDate = startDate, StatusCode = "P" });
                        ExportStimulDocumentToPDF();
                    }
                    else queueService.Complete();
                }
            }
            catch (Exception ex)
            {
                HandleDocumentsExecutionException(ex);
            }
        }

        private void ExportStimulDocumentToPDF()
        {
            ExportDocumentArgs exportDocumentArgs = !string.IsNullOrEmpty(documentsExecutionLog.RequestXML) ? LogitudeXmlSerializer.DeserializeObject<ExportDocumentArgs>(documentsExecutionLog.RequestXML) : null;
            if (exportDocumentArgs != null)
            {
                string authenticatedUserEmail = GetContactEmailByContactId(exportDocumentArgs.LoggedContactId, exportDocumentArgs.Tenant);
                Parallel.ForEach(exportDocumentArgs.DocumentTypeCopyIdsList, (documentTypeCopyId) =>
                {
                    AuthenticationUtil.AuthenticatedUserEmail = authenticatedUserEmail;
                    ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                    string result = exportDocumentHelper.ExportDocument2Pdf(exportDocumentArgs, documentTypeCopyId);
                });
                UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() {StatusCode = "D", DoneDate = DateTime.Now });
                queueService.Complete();
            }
            else
            {
                UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() { Exception = new Exception("RequestXML is null"), DoneDate = DateTime.Now, StartDate = startDate, StatusCode = "F" });
                queueService.Complete();
            }
        }

        private void UpdateDocumentsExecutionLog(DocumentsExecutionLogArgs documentsExecutionLogArgs)
        {
            if (documentsExecutionLog != null)
            {
                documentsExecutionLog.StatusCode = !string.IsNullOrEmpty(documentsExecutionLogArgs.StatusCode) ? documentsExecutionLogArgs.StatusCode : documentsExecutionLog.StatusCode;
                documentsExecutionLog.RetryNumber = queueResponse != null ? queueResponse.RetryNumber : documentsExecutionLog.RetryNumber;
                documentsExecutionLog.StartDate = documentsExecutionLogArgs.StartDate != null ? documentsExecutionLogArgs.StartDate : documentsExecutionLog.StartDate;
                documentsExecutionLog.ExceptionMessage = documentsExecutionLogArgs.Exception != null ? GetFullExceptionMessageFromException(documentsExecutionLogArgs.Exception) : documentsExecutionLog.ExceptionMessage;
                documentsExecutionLog.DoneDate = documentsExecutionLogArgs.DoneDate != null ? documentsExecutionLogArgs.DoneDate : documentsExecutionLog.DoneDate;
                if (documentsExecutionLog.RetryNumber >= 2 && documentsExecutionLog.StatusCode != "D" && documentsExecutionLogArgs.StatusCode !="P")
                {
                    documentsExecutionLog.StatusCode = "F";
                    documentsExecutionLog.DoneDate = DateTime.Now;
                }
                documentsExecutionLogRepository.Update(documentsExecutionLog);
                documentsExecutionLogRepository.SubmitChanges();
            }
        }

        private void HandleDocumentsExecutionException(Exception exception)
        {
            ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Document execution log queue worker role start", null, null);
            if (queueResponse != null)
            {
                if (queueResponse.RetryNumber <= 1)
                {
                    queueService.DelayAndReturnBackToQueue(new TimeSpan(0, 0, 0, 5), queueResponse.MessageId);
                }
                if (queueResponse.RetryNumber >= 2) queueService.CompleteAsFailed();
            }
            else queueService.CompleteAsFailed();

            UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() { Exception = exception.InnerException != null ? exception.InnerException : exception });
            Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
        }

        private DocumentsExecutionLog GetDocumentsExecutionLog()
        {
            DocumentsExecutionLog documentsExecutionLog = null;
            if (!string.IsNullOrEmpty(documentsExecutionLogId) && tenant!=null)
            {
                documentsExecutionLogRepository = new DocumentsExecutionLogRepository((int)tenant);
                documentsExecutionLog = documentsExecutionLogRepository.GetSingleDocumentsExecutionLog(documentsExecutionLogId, (int)tenant);
            }

            return documentsExecutionLog;
        }

        private string GetContactEmailByContactId(string loggedContactId, int tenant)
        {
            string contactEmail = string.Empty;
            if (!string.IsNullOrEmpty(loggedContactId))
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                contactEmail = contactQuery.GetContactEmailById(loggedContactId, tenant);
                if (contactEmail == null && tenant != 0) contactEmail = contactQuery.GetContactEmailById(loggedContactId, 0);
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


    public class DocumentsExecutionLogArgs
    {
        public string ExceptionMessage { get; set; }
        public string StatusCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public Exception Exception { get; set; }
        
    }
}