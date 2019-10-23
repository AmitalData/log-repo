
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

        public override async void AsyncRun()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        StartProcessingDocumentExecutionQueueservice();
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Document execution log queue worker role start", null, null);
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void StartProcessingDocumentExecutionQueueservice()
        {
            queueservice = new DbQueueService("DocumentExecutionQueue", 0);
            var response = queueservice.Receive(new TimeSpan(0, 0, 1));
            if (response != null && response.MessageId != null)
            {
                try
                {
                    ExportDocumentArgs exportDocumentArgs = GetExportDocumentArgs(response);
                    new Thread(() => BuildDocument(exportDocumentArgs, queueservice, response)) { IsBackground = true }.Start();
                    queueservice.Complete();
                }
                catch (Exception ex)
                {
                    HandleDocumentExecutionException(ex, queueservice, response);
                }
            }
            else Thread.Sleep(new TimeSpan(0, 0, 1));
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

        private void BuildDocument(ExportDocumentArgs exportDocumentArgs, DbQueueService queueservice, QueueResponse response)
        {
            try
            {
                AuthenticationUtil.AuthenticatedUserEmail = GetLoggedUserEmail(exportDocumentArgs.LoggedContactId , exportDocumentArgs.Tenant);
                Parallel.ForEach(exportDocumentArgs.DocumentTypeCopyIdsList, (documentTypeCopyId) =>
                {
                    ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                    string result = exportDocumentHelper.ExportDocument2Pdf(exportDocumentArgs.DocumentTypeId, exportDocumentArgs.EntityId, exportDocumentArgs.ObjectTableId, exportDocumentArgs.ChildEntityId, exportDocumentArgs.ChildObjectTableId, exportDocumentArgs.CurrentDocumentOutId, exportDocumentArgs.Tenant, documentTypeCopyId, exportDocumentArgs.LoggedContactId);
                });
                queueservice.Complete();
            }
            catch (Exception ex)
            {
                HandleDocumentExecutionException( ex , queueservice , response);
            }
        }

        private string GetLoggedUserEmail(string loggedContactId, int tenant)
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            return   contactQuery.GetContactEmailById(loggedContactId, tenant);
        }

        private void HandleDocumentExecutionException(Exception ex, DbQueueService queueservice, QueueResponse response)
        {
            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Document execution log queue worker role start", null, null);

            if (response != null && response.MessageValues.Keys.Contains("ExportDocumentArgsXmal"))
            {
                if (response.RetryNumber <= 1)
                {
                    queueservice.DelayAndReturnBackToQueue(new TimeSpan(0, 0, 0, 5), response.MessageId);
                }

                if (response.RetryNumber >= 2)
                {
                     queueservice.CompleteAsFailed();
                }
            }
            else queueservice.CompleteAsFailed();
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

}
