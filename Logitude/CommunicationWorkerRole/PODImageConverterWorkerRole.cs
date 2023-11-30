
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
using WebFreight.Web.Helpers.WorkerRole.PODImage;
using WebFreight.Web.Helpers.WorkerRoleHelpers;

namespace CommunicationWorkerRole
{

    class PODImageConverterWorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueService;
        private int? tenant;
        private string documentsFilingId = string.Empty;
        private string entityId = string.Empty;

        private QueueResponse queueResponse;
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "PODImageConverterWorkerRole";
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
                        HandleException(exception);
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void HandleException(Exception exception)
        {
            ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "PODImageConverterWorkerRole queue worker role start", null, null);
            if (queueResponse == null || queueResponse.RetryNumber >= 2)
            {
                queueService.CompleteAsFailed();
                return;
            }

            if (queueResponse.RetryNumber <= 1)
            {
                queueService.Delay(new TimeSpan(0, 0, 0, 5));
            }

        }

        private void ExecuteQueue()
        {
            queueService = new DbQueueService("PODImageConverterQueue", 0);
            queueResponse = queueService.Receive(new TimeSpan(0, 0, 1));

            if (queueResponse != null && queueResponse.MessageId != null)
            {
                ConvertPODImageService();
                queueService.Complete();
                LogDoneItemInMemory();

            } else Thread.Sleep(new TimeSpan(0, 0, 1));
        }

        private void ConvertPODImageService()
        {

            PODMobileDocumentsFilingArgs pODMobileDocumentsFilingArgs = new PODMobileDocumentsFilingArgs();
            pODMobileDocumentsFilingArgs.ShipmentNumber = queueResponse.MessageValues.Keys.Contains("ShipmentNumber") ? queueResponse.MessageValues["ShipmentNumber"].ToString() : "";
            pODMobileDocumentsFilingArgs.ShipmentId = queueResponse.MessageValues.Keys.Contains("ShipmentId") ? queueResponse.MessageValues["ShipmentId"].ToString() : "";
            pODMobileDocumentsFilingArgs.DocumentTypeName = queueResponse.MessageValues.Keys.Contains("DocumentTypeName") ? queueResponse.MessageValues["DocumentTypeName"].ToString() : "";
            pODMobileDocumentsFilingArgs.DocumentTypeId = queueResponse.MessageValues.Keys.Contains("DocumentTypeId") ? queueResponse.MessageValues["DocumentTypeId"].ToString() : "";
            pODMobileDocumentsFilingArgs.DocumentId = queueResponse.MessageValues.Keys.Contains("DocumentId") ? queueResponse.MessageValues["DocumentId"].ToString() : "";
            pODMobileDocumentsFilingArgs.Note = queueResponse.MessageValues.Keys.Contains("Note") ? queueResponse.MessageValues["Note"].ToString() : "";
            pODMobileDocumentsFilingArgs.Tenant = queueResponse.MessageValues.Keys.Contains("Tenant") && !string.IsNullOrEmpty(queueResponse.MessageValues["Tenant"].ToString()) ? int.Parse(queueResponse.MessageValues["Tenant"].ToString()) : 0;

            if (string.IsNullOrEmpty(pODMobileDocumentsFilingArgs.DocumentId)) return;
  
            new PODImageConverterService(pODMobileDocumentsFilingArgs).Convert(new PODImagePdfConverter());
        }

        private void ConnectClient()
        {
            try
            {
                queueService = new DbQueueService();
                queueService.InitializeQueue("PODImageConverterQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "PODImageConverterWorkerRole worker role start", null, null);
                Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }
    }



}
