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
        string URI = "";//"http://localhost:9996/api/";
        APILogsService apiLogsService;
        IWebFreightContext webFreightContext;

        public ImporterShipmentsDocumentsQueueBuilderWorkerRole(string tenant)
        {
            Tenant = int.Parse(tenant);
            IGlobalContext objectContext = GlobalContext.GetContext();
            SettingRepository SettingRepository = new SettingRepository(objectContext);
            SettingQuery SettingQuery = new SettingQuery(SettingRepository);
            URI = SettingQuery.GetSinglePM().CustomerTenantsURL.TrimEnd('/') + "/api/";
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ImporterShipmentDocuments";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            ConnectClient();
            return base.OnStart();
        }
        string Token;
        string CorrelationId;
        public override async void AsyncRun()
        {
            try
            {
                APICredentialsParameters APICredentialsParam = new APICredentialsParameters()
                {
                    PrimaryKey = "8eb9c6e4-c1ca-43e5-8061-87a7adcdc5f8",
                    SecondaryKey = "c2dd0ebf-20bf-4d44-916c-7f9000dce4ec"
                };
                using (var client = new HttpClient())
                {
                    //var GetURI = URI + "ImporterShipmentDocuments/GetIfNew?id=" + DocumentFilingPM.CustomerDocumentId + "&tenant=" + importerTenant;// +"&importertenant=" + importerTenant;

                    string AuthURI = URI + "APIAuthentication";
                    var serializedObject = JsonConvert.SerializeObject(APICredentialsParam);
                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    var result = await client.PostAsync(AuthURI, content);
                    var tempUser = result.Content.ReadAsStringAsync().Result;
                    ApiCredential User = JsonConvert.DeserializeObject<ApiCredential>(tempUser);
                    Token = User.Token;
                }

                while (IsRunning)
                {
                    if (!General.IsUpdating())
                    {
                        try
                        {
                            //int tenant = 0;

                            queueservice = new DbQueueService();
                            queueservice.InitializeQueue("ImportersShipmentsDocsQueueBuilderQueue", 0);
                            var response = queueservice.Receive();
                            LastActivity = DateTime.UtcNow;
                            int tenant = 0;
                            int importerTenant = 0;

                            if (response != null && response.MessageId != null)
                            {

                                string ShipmentId = response.MessageValues["ShipmentId"].ToString();
                                string BatchNumber = response.MessageValues["BatchNumber"].ToString();
                                string CustomerId = response.MessageValues["CustomerId"].ToString();
                                //string ImporterShipmentId = response.MessageValues["ImporterShipmentId"].ToString();
                                //int.TryParse(response.MessageValues["ImporterTenant"], out importerTenant);
                                int.TryParse(response.MessageValues["Tenant"], out Tenant);
                                tenant = Tenant;
                                string TempCorrelationId = response.MessageValues["CorrelationId"].ToString();
                                if (string.IsNullOrEmpty(CorrelationId))
                                {
                                    CorrelationId = TempCorrelationId;
                                }
                                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(Tenant);
                                var DocumentFilingPMsIds = documentsFilingQuery.GetDocumentsFilingPMsIdsByEntityId(ShipmentId, "I", Tenant);
                                if (DocumentFilingPMsIds != null && DocumentFilingPMsIds.Count > 0)
                                {
                                    #region API Initialization
                                    webFreightContext = WebFreightContext.GetContext(tenant);
                                    var aPILogsRepository = new APILogsRepository(webFreightContext);
                                    APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, tenant);
                                    APILogsPM LogPM;
                                    bool IsNewLog = false;
                                    if (Log == null)
                                    {
                                        IsNewLog = true;

                                        LogPM = new APILogsPM()
                                        {
                                            Id = IdCounter.GetNumber("APILogs", tenant),
                                            CorrelationId = CorrelationId,
                                            CreateDate = DateTime.Now,
                                            CreateDateUTC = DateTime.UtcNow,
                                            Direction = "O",
                                            LastUpdateDate = DateTime.Now,
                                            LastUpdateDateUTC = DateTime.UtcNow,
                                            NumberOfRetries = 1,
                                            ExpirationDate = DateTime.Now.AddDays(90),
                                            Status = "I",
                                            QueueMessageMoreDetailsId = response.MessageId
                                        };
                                    }
                                    else
                                    {
                                        IsNewLog = false;

                                        LogPM = new APILogsPM()
                                        {
                                            Id = Log.Id,
                                            CorrelationId = Log.CorrelationId,
                                            CreateDate = Log.CreateDate,
                                            CreateDateUTC = Log.CreateDateUTC,
                                            Direction = Log.Direction,
                                            EntityId = Log.EntityId,
                                            LastUpdateDate = Log.LastUpdateDate,
                                            LastUpdateDateUTC = Log.LastUpdateDateUTC,
                                            NumberOfRetries = Log.NumberOfRetries++,
                                            ObjectTableId = Log.ObjectTableId,
                                            ExpirationDate = Log.ExpirationDate,
                                            Refrence = Log.Refrence,
                                            Status = "I",
                                            Tenant = Log.Tenant,
                                            QueueMessageMoreDetailsId = Log.QueueMessageMoreDetailsId
                                        };
                                    }
                                    #endregion

                                    try
                                    {
                                        ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                                        var Objecttable = objectTabelRepository.GetObjectTableByName("DocumentsFiling", tenant, true);
                                        LogPM.ObjectTableId = Objecttable.Id;
                                        LogPM.Tenant = tenant;
                                        LogPM.Subject = "Start Building Queues For Documents Batch ImporterShipmentDocuments Controller";
                                        if (IsNewLog)
                                        {
                                            LogPM.BatchNumber = BatchNumber;
                                            LogPM.CustomerId = CustomerId;
                                            apiLogsService = new APILogsService(webFreightContext, tenant);
                                            apiLogsService.Create(LogPM);
                                        }
                                        foreach (var DocumentFilingPMId in DocumentFilingPMsIds)
                                        {
                                            DocumentsFilingPM DocumentFilingPM = documentsFilingQuery.GetSinglePM(DocumentFilingPMId, tenant);
                                            var msg = "Queue Is Building for Document with Id " + DocumentFilingPMId + " " + DateTime.Now;
                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");
                                            if (DocumentFilingPM.IsSharedWithCustomer)
                                            {
                                                queueservice.InitializeQueue("ImportersShipmentDocumentsBatchQueue", 0);
                                                queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", ShipmentId }, { "DocumentFilingId", DocumentFilingPMId }, { "Tenant", tenant.ToString() }, { "CorrelationId", Guid.NewGuid().ToString() }, { "CustomerId", CustomerId }, { "BatchNumber", BatchNumber } }, null, CustomerId, BatchNumber);

                                            }

                                            msg = "Queue Built for Document with Id " + DocumentFilingPMId + " " + DateTime.Now;
                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");



                                        }
                                        queueservice.Complete();
                                        LogDoneItemInMemory();
                                    }
                                    catch (Exception ex)
                                    {
                                        #region Exception handling
                                        ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, "", "WorkerRole", "", null);

                                        var Failmsg = ex.Message + " " + DateTime.Now;
                                        string errorMessage = ex.Message + Environment.NewLine;

                                        if (ex.InnerException != null)
                                        {

                                            errorMessage = errorMessage + " (" + ex.InnerException.Message + ")" + Environment.NewLine;

                                        }

                                        errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                        if (response.RetryNumber <= 1)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                        }

                                        if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                        }
                                        if (response.RetryNumber >= 3)
                                        {
                                            queueservice.CompleteAsFailed();
                                            if (IsNewLog)
                                            {
                                                LogPM.Subject = "Build Documents Queues";
                                                LogPM.BatchNumber = BatchNumber;
                                                LogPM.CustomerId = CustomerId;
                                                apiLogsService = new APILogsService(webFreightContext, tenant);
                                                apiLogsService.Create(LogPM);
                                            }
                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
                                        }



                                        #endregion
                                    }
                                }
                                else
                                {
                                    queueservice.Complete();
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
