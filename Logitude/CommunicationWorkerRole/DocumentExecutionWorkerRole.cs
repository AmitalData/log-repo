
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
    class DocumentExecutionWorkerRole : WorkerEntryPoint
    {

        DbQueueService queueservice;
        int tenant = 0;

        public DocumentExecutionWorkerRole()
        {

        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DocumentExecutionWR";
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
                    queueservice = new DbQueueService("DocumentExecutionQueue", 0);
                    var response = queueservice.Receive(new TimeSpan(0, 0, 1));
                    if (response != null && response.MessageId != null)
                    {
                        ExecuteQueue(response);
                        queueservice.Complete();
                    }
                    else Thread.Sleep(new TimeSpan(0, 0, 1));
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }


        private void ExecuteQueue(QueueResponse queueResponse)
        {
            ReportExecutionLog documentExecutionLog = null;
            try
            {
                documentExecutionLog = GetDocumentExecutionLogByQueueResponse(queueResponse);
                //if (documentExecutionLog != null)
              //  {
                   OpenNewThreadToBuildStimulDocument(documentExecutionLog, queueservice, queueResponse);
              //  }
            }
            catch (Exception ex)
            {
                HandleDocumentExecutionException(new DocumentExecutionArgs() { Exception = ex, DocumentExecutionLog = documentExecutionLog, queueservice = queueservice, response = queueResponse });
    
            }
        }

        private void OpenNewThreadToBuildStimulDocument(ReportExecutionLog documentExecutionLog, DbQueueService queueservice, QueueResponse response)
        {
            //UpdateDocumentExecutionLog(new DocumentExecutionArgs() {  DocumentExecutionLog = documentExecutionLog , queueservice = queueservice, response =response ,StartDate= DateTime.Now, StatusCode = "P"});
            ExportDocumentArgs exportDocumentArgs = GetExportDocumentArgs(response);
            if (exportDocumentArgs != null)
            {
                new Thread(() => BuildStimulDocument(new DocumentExecutionArgs() { DocumentExecutionLog = documentExecutionLog, queueservice = queueservice, response = response, ExportDocumentArgs = exportDocumentArgs })) { IsBackground = true }.Start();
            }
        }

        private void BuildStimulDocument(DocumentExecutionArgs documentExecutionArgs)
        {
            try
            {
                var exportDocumentArgs = documentExecutionArgs.ExportDocumentArgs;
                AuthenticationUtil.AuthenticatedUserEmail = GetLoggedUserEmail(exportDocumentArgs.LoggedContactId, exportDocumentArgs.Tenant);
                Parallel.ForEach(exportDocumentArgs.DocumentTypeCopyIdsList, (documentTypeCopyId) =>
                {
                    ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                    string result = exportDocumentHelper.ExportDocument2Pdf(exportDocumentArgs, documentTypeCopyId);
                });
               // UpdateDocumentExecutionLog(new DocumentExecutionArgs() {  DocumentExecutionLog = documentExecutionArgs.DocumentExecutionLog, queueservice = queueservice, response = documentExecutionArgs.response, StartDate= DateTime.Now, StatusCode = "D", DoneDate = DateTime.Now});
                documentExecutionArgs.queueservice.Complete();
                LogDoneItemInMemory();
            }
            catch (Exception ex)
            {
                HandleDocumentExecutionException(new DocumentExecutionArgs() { Exception = ex, DocumentExecutionLog = documentExecutionArgs.DocumentExecutionLog, queueservice = documentExecutionArgs.queueservice, response = documentExecutionArgs.response });
            }
        }

        private ExportDocumentArgs GetExportDocumentArgs(QueueResponse response)
        {
            ExportDocumentArgs exportDocumentArgs = null;
            string exportDocumentArgsXmal = response.MessageValues.Keys.Contains("ExportDocumentArgsXmal") ? response.MessageValues["ExportDocumentArgsXmal"].ToString() : "";
            if (!string.IsNullOrEmpty(exportDocumentArgsXmal))
            {
                exportDocumentArgs = LogitudeXmlSerializer.DeserializeObject<ExportDocumentArgs>(exportDocumentArgsXmal);
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

        private ReportExecutionLog GetDocumentExecutionLogByQueueResponse(QueueResponse queueResponse)
        {
            string tenantString = string.Empty;
            ReportExecutionLog reportExecutionLog = null;
            string documentExecutionLogId = queueResponse.MessageValues.Keys.Contains("DocumentExecutionLogId") ? queueResponse.MessageValues["DocumentExecutionLogId"].ToString() : "";
            if (queueResponse.MessageValues.Keys.Contains("Tenant"))
            {
                tenantString = queueResponse.MessageValues["Tenant"].ToString();
                if (!string.IsNullOrEmpty(tenantString)) tenant = int.Parse(tenantString);
            }
            if (!string.IsNullOrEmpty(documentExecutionLogId) && !string.IsNullOrEmpty(tenantString))
            {
                ReportExecutionLogRepository reportExecutionLogRepository = new ReportExecutionLogRepository(tenant);
                reportExecutionLog = reportExecutionLogRepository.GetSingleReportExecutionLog(documentExecutionLogId, tenant);
            }
            return reportExecutionLog;
        }

        private void HandleDocumentExecutionException(DocumentExecutionArgs documentExecutionArgs)
        {
            ExceptionHandler.HandleException(documentExecutionArgs.Exception, DateTime.Now, 0, null, "Document execution log queue worker role start", null, null);
            if (documentExecutionArgs.response != null && documentExecutionArgs.response.MessageValues.Keys.Contains("ExportDocumentArgsXmal"))
            {
                if (documentExecutionArgs.response.RetryNumber <= 1)
                {
                    queueservice.DelayAndReturnBackToQueue(new TimeSpan(0, 0, 0, 5), documentExecutionArgs.response.MessageId);
                }
                if (documentExecutionArgs.response.RetryNumber >= 2)
                {
                    queueservice.CompleteAsFailed();
                }
            }
            else queueservice.CompleteAsFailed();
           // UpdateDocumentExecutionLog(new DocumentExecutionArgs() { Exception = documentExecutionArgs.Exception, DocumentExecutionLog = documentExecutionArgs.DocumentExecutionLog, queueservice = queueservice, response = documentExecutionArgs.response });
        }

        private void UpdateDocumentExecutionLog(DocumentExecutionArgs documentExecutionArgs)
        {
            var documentExecutionLog = documentExecutionArgs.DocumentExecutionLog;
            if (documentExecutionLog != null)
            {
                ReportExecutionLogRepository documentExecutionLogRepository = new ReportExecutionLogRepository(tenant);
                documentExecutionLog.StatusCode = !string.IsNullOrEmpty(documentExecutionArgs.StatusCode) ? documentExecutionArgs.StatusCode : documentExecutionLog.StatusCode;
                // documentExecutionLog.RetryNumber = documentExecutionArgs.response!=null ?  documentExecutionArgs.response.RetryNumber : documentExecutionLog.RetryNumber;
                //documentExecutionLog.StartDate = documentExecutionArgs.StartDate != null ? documentExecutionArgs.StartDate : documentExecutionLog.StartDate;
                documentExecutionLog.ExceptionMessage = documentExecutionArgs.Exception != null ? GetExceptionMessage(documentExecutionArgs.Exception) : documentExecutionLog.ExceptionMessage;
                documentExecutionLog.DoneDate = documentExecutionArgs.DoneDate != null ? documentExecutionArgs.DoneDate : documentExecutionLog.DoneDate;
                //if (documentExecutionLog.RetryNumber >= 2 && documentExecutionLog.StatusCode!="D")
                //{
                //    documentExecutionLog.StatusCode = "F";
                //    documentExecutionLog.DoneDate = DateTime.Now;
                //}
                documentExecutionLogRepository.Update(documentExecutionLog);
                documentExecutionLogRepository.SubmitChanges();
            }
        }

        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("DocumentExecutionQueue", tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Document execution worker role start", null, null);
            }
        }

  
    }

    public class DocumentExecutionArgs
    {
        public Exception Exception { get; set; }
        public ReportExecutionLog DocumentExecutionLog { get; set; }
        public DbQueueService queueservice { get; set; }
        public QueueResponse response { get; set; }
        public string ExceptionMessage { get; set; }
        public string StatusCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public ExportDocumentArgs ExportDocumentArgs { get; set; }
    }

}
