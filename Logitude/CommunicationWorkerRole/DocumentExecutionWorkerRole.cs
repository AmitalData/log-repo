
using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole
{
    class DocumentsExecutionWorkerRole : WorkerEntryPoint
    {

        DbQueueService queueservice;
        int tenant = 0;
        DocumentsExecutionLogRepository documentsExecutionLogRepository = null;
        DocumentsExecutionLog documentsExecutionLog = null;
        public DocumentsExecutionWorkerRole()
        {

        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DocumentsExecutionWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            return base.OnStart();
        }


        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    queueservice = new DbQueueService("DocumentsExecutionQueue", 0);
                    var response = queueservice.Receive(new TimeSpan(0, 0, 1));
                    if (response != null && response.MessageId != null)
                    {
                        ExecuteDocumentsExecutionQueue(response);
                    }
                    else Thread.Sleep(new TimeSpan(0, 0, 1));
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }


        private void ExecuteDocumentsExecutionQueue(QueueResponse queueResponse)
        {
            try
            {
                documentsExecutionLog = GetDocumentsExecutionLogByQueueResponse(queueResponse);
                if (documentsExecutionLog != null && documentsExecutionLog.StatusCode != "D")
                {
                    OpenNewThreadToBuildStimulDocument(documentsExecutionLog, queueservice, queueResponse);
                }
                else
                {
                    queueservice.Complete();
                    LogDoneItemInMemory();
                }
            }
            catch (Exception ex)
            {
                HandleDocumentsExecutionException(new DocumentsExecutionArgs() { Exception = ex, DocumentsExecutionLog = documentsExecutionLog, DocumentsExecutionLogRepository = documentsExecutionLogRepository, Queueservice = queueservice, Response = queueResponse });
            }
        }

        private void OpenNewThreadToBuildStimulDocument(DocumentsExecutionLog documentsExecutionLog, DbQueueService queueservice, QueueResponse response)
        {
            UpdateDocumentsExecutionLog(new DocumentsExecutionArgs() {  DocumentsExecutionLog = documentsExecutionLog, DocumentsExecutionLogRepository = documentsExecutionLogRepository, Response = response ,StartDate= DateTime.Now, StatusCode = "P"});
            ExportDocumentArgs exportDocumentArgs = GetExportDocumentArgs(documentsExecutionLog);
            if (exportDocumentArgs != null)
            {
                new Thread(() => BuildStimulDocument(new DocumentsExecutionArgs() { DocumentsExecutionLog = documentsExecutionLog, DocumentsExecutionLogRepository = documentsExecutionLogRepository, Queueservice = queueservice, Response = response, ExportDocumentArgs = exportDocumentArgs })) { IsBackground = true }.Start();
                queueservice.Complete();
            }
            else
            {
                UpdateDocumentsExecutionLog(new DocumentsExecutionArgs() {Exception = new Exception("RequestXML is null"), DocumentsExecutionLog = documentsExecutionLog, DocumentsExecutionLogRepository = documentsExecutionLogRepository, Response = response, DoneDate = DateTime.Now, StatusCode = "F" });
                queueservice.Complete();
                LogDoneItemInMemory();
            }
        }

        private void BuildStimulDocument(DocumentsExecutionArgs documentsExecutionArgs)
        {
            try
            {
                var exportDocumentArgs = documentsExecutionArgs.ExportDocumentArgs;
                AuthenticationUtil.AuthenticatedUserEmail = GetLoggedUserEmail(exportDocumentArgs.LoggedContactId, exportDocumentArgs.Tenant);
                Parallel.ForEach(exportDocumentArgs.DocumentTypeCopyIdsList, (documentTypeCopyId) =>
                {
                    ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                    string result = exportDocumentHelper.ExportDocument2Pdf(exportDocumentArgs, documentTypeCopyId);
                });
                 UpdateDocumentsExecutionLog(new DocumentsExecutionArgs() {  DocumentsExecutionLog = documentsExecutionArgs.DocumentsExecutionLog, DocumentsExecutionLogRepository = documentsExecutionArgs.DocumentsExecutionLogRepository, Queueservice = queueservice, Response = documentsExecutionArgs.Response, StatusCode = "D", DoneDate = DateTime.Now});
                 documentsExecutionArgs.Queueservice.Complete();
                 LogDoneItemInMemory();
            }
            catch (Exception ex)
            {
                HandleDocumentsExecutionException(new DocumentsExecutionArgs() { Exception = ex, DocumentsExecutionLog = documentsExecutionArgs.DocumentsExecutionLog, DocumentsExecutionLogRepository = documentsExecutionArgs.DocumentsExecutionLogRepository, Queueservice = documentsExecutionArgs.Queueservice, Response = documentsExecutionArgs.Response });
            }
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

        private string GetLoggedUserEmail(string loggedContactId, int tenant)
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            return   contactQuery.GetContactEmailById(loggedContactId, tenant);
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
                documentsExecutionLog = documentsExecutionLogRepository.GetSingleDocumentsExecutionLog(documentsExecutionLogId,tenant);
            }

            return documentsExecutionLog;
        }

        private void HandleDocumentsExecutionException(DocumentsExecutionArgs DocumentsExecutionArgs)
        {
            var exception = DocumentsExecutionArgs.Exception != null ? DocumentsExecutionArgs.Exception.InnerException != null ? DocumentsExecutionArgs.Exception.InnerException : DocumentsExecutionArgs.Exception : DocumentsExecutionArgs.Exception;
            ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Document execution log queue worker role start", null, null);
            if (DocumentsExecutionArgs.Response != null && DocumentsExecutionArgs.Response.MessageValues.Keys.Contains("DocumentsExecutionLogId"))
            {
                if (DocumentsExecutionArgs.Response.RetryNumber <= 1)
                {
                    queueservice.DelayAndReturnBackToQueue(new TimeSpan(0, 0, 0, 5), DocumentsExecutionArgs.Response.MessageId);
                }
                if (DocumentsExecutionArgs.Response.RetryNumber >= 2)
                {
                    queueservice.CompleteAsFailed();
                }
            }
            else queueservice.CompleteAsFailed();

            UpdateDocumentsExecutionLog(new DocumentsExecutionArgs() { Exception = exception, DocumentsExecutionLog = DocumentsExecutionArgs.DocumentsExecutionLog, DocumentsExecutionLogRepository = DocumentsExecutionArgs.DocumentsExecutionLogRepository, Queueservice = queueservice, Response = DocumentsExecutionArgs.Response });
        }

        private void UpdateDocumentsExecutionLog(DocumentsExecutionArgs documentsExecutionArgs)
        {
            var documentsExecutionLog = documentsExecutionArgs.DocumentsExecutionLog;
            if (documentsExecutionLog != null && documentsExecutionArgs.DocumentsExecutionLogRepository!=null)
            {
                documentsExecutionLog.StatusCode = !string.IsNullOrEmpty(documentsExecutionArgs.StatusCode) ? documentsExecutionArgs.StatusCode : documentsExecutionLog.StatusCode;
                documentsExecutionLog.RetryNumber = documentsExecutionArgs.Response!=null ?  documentsExecutionArgs.Response.RetryNumber : documentsExecutionLog.RetryNumber;
                documentsExecutionLog.StartDate = documentsExecutionArgs.StartDate != null ? documentsExecutionArgs.StartDate : documentsExecutionLog.StartDate;
                documentsExecutionLog.ExceptionMessage = documentsExecutionArgs.Exception != null ? GetExceptionMessage(documentsExecutionArgs.Exception) : documentsExecutionLog.ExceptionMessage;
                documentsExecutionLog.DoneDate = documentsExecutionArgs.DoneDate != null ? documentsExecutionArgs.DoneDate : documentsExecutionLog.DoneDate;
                if (documentsExecutionLog.RetryNumber >= 2 && documentsExecutionLog.StatusCode != "D")
                {
                    documentsExecutionLog.StatusCode = "F";
                    documentsExecutionLog.DoneDate = DateTime.Now;
                }
                documentsExecutionArgs.DocumentsExecutionLogRepository.Update(documentsExecutionLog);
                documentsExecutionArgs.DocumentsExecutionLogRepository.SubmitChanges();
            }
        }

        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("DocumentsExecutionQueue", tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Document execution worker role start", null, null);
            }
        }

  
    }

    public class DocumentsExecutionArgs
    {
        public Exception Exception { get; set; }
        public DocumentsExecutionLog DocumentsExecutionLog { get; set; }
        public DbQueueService Queueservice { get; set; }
        public QueueResponse Response { get; set; }
        public string ExceptionMessage { get; set; }
        public string StatusCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public ExportDocumentArgs ExportDocumentArgs { get; set; }
        public DocumentsExecutionLogRepository DocumentsExecutionLogRepository { get; set; }
        
    }

}
