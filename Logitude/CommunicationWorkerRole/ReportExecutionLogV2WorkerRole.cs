using CommunicationWorkerRole.Stimulsoft.fonts;
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
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.WorkerRoleHelpers;

namespace CommunicationWorkerRole
{
    class ReportExecutionLogV2WorkerRole : WorkerEntryPoint
    {

        DbQueueService queueService;

        public ReportExecutionLogV2WorkerRole()
        {

        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ReportExecutionLogV2";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            ExceptionHandler.HandleException(new Exception("ReportExecutionLog V2 Worker role started"), DateTime.Now, 0, null, "Doc WorkerRole Monitor" + "|" + ThreadedRoleEntryPoint.getWorkerRoleName(), null, System.Environment.MachineName);
            return base.OnStart();
        }

        public override void Run()
        {
            ExceptionHandler.HandleException(new Exception("ReportExecutionLog V2 thread start running"), DateTime.Now, 0, null, "Doc WorkerRole Monitor" + "|" + ThreadedRoleEntryPoint.getWorkerRoleName(), null, System.Environment.MachineName);
            while (IsRunning)
            {
                if (!General.IsUpdating()) IsRunningThread();
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void IsRunningThread()
        {
            try
            {
                ExecuteQueue();
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "ReportExecutionLog V2 queue worker role thread error", null, null);
                Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void ExecuteQueue()
        {
            queueService = new DbQueueService("ReportExecutionLogV2Queue", 0);
            var queueResponse = queueService.Receive(new TimeSpan(0, 0, 1));
            if (queueResponse == null || queueResponse.MessageId == null)
            {
                Thread.Sleep(new TimeSpan(0, 0, 1));
                return;
            }
            try
            {
                new ReportExecutionService(queueService, queueResponse).ExecuteReportExecutionV2Queue();
                LogDoneItemInMemory();
                GC.Collect();
                queueService.Complete();
            }
            catch(Exception exception)
            {
                HandleExceptionRetries(queueResponse, exception);
            }
        }

        private void HandleExceptionRetries(QueueResponse response, Exception insideException)
        {
            if (response.RetryNumber <= 1)
            {
                queueService.Delay(new TimeSpan(0, 0, 0, 5));
            }
            if (response.RetryNumber >= 2)
            {
                queueService.CompleteAsFailed();
            }
            ExceptionHandler.HandleException(insideException, DateTime.Now, 0, null, "ReportExecutionLog V2 WorkerRole Monitor|" + "Catch ReportExecutionLogV2Queue", null, System.Environment.MachineName);
            Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
        }

        private void ConnectClient()
        {
            try
            {
                queueService = new DbQueueService();
                queueService.InitializeQueue("ReportExecutionLogV2Queue", 0);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Report Execution Log worker role start", null, null);
            }
        }
    }
}
