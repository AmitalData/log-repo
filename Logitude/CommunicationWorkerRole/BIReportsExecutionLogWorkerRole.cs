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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using WebFreight.Web.Helpers.WorkerRole;

namespace CommunicationWorkerRole
{
    public class BIReportsExecutionLogWorkerRole : WorkerEntryPoint
    {
        DbQueueService queueservice;
        int tenant = 0;

        public BIReportsExecutionLogWorkerRole()
        {

        }
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "BIReportExecutionLog";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            return base.OnStart();
        }
        private void ExecuteQueue()
        {
            queueservice = new DbQueueService("BIReportsExecutionLogQueue", 0);
            var queueResponse = queueservice.Receive(new TimeSpan(0, 0, 1));
            if (queueResponse != null && queueResponse.MessageId != null)
            {
                ThreadStart reportExecutionServiceThreadStart = (() =>
                     new BIReportExecutionService(queueservice, queueResponse).ExecuteReportExecutionQueue()
                );
                reportExecutionServiceThreadStart += () => { LogDoneItemInMemory(); };
                new Thread(reportExecutionServiceThreadStart) { IsBackground = true }.Start();
                queueservice.Complete();
            }
            else
            {
                Thread.Sleep(new TimeSpan(0, 0, 1));
            }
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
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "BI Report execution log queue worker role start", null, null);
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("BIReportsExecutionLogQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "BI Report Execution Log worker role start", null, null);
            }
        }
    }
}
