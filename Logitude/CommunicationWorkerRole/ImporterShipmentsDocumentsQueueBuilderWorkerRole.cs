using CommunicationWorkerRole.Services.Logbox;
using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole
{
    class ImporterShipmentsDocumentsQueueBuilderWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
        int Tenant;
        string URI = "";
        ImporterShipmentsDocumentsBatchService importerShipmentsDocumentsBatchService;

        public ImporterShipmentsDocumentsQueueBuilderWorkerRole(string tenant)
        {
            Tenant = int.Parse(tenant);
            URI = CustomerTenantsURLService.Get();
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ImporterShipmentDocuments";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            ConnectClient();
            return base.OnStart();
        }

        public override void Run()
        {
            try
            {
                APICredentialsAuthenticationService.Authenticate(URI);

                while (IsRunning)
                {
                    if (!General.IsUpdating())
                    {
                        try
                        {
                            queueservice = new DbQueueService();
                            queueservice.InitializeQueue("ImportersShipmentsDocsQueueBuilderQueue", 0);
                            var response = queueservice.Receive();
                            LastActivity = DateTime.UtcNow;

                            if (response != null && response.MessageId != null)
                            {
                                try
                                {
                                    importerShipmentsDocumentsBatchService = new ImporterShipmentsDocumentsBatchService(response, queueservice);
                                    importerShipmentsDocumentsBatchService.Build();
                                    LogDoneItemInMemory();
                                }
                                catch (Exception exception)
                                {
                                    ExceptionHandler.HandleException(exception, DateTime.Now, Tenant, "", "WorkerRole", "", null);
                                    LogboxExceptionHandlerResult logboxExceptionHandlerResult = LogboxExceptionHandler.Handle(queueservice, new LogboxExceptionHandlerArgs { queueResponse = response, Exception = exception, Tenant = Tenant });
                                    if (response.RetryNumber >= 3)
                                    {
                                        importerShipmentsDocumentsBatchService.UpsertAPILogAsFailed(logboxExceptionHandlerResult.FailMessage, logboxExceptionHandlerResult.ErrorMessage);
                                    }
                                }
                            }
                            else
                            {
                                Thread.Sleep(10000);
                            }
                        }
                        catch (Exception ex)
                        {
                            ConnectClient();
                            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "importer shipments worker role start", null, null);
                            Thread.Sleep(10000);
                        }

                    }
                    else
                    {
                        Thread.Sleep(60000);
                    }
                }
            }
            catch (Exception ex)
            {
                ConnectClient();
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "importer shipments worker role start", null, null);
                Thread.Sleep(10000);
            } 
        }

        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("ImportersShipmentsDocsQueueBuilderQueue", Tenant);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }
        }
    }
}
