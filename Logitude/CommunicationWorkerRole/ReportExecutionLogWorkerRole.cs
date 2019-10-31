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
    class ReportExecutionLogWorkerRole : WorkerEntryPoint
    {

        DbQueueService queueService;

        public ReportExecutionLogWorkerRole()
        {

        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ReportExecutionLog";
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
                    try
                    {
                        ExecuteQueue();
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Report execution log queue worker role start", null, null);
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }






        private void ExecuteQueue()
        {
            queueService = new DbQueueService("ReportExecutionLogQueue", 0);
            var queueResponse = queueService.Receive(new TimeSpan(0, 0, 1));
            if (queueResponse != null && queueResponse.MessageId != null)
            {
                ThreadStart reportExecutionServiceThreadStart = (() => new ReportExecutionService(queueService, queueResponse).ExecuteReportExecutionQueue());
                reportExecutionServiceThreadStart += () => { LogDoneItemInMemory(); };
                new Thread(reportExecutionServiceThreadStart) { IsBackground = true }.Start();
                queueService.Complete();
            }
            else
            {
                Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void ConnectClient()
        {
            try
            {
                queueService = new DbQueueService();
                queueService.InitializeQueue("ReportExecutionLogQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Report Execution Log worker role start", null, null);
            }
        }



    }

}
