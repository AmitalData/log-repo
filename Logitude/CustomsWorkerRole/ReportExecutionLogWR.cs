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

namespace CustomsWorkerRole
{
     public class ReportExecutionLogWR : CustomsWorkerEntryPoint
    {

        DbQueueService queueService;

        public ReportExecutionLogWR()
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
            while (true)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        WorkOnce();
                        Thread.Sleep(TimeSpan.FromSeconds(1));

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
		
			queueService = new DbQueueService("ReportExecutionLogQueue", General.GetTenantDB());
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
                queueService.InitializeQueue("ReportExecutionLogQueue", General.GetTenantDB());

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Report Execution Log worker role start", null, null);
            }
        }

        //public bool IsUpdating()
        //{
        //    try
        //    {
        //        bool isUpgrading = false;
        //        if (CacheManager.CacheWrapper != null)
        //        {
        //            string cachekey = "isUpgrading_Check";
        //            if (CacheManager.CacheWrapper.Get(cachekey) == null)
        //            {

        //                IGlobalContext globalcontext = GlobalContext.GetContext();
        //                isUpgrading = (from a in globalcontext.GlobalDBs
        //                               select a).FirstOrDefault().IsUpgrading;

        //                if (CacheManager.CacheWrapper.Get(cachekey) == null)
        //                {
        //                    CacheManager.CacheWrapper.Insert(cachekey, isUpgrading, null, DateTime.UtcNow.AddSeconds(30), TimeSpan.Zero);
        //                }

        //            }
        //            else
        //            {
        //                isUpgrading = (bool)CacheManager.CacheWrapper.Get(cachekey);
        //            }
        //        }
        //        else
        //        {
        //            IGlobalContext globalcontext = GlobalContext.GetContext();
        //            isUpgrading = (from a in globalcontext.GlobalDBs
        //                           select a).FirstOrDefault().IsUpgrading;
        //        }

        //        return isUpgrading;
        //    }
        //    catch (Exception e)
        //    {
        //        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "General : IsUpdating() Method", null);
        //        return false;
        //    }
        //}

        public override void WorkOnce()
        {
            while (!WorkerRoleServiceLocator.PleaseShutDown)
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


        }

        public void DebugStep()
        {
            ExecuteQueue();
        }

    }

}
