
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
using WebFreight.Web.Helpers.WorkerRole.DocsOut;
using WebFreight.Web.Helpers.WorkerRoleHelpers;

namespace CommunicationWorkerRole
{
    class DocumentsExecutionWorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueService;

        public static int NmuberOfRunningDocumentThreads =0;
        private const int AllowedThreadNumbers = 20;
        private static DateTime startExecuteDate;
        int tenant = 0;
        string documentsExecutionProgressStatus = "P";
        string documentsExecutionFailedStatus = "F";

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DocumentsExecutionWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
           
            new Thread(new ThreadStart(CleanUp)).Start();
            ExceptionHandler.HandleException(new Exception("Document Worker role started"), DateTime.Now, 0, null, "Doc WorkerRole Monitor"+"|"+ThreadedRoleEntryPoint.getWorkerRoleName(), null,  System.Environment.MachineName);

            return base.OnStart();
        }



        private void CleanUp()
        {
            bool isUpdatedRequired = false;
            DocumentsExecutionLogRepository documentsExecutionLogRepository = new DocumentsExecutionLogRepository(tenant);
            var documentsExecutionLogs = documentsExecutionLogRepository.GetAllDocumentsExecutionLogs().Where(d => d.ExecutedByServerName == System.Environment.MachineName && d.StatusCode == documentsExecutionProgressStatus && d.StartDate < DateTime.Now).ToList();
            foreach (DocumentsExecutionLog documentsExecutionLog in documentsExecutionLogs)
            {
                MarkDocumentsExecutionLogFailed(documentsExecutionLog, documentsExecutionLogRepository);
                isUpdatedRequired = true;
            }

            if (!isUpdatedRequired) return;
            documentsExecutionLogRepository.SubmitChanges();
            ExceptionHandler.HandleException(new Exception("Document Worker cleaned up all stuck queue messages("+ documentsExecutionLogs.Count()+") and convert them to Fail"), DateTime.Now, 0, null, "Doc WorkerRole Monitor" + "|" + ThreadedRoleEntryPoint.getWorkerRoleName(), null, System.Environment.MachineName);
        }


        private void MarkDocumentsExecutionLogFailed(DocumentsExecutionLog documentsExecutionLog , DocumentsExecutionLogRepository documentsExecutionLogRepository)
        {
            documentsExecutionLog.StatusCode = documentsExecutionFailedStatus;
            documentsExecutionLog.ExceptionMessage = "The document failed to build.Please try again. Server Machine was down";
            documentsExecutionLogRepository.Update(documentsExecutionLog);
        }


        public override void Run()
        {
            startExecuteDate = DateTime.Now;
            ExceptionHandler.HandleException(new Exception("Document Worker role thread start running"), DateTime.Now, 0, null, "Doc WorkerRole Monitor" + "|" + ThreadedRoleEntryPoint.getWorkerRoleName(), null, System.Environment.MachineName);
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                       ExecuteQueue();
                    }
                    catch (Exception exception)
                    {
                        NmuberOfRunningDocumentThreads -= 1;
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Document execution queue worker role start", null, null);
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Doc WorkerRole Monitor|Main thread", null, System.Environment.MachineName);                 
                        Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
            }
        }


        private void ExecuteQueue()
        {
            TimeSpan timeSpan = (DateTime.Now - startExecuteDate);
            if (timeSpan.Seconds > 30)
            {
                NmuberOfRunningDocumentThreads = 0;
                startExecuteDate = DateTime.Now;
            }
            else if (NmuberOfRunningDocumentThreads > AllowedThreadNumbers)
            {
                Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
                return;
            }

            queueService = new DbQueueService("DocumentsExecutionQueue", 0);
            var queueResponse = queueService.Receive(new TimeSpan(0, 0, 0, 0 ,250));

            if (queueResponse != null && queueResponse.MessageId != null)
            {
                NmuberOfRunningDocumentThreads += 1;
                ThreadStart executeDocumentsThreadStart = (() => new DocumentsExecutionService(queueService, queueResponse).ExecuteDocumentsExecutionQueue());
                executeDocumentsThreadStart += () =>
                {
                    if (NmuberOfRunningDocumentThreads > 0) NmuberOfRunningDocumentThreads -= 1;
          
                    LogDoneItemInMemory();
                };
                new Thread(executeDocumentsThreadStart) { IsBackground = true }.Start();
                queueService.Complete();
            }
        }


        private void ConnectClient()
        {
            try
            {
                queueService = new DbQueueService();
                queueService.InitializeQueue("DocumentsExecutionQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Document execution worker role start", null, null);
            }
        }


     

    }

   

}
