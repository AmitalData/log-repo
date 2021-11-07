using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityAMs;
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
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole
{
    public class ImporterShipmentsQueueBuilderWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
        int Tenant;
        string URI = "";//"http://localhost:9996/api/";
        //UserData User;
        APILogsService apiLogsService;
        IWebFreightContext webFreightContext;
        string ShipmentsCorrelationId;
        int RetriesCount = 0;

        string StartLogCorrelationId = Guid.NewGuid().ToString();
        public ImporterShipmentsQueueBuilderWorkerRole(string tenant)
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
            BatchServiceCode = "ImporterShipmentsSchedule";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            ConnectClient();
            return base.OnStart();
        }
        string Token;
        private bool IsCustomShipmentsAllowedForLogBox(ShipmentPM entityPM, bool isImportActivated)
        { 
            if (isImportActivated)
            {
                return (entityPM.DirectionId.ToUpper() == "C");
            }
            else
            {
                return false;
            }
        }

        private bool IsExportShipmentsAllowedForLogBox(TenantPM loggedTenant, ShipmentPM entityPM, bool isExportActivated)
        {
            if (loggedTenant.CustomerTenantShareExportFile && isExportActivated )
            {
                return (entityPM.DirectionId.ToUpper() == "E");
            }
            else
            {
                return false;
            }
        }

        private bool IsShipmentAllowedForLogBox(CustomerTenantAccessCardPM customerTenantAccessCard, TenantPM tenantPM, ShipmentPM Shipment)
        {
            return (IsCustomShipmentsAllowedForLogBox(Shipment, customerTenantAccessCard.IsImportActivated) || IsExportShipmentsAllowedForLogBox(tenantPM, Shipment, customerTenantAccessCard.IsExportActivated));
        }

        public override void Run()
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
                    var result = client.PostAsync(AuthURI, content);
                    result.Wait();
                    var tempUser = result.Result.Content.ReadAsStringAsync().Result;
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
                            queueservice.InitializeQueue("ImporterShipmentsQueueBuilderQueue", Tenant);
                            var response = queueservice.Receive();
                            LastActivity = DateTime.UtcNow;
                            int tenant = 0;
                            int importerTenant = 0;
                            //DateTime LastShipmentDateInQueue = DateTime.MinValue;
                            //DateTime HybridStartDate = DateTime.MinValue;

                            if (response != null && response.MessageId != null)
                            {
                                bool IsNewLog = false;
                                int.TryParse(response.MessageValues["tenant"], out tenant);
                                string CustomerId = response.MessageValues["CustomerId"];
                                string CustomerTenantAccessId = response.MessageValues["CustomerTenantAccessId"];
                                string BatchNumber = response.MessageValues["BatchNumber"];
                                string TempCorrelationId = response.MessageId;


                                webFreightContext = WebFreightContext.GetContext(tenant);
                                var aPILogsRepository = new APILogsRepository(webFreightContext);
                                CustomerTenantAccessCardsBatchPM customerTenantAccessCardsBatch = null;
                                int ShipmentsCount = 0;
                                ICommonDataContext Context = CommonDataContext.GetContext(tenant);
                                try
                                {
                                    List<string> ShipmentsIds;
                                    ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                                    CustomerTenantAccessCardPM customerTenantAccessCard = null;

                                    CustomerTenantAccessPM customerTenantAccessInfo = null;
                                    string systemEmail = "system@tenant" + tenant + ".com";

                                    if (!string.IsNullOrEmpty(CustomerTenantAccessId) && !string.IsNullOrEmpty(CustomerId))
                                    {
                                        CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                                        CustomerTenantAccessCardQuery customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(tenant);
                                        customerTenantAccessInfo = customerTenantAccessQuery.GetSinglePM(CustomerTenantAccessId, tenant);
                                        customerTenantAccessCard = customerTenantAccessInfo.CustomerTenantAccessCards.Where(a => a.CustomerId == CustomerId && a.CustomerTenantAccessId == CustomerTenantAccessId).FirstOrDefault();
                                        CustomerTenantAccessCardBatchQuery customerTenantAccessCardBatchQuery = new CustomerTenantAccessCardBatchQuery(tenant);
                                        customerTenantAccessCardsBatch = customerTenantAccessCardBatchQuery.GetSinglePM(CustomerId, CustomerTenantAccessId, BatchNumber, tenant);
                                        importerTenant = customerTenantAccessInfo.CustomerTenant;
                                        customerTenantAccessCard.StatusTypeCode = "IP";
                                        customerTenantAccessCard.ChangeSetOp = ChangeSetOperation.Update;
                                        CustomerTenantAccessService customerTenantAccessService = new CustomerTenantAccessService(Context, tenant, customerTenantAccessInfo, systemEmail);
                                        customerTenantAccessService.Update();
                                    }

                                    APILogsPM LogPM;
                                    if (customerTenantAccessCard != null)
                                    {

                                        string message;
                                        //if (customerTenantAccessCard.LastShipmentDateInQueue != null)
                                        //{
                                        //    message = "Start Getting Shipment From Date " + customerTenantAccessCard.LastShipmentDateInQueue + " To Date" + DateTime.Now + System.Environment.NewLine;
                                        //    ShipmentsIds = shipmentQuery.GetShipmentsByTenantCreateDateCustomer(tenant, (DateTime)customerTenantAccessCardsBatch.LastShipmentDateInQueue, customerTenantAccessCard.CustomerId);
                                        //}
                                        //else
                                        //{
                                        message = "Start Getting Shipment From Date " + customerTenantAccessCardsBatch.FromDatetime + " To Date" + customerTenantAccessCardsBatch.ToDatetime + System.Environment.NewLine + DateTime.Now + System.Environment.NewLine;
                                        ShipmentsIds = shipmentQuery.GetShipmentsByTenantCreateDateCustomer(tenant, (DateTime)customerTenantAccessCardsBatch.FromDatetime, (DateTime)customerTenantAccessCardsBatch.ToDatetime, customerTenantAccessCard.CustomerId, customerTenantAccessCardsBatch.CustomerTenantAccessId);
                                        //}
                                        if (ShipmentsIds != null)
                                        {
                                            //customerTenantAccessCardsBatch.TotalShipment = 0;
                                            //if (ShipmentsIds.Count == 0)
                                            //{

                                            //    customerTenantAccessCardsBatch.Status = "Done";
                                            //}
                                            //else
                                            //{
                                            //    customerTenantAccessCardsBatch.Status = "Build Queue";
                                            //}
                                            CustomerTenantAccessCardsBatchService customerTenantAccessCardsBatchService = new CustomerTenantAccessCardsBatchService(Context, tenant, customerTenantAccessCardsBatch);
                                            customerTenantAccessCardsBatchService.Update();
                                            apiLogsService = new APILogsService(webFreightContext, tenant);
                                            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                                            var objectContext = ShipmentsContext.GetContext(tenant);
                                            // message += "Getting Shipment From Date " + customerTenantAccessCard.HybridStartDate + " To Date" + DateTime.Now + " Completed Successfully" + System.Environment.NewLine + DateTime.Now + System.Environment.NewLine;

                                            APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(StartLogCorrelationId, tenant);
                                            if (Log == null)
                                            {
                                                IsNewLog = true;
                                                LogPM = new APILogsPM()
                                                {
                                                    Id = IdCounter.GetNumber("APILogs", tenant),
                                                    CorrelationId = StartLogCorrelationId,
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
                                            var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);

                                            using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//TransactionFactory.GetTransaction())
                                            {
                                                LogPM.ObjectTableId = Objecttable.Id;

                                                LogPM.Tenant = tenant;
                                                LogPM.Subject = "Get Shipments Range To Send To Importer Tenant";
                                                if (IsNewLog)
                                                {
                                                    apiLogsService.Create(LogPM);
                                                }
                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, message, LogitudeXmlSerializer.SerializeObjectToXmlString(ShipmentsIds), null, null, "");
                                                scope.Complete();
                                            }
                                            List<string> IdsList = new List<string>();
                                            List<string> ImportIdsList = new List<string>();

                                            TenantQuery tenantQuery = new TenantQuery(tenant);
                                            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
                                            CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                                            foreach (var item in ShipmentsIds)
                                            {
                                                var Shipment = shipmentQuery.GetSinglePMWithoutComposition(item, tenant);
                                                if (Shipment != null && (customerTenantAccessCard.LastMappingDateTime == null || Shipment.CreateDateTime > customerTenantAccessCard.LastMappingDateTime) && tenantPM.IsCustomerTenantShare && !Shipment.IsCancelled)
                                                {
                                                    CustomerTenantAccessInfo customerTenantAccess = customerTenantAccessQuery.GetCustomerTenantAccessInfo(tenant, Shipment.CustomerId);

                                                    if (customerTenantAccess != null && customerTenantAccess.HasAccess)
                                                    {
                                                        if (IsShipmentAllowedForLogBox(customerTenantAccessCard, tenantPM, Shipment))
                                                            IdsList.Add(item);
                                                        else if (Shipment.DirectionId == "I" && !string.IsNullOrEmpty(Shipment.CustomFileId))
                                                            ImportIdsList.Add(item);
                                                    }
                                                }
                                            }
                                            if (IdsList.Count == 0)
                                            {

                                                customerTenantAccessCardsBatch.Status = "Done";
                                            }
                                            else
                                            {
                                                customerTenantAccessCardsBatch.Status = "Build Queue";
                                            }
                                            customerTenantAccessCardsBatch.TotalShipment = IdsList.Count;
                                            customerTenantAccessCardsBatchService = new CustomerTenantAccessCardsBatchService(Context, tenant, customerTenantAccessCardsBatch);
                                            customerTenantAccessCardsBatchService.Update();
                                            foreach (var ShipmentId in IdsList)
                                            {
                                                RetriesCount++;
                                                var Shipment = shipmentQuery.GetSinglePM(ShipmentId, tenant);
                                                Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(ShipmentsCorrelationId, tenant);
                                                if (Log == null)
                                                {
                                                    IsNewLog = true;
                                                    LogPM = new APILogsPM()
                                                    {
                                                        Id = IdCounter.GetNumber("APILogs", tenant),
                                                        CorrelationId = ShipmentsCorrelationId,
                                                        CreateDate = DateTime.Now,
                                                        CreateDateUTC = DateTime.UtcNow,
                                                        Direction = "O",
                                                        LastUpdateDate = DateTime.Now,
                                                        LastUpdateDateUTC = DateTime.UtcNow,
                                                        NumberOfRetries = 1,
                                                        ExpirationDate = DateTime.Now.AddDays(90),
                                                        Status = "I",
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

                                                    };
                                                }
                                                LogPM.Subject = "Send Schedual updates To Importer By ImporterShipments Controller";
                                                message = "Check If Shipment Exist in Importer Tenant " + DateTime.Now + System.Environment.NewLine;

                                                Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);
                                                LogPM.ObjectTableId = Objecttable.Id;
                                                LogPM.EntityId = Shipment.Id;
                                                LogPM.Refrence = Shipment.ShipmentNumber;
                                                LogPM.Tenant = Shipment.Tenant;

                                                queueservice.InitializeQueue("ImportersShipmentsBatchQueue", 0);
                                                queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", Shipment.Id }, { "ImporterTenant", importerTenant.ToString() }, { "Tenant", tenant.ToString() }, { "BatchNumber", BatchNumber } }, tenant, null, CustomerId, BatchNumber);
                                         

                                            }
                                            foreach (var ImportId in ImportIdsList)
                                            {
                                                queueservice.InitializeQueue("ImportersShipmentsDocsQueueBuilderQueue", 0);
                                                queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", ImportId }, { "Tenant", tenant.ToString() }, { "CustomerId", customerTenantAccessCardsBatch.CustomerId }, { "BatchNumber", customerTenantAccessCardsBatch.BatchNumber } }, tenant, null, customerTenantAccessCardsBatch.CustomerId, customerTenantAccessCardsBatch.BatchNumber);
                                            }
                                            var TempLog = aPILogsRepository.GetSingleAPILogsByCorrelationId(StartLogCorrelationId, tenant);
                                            APILogsPM TempLogPM;
                                            if (TempLog == null)
                                            {
                                                IsNewLog = true;
                                                TempLogPM = new APILogsPM()
                                                {
                                                    Id = IdCounter.GetNumber("APILogs", tenant),
                                                    CorrelationId = StartLogCorrelationId,
                                                    CreateDate = DateTime.Now,
                                                    CreateDateUTC = DateTime.UtcNow,
                                                    Direction = "O",
                                                    LastUpdateDate = DateTime.Now,
                                                    LastUpdateDateUTC = DateTime.UtcNow,
                                                    NumberOfRetries = 1,
                                                    ExpirationDate = DateTime.Now.AddDays(90),
                                                    Status = "I",
                                                };
                                            }
                                            else
                                            {
                                                IsNewLog = false;
                                                TempLogPM = new APILogsPM()
                                                {
                                                    Id = TempLog.Id,
                                                    CorrelationId = TempLog.CorrelationId,
                                                    CreateDate = TempLog.CreateDate,
                                                    CreateDateUTC = TempLog.CreateDateUTC,
                                                    Direction = TempLog.Direction,
                                                    EntityId = TempLog.EntityId,
                                                    LastUpdateDate = TempLog.LastUpdateDate,
                                                    LastUpdateDateUTC = TempLog.LastUpdateDateUTC,
                                                    NumberOfRetries = TempLog.NumberOfRetries++,
                                                    ObjectTableId = TempLog.ObjectTableId,
                                                    ExpirationDate = TempLog.ExpirationDate,
                                                    Refrence = TempLog.Refrence,
                                                    Status = "I",
                                                    Tenant = TempLog.Tenant,

                                                };
                                            }
                                            APILogsUtility.UpdateAPILogStatus(TempLogPM.Id, tenant, "D", RetriesCount, DateTime.Now, DateTime.UtcNow, "Sending Schedual Shipments Done Successfully", null, null, null, "");
                                            queueservice.Complete();
                                            customerTenantAccessCard.StatusTypeCode = "A";
                                            customerTenantAccessInfo.Status = "A";
                                            customerTenantAccessCard.ChangeSetOp = ChangeSetOperation.Update;
                                            CustomerTenantAccessService Service = new CustomerTenantAccessService(Context, tenant, customerTenantAccessInfo, systemEmail);
                                            Service.Update();

                                        }
                                        //else
                                        //{
                                        //    customerTenantAccessCardsBatch.Status = "Done";
                                        //}
                                    }

                                    LogDoneItemInMemory();
                                }
                                catch (Exception ex)
                                {
                                    ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, "", "WorkerRole", "", null);
                                    if (response.MessageValues.Keys.Contains("CustomerId"))
                                    {
                                        //if (IsNewLog)
                                        //{
                                        //    apiLogsService.Create(LogPM);
                                        //}
                                        string errorMessage = ex.Message + Environment.NewLine;

                                        if (ex.InnerException != null)
                                        {

                                            errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                                        }

                                        errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                        //APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", DateTime.Now, DateTime.UtcNow, ex.Message + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
                                        string ShipmentId = response.MessageValues["CustomerId"].ToString();
                                        if (!string.IsNullOrEmpty(ShipmentId))
                                        {

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
                                            }

                                        }
                                        else
                                        {
                                            queueservice.CompleteAsFailed();
                                        }
                                    }
                                    else
                                    {
                                        queueservice.CompleteAsFailed();
                                    }
                                }
                                finally
                                {
                                    if (customerTenantAccessCardsBatch != null)
                                    {
                                        if (ShipmentsCount > 0)
                                        {
                                            customerTenantAccessCardsBatch.TotalShipment = ShipmentsCount;
                                            customerTenantAccessCardsBatch.Status = "InProgress";
                                        }
                                        CustomerTenantAccessCardsBatchService customerTenantAccessCardsBatchService = new CustomerTenantAccessCardsBatchService(Context, tenant, customerTenantAccessCardsBatch);
                                        customerTenantAccessCardsBatchService.Update();
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
                queueservice.InitializeQueue("ImporterShipmentsQueueBuilderQueue", Tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }
        }
    }
}
