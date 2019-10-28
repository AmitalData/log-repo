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

namespace WebFreight.Web.Helpers.WorkerRoleHelpers
{
    public class DocumentsExecutionService
    {
        private DbQueueService queueservice;
        private QueueResponse queueResponse = null;
        private int tenant = 0;
        private DocumentsExecutionLogRepository documentsExecutionLogRepository = null;
        private DocumentsExecutionLog documentsExecutionLog = null;
        private DateTime? startDate = null;

        public DocumentsExecutionService(DbQueueService queueservice , QueueResponse queueResponse)
        {
            this.queueservice = queueservice;
            this.queueResponse = queueResponse;
            startDate = DateTime.Now;
        }

        public void ExportDocumentToPDF()
        {
            try
            {
                documentsExecutionLog = GetDocumentsExecutionLogByQueueResponse(queueResponse);
                if (documentsExecutionLog != null && documentsExecutionLog.StatusCode != "D")
                {
                    UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() {StartDate = startDate, StatusCode = "P" });
                    BuildStimulDocument();
                }
                else queueservice.Complete();
            }
            catch (Exception ex)
            {
                HandleDocumentsExecutionException(ex);
            }
        }

        private void BuildStimulDocument()
        {
            ExportDocumentArgs exportDocumentArgs = GetExportDocumentArgs(documentsExecutionLog);
            if (exportDocumentArgs != null)
            {
                AuthenticationUtil.AuthenticatedUserEmail = GetLoggedUserEmail(exportDocumentArgs.LoggedContactId, exportDocumentArgs.Tenant);
                Parallel.ForEach(exportDocumentArgs.DocumentTypeCopyIdsList, (documentTypeCopyId) =>
                {
                    ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                    string result = exportDocumentHelper.ExportDocument2Pdf(exportDocumentArgs, documentTypeCopyId);
                });
                UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() {StatusCode = "D", DoneDate = DateTime.Now });
                queueservice.Complete();
      
            }
            else
            {
                UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() { Exception = new Exception("RequestXML is null"), DoneDate = DateTime.Now, StartDate = startDate, StatusCode = "F" });
                queueservice.Complete();
            }

        }

        private string GetLoggedUserEmail(string loggedContactId, int tenant)
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            return contactQuery.GetContactEmailById(loggedContactId, tenant);
        }

        private ExportDocumentArgs GetExportDocumentArgs(DocumentsExecutionLog documentsExecutionLog)
        {
            ExportDocumentArgs exportDocumentArgs = null;
            if (!string.IsNullOrEmpty(documentsExecutionLog.RequestXML))
            {
                exportDocumentArgs = LogitudeXmlSerializer.DeserializeObject<ExportDocumentArgs>(documentsExecutionLog.RequestXML);
            }

            return exportDocumentArgs;
        }

        private void HandleDocumentsExecutionException(Exception ex)
        {
            var exception = ex.InnerException != null ? ex.InnerException : ex;
            ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Document execution log queue worker role start", null, null);
            if (queueResponse != null && queueResponse.MessageValues.Keys.Contains("DocumentsExecutionLogId"))
            {
                if (queueResponse.RetryNumber <= 1)
                {
                    queueservice.DelayAndReturnBackToQueue(new TimeSpan(0, 0, 0, 5), queueResponse.MessageId);
                }
                if (queueResponse.RetryNumber >= 2)
                {
                    queueservice.CompleteAsFailed();
                }
            }
            else queueservice.CompleteAsFailed();

            UpdateDocumentsExecutionLog(new DocumentsExecutionLogArgs() { Exception = exception });
        }

        private DocumentsExecutionLog GetDocumentsExecutionLogByQueueResponse(QueueResponse queueResponse)
        {
            string tenantString = string.Empty;
            DocumentsExecutionLog documentsExecutionLog = null;
            string documentsExecutionLogId = queueResponse.MessageValues.Keys.Contains("DocumentsExecutionLogId") ? queueResponse.MessageValues["DocumentsExecutionLogId"].ToString() : "";
            if (queueResponse.MessageValues.Keys.Contains("Tenant"))
            {
                tenantString = queueResponse.MessageValues["Tenant"].ToString();
                if (!string.IsNullOrEmpty(tenantString)) tenant = int.Parse(tenantString);
            }
            if (!string.IsNullOrEmpty(documentsExecutionLogId) && !string.IsNullOrEmpty(tenantString))
            {
                documentsExecutionLogRepository = new DocumentsExecutionLogRepository(tenant);
                documentsExecutionLog = documentsExecutionLogRepository.GetSingleDocumentsExecutionLog(documentsExecutionLogId, tenant);
            }

            return documentsExecutionLog;
        }
        
        private void UpdateDocumentsExecutionLog(DocumentsExecutionLogArgs documentsExecutionLogArgs)
        {
            if (documentsExecutionLog != null)
            {
                documentsExecutionLog.StatusCode = !string.IsNullOrEmpty(documentsExecutionLogArgs.StatusCode) ? documentsExecutionLogArgs.StatusCode : documentsExecutionLog.StatusCode;
                documentsExecutionLog.RetryNumber = queueResponse!=null ?  queueResponse.RetryNumber : documentsExecutionLog.RetryNumber;
                documentsExecutionLog.StartDate = documentsExecutionLogArgs.StartDate != null ? documentsExecutionLogArgs.StartDate : documentsExecutionLog.StartDate;
                documentsExecutionLog.ExceptionMessage = documentsExecutionLogArgs.Exception != null ? GetExceptionMessage(documentsExecutionLogArgs.Exception) : documentsExecutionLog.ExceptionMessage;
                documentsExecutionLog.DoneDate = documentsExecutionLogArgs.DoneDate != null ? documentsExecutionLogArgs.DoneDate : documentsExecutionLog.DoneDate;
                if (documentsExecutionLog.RetryNumber >= 2 && documentsExecutionLog.StatusCode != "D")
                {
                    documentsExecutionLog.StatusCode = "F";
                    documentsExecutionLog.DoneDate = DateTime.Now;
                }
                documentsExecutionLogRepository.Update(documentsExecutionLog);
                documentsExecutionLogRepository.SubmitChanges();
            }
        }

        private string GetExceptionMessage(Exception exception)
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