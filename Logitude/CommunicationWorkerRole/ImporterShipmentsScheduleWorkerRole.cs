using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityAMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
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
    public class ImporterShipmentsScheduleWorkerRole : WorkerEntryPoint
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
        public ImporterShipmentsScheduleWorkerRole(int tenant)
        {
            Tenant = tenant;
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
        public override async void AsyncRun()
        {
            //APICredentialsParameters APICredentialsParam = new APICredentialsParameters()
            //{
            //    PrimaryKey = "8eb9c6e4-c1ca-43e5-8061-87a7adcdc5f8",
            //    SecondaryKey = "c2dd0ebf-20bf-4d44-916c-7f9000dce4ec"
            //};
            //using (var client = new HttpClient())
            //{
            //    //var GetURI = URI + "ImporterShipmentDocuments/GetIfNew?id=" + DocumentFilingPM.CustomerDocumentId + "&tenant=" + importerTenant;// +"&importertenant=" + importerTenant;

            //    string AuthURI = URI + "APIAuthentication";
            //    var serializedObject = JsonConvert.SerializeObject(APICredentialsParam);
            //    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            //    var result = await client.PostAsync(AuthURI, content);
            //    var tempUser = result.Content.ReadAsStringAsync().Result;
            //    ApiCredential User = JsonConvert.DeserializeObject<ApiCredential>(tempUser);
            //    Token = User.Token;
            //}

            //while (IsRunning)
            //{
            //    if (!General.IsUpdating())
            //    {
            //        try
            //        {
            //            //int tenant = 0;

            //            queueservice = new DbQueueService();
            //            queueservice.InitializeQueue("ImportersShipmentScheduleQueue", Tenant);
            //            var response = queueservice.Receive();
            //            LastActivity = DateTime.UtcNow;
            //            int tenant = 0;
            //            int importerTenant = 0;
            //            //DateTime LastShipmentDateInQueue = DateTime.MinValue;
            //            //DateTime HybridStartDate = DateTime.MinValue;

            //            if (response != null && response.MessageId != null)
            //            {
            //                bool IsNewLog = false;
            //                int.TryParse(response.MessageValues["tenant"], out tenant);
            //                string CustomerId = response.MessageValues["CustomerId"];
            //                string CustomerTenantAccessId = response.MessageValues["CustomerTenantAccessId"];
            //                string TempCorrelationId = response.MessageValues["ShipmentsCorrelationId"].ToString();
            //                if (string.IsNullOrEmpty(ShipmentsCorrelationId))
            //                {
            //                    ShipmentsCorrelationId = TempCorrelationId;
            //                }
            //                //string DocsCorrelationId = response.MessageValues["DocsCorrelationId"].ToString(); 
            //                webFreightContext = WebFreightContext.GetContext(tenant);
            //                var aPILogsRepository = new APILogsRepository(webFreightContext);

            //                try
            //                {
            //                    List<string> ShipmentsIds;
            //                    ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            //                    CustomerTenantAccessCardPM customerTenantAccessCard = null;
            //                    CustomerTenantAccessPM customerTenantAccessInfo = null;
            //                    string systemEmail = "system@tenant" + tenant + ".com";
            //                    ICommonDataContext Context = CommonDataContext.GetContext(tenant);
            //                    if (!string.IsNullOrEmpty(CustomerTenantAccessId) && !string.IsNullOrEmpty(CustomerId))
            //                    {
            //                        CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
            //                        CustomerTenantAccessCardQuery customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(tenant);
            //                        customerTenantAccessInfo = customerTenantAccessQuery.GetSinglePM(CustomerTenantAccessId, tenant);
            //                        customerTenantAccessCard = customerTenantAccessInfo.CustomerTenantAccessCards.Where(a => a.CustomerId == CustomerId && a.CustomerTenantAccessId == CustomerTenantAccessId).FirstOrDefault();
            //                        importerTenant = customerTenantAccessInfo.CustomerTenant;
            //                        customerTenantAccessCard.StatusTypeCode = "IP";
            //                        customerTenantAccessCard.ChangeSetOp = ChangeSetOperation.Update;
            //                        CustomerTenantAccessService customerTenantAccessService = new CustomerTenantAccessService(Context, tenant, customerTenantAccessInfo, systemEmail);
            //                        customerTenantAccessService.Update();
            //                    }

            //                    APILogsPM LogPM;
            //                    if (customerTenantAccessCard != null)
            //                    {

            //                        string message;
            //                        if (customerTenantAccessCard.LastShipmentDateInQueue != null)
            //                        {
            //                            message = "Start Getting Shipment From Date " + customerTenantAccessCard.LastShipmentDateInQueue + " To Date" + DateTime.Now + System.Environment.NewLine;
            //                            ShipmentsIds = shipmentQuery.GetShipmentsByTenantCreateDateCustomer(tenant, (DateTime)customerTenantAccessCard.LastShipmentDateInQueue, customerTenantAccessCard.CustomerId);
            //                        }
            //                        else
            //                        {
            //                            message = "Start Getting Shipment From Date " + customerTenantAccessCard.HybridStartDate + " To Date" + DateTime.Now + System.Environment.NewLine + DateTime.Now + System.Environment.NewLine;
            //                            ShipmentsIds = shipmentQuery.GetShipmentsByTenantCreateDateCustomer(tenant, (DateTime)customerTenantAccessCard.HybridStartDate, customerTenantAccessCard.CustomerId);
            //                        }
            //                        if (ShipmentsIds != null)
            //                        {
            //                            apiLogsService = new APILogsService(webFreightContext, tenant);
            //                            ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(tenant);
            //                            var objectContext = ShipmentsContext.GetContext(tenant);
            //                            message += "Getting Shipment From Date " + customerTenantAccessCard.HybridStartDate + " To Date" + DateTime.Now + " Completed Successfully" + System.Environment.NewLine + DateTime.Now + System.Environment.NewLine;
                                        
            //                            APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(StartLogCorrelationId, tenant);
            //                            if (Log == null)
            //                            {
            //                                IsNewLog = true;
            //                                LogPM = new APILogsPM()
            //                                {
            //                                    Id = IdCounter.GetNumber("APILogs", tenant),
            //                                    CorrelationId = StartLogCorrelationId,
            //                                    CreateDate = DateTime.Now,
            //                                    CreateDateUTC = DateTime.UtcNow,
            //                                    Direction = "O",
            //                                    LastUpdateDate = DateTime.Now,
            //                                    LastUpdateDateUTC = DateTime.UtcNow,
            //                                    NumberOfRetries = 1,
            //                                    ExpirationDate = DateTime.Now.AddDays(90),
            //                                    Status = "I",
            //                                };
            //                            }
            //                            else
            //                            {
            //                                IsNewLog = false;
            //                                LogPM = new APILogsPM()
            //                                {
            //                                    Id = Log.Id,
            //                                    CorrelationId = Log.CorrelationId,
            //                                    CreateDate = Log.CreateDate,
            //                                    CreateDateUTC = Log.CreateDateUTC,
            //                                    Direction = Log.Direction,
            //                                    EntityId = Log.EntityId,
            //                                    LastUpdateDate = Log.LastUpdateDate,
            //                                    LastUpdateDateUTC = Log.LastUpdateDateUTC,
            //                                    NumberOfRetries = Log.NumberOfRetries++,
            //                                    ObjectTableId = Log.ObjectTableId,
            //                                    ExpirationDate = Log.ExpirationDate,
            //                                    Refrence = Log.Refrence,
            //                                    Status = "I",
            //                                    Tenant = Log.Tenant,

            //                                };
            //                            }
            //                            var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);

            //                            using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//TransactionFactory.GetTransaction())
            //                            {
            //                                LogPM.ObjectTableId = Objecttable.Id;

            //                                LogPM.Tenant = tenant;
            //                                LogPM.Subject = "Get Shipments Range To Send To Importer Tenant";
            //                                if (IsNewLog)
            //                                {
            //                                    apiLogsService.Create(LogPM);
            //                                }
            //                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, message, LogitudeXmlSerializer.SerializeObjectToXmlString(ShipmentsIds), null, null, "");
            //                                scope.Complete();
            //                            }
                                    
            //                            foreach (var ShipmentId in ShipmentsIds)
            //                            {
            //                                RetriesCount++;
            //                                var Shipment = shipmentQuery.GetSinglePM(ShipmentId, tenant);
            //                                Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(ShipmentsCorrelationId, tenant);
            //                                if (Log == null)
            //                                {
            //                                    IsNewLog = true;
            //                                    LogPM = new APILogsPM()
            //                                    {
            //                                        Id = IdCounter.GetNumber("APILogs", tenant),
            //                                        CorrelationId = ShipmentsCorrelationId,
            //                                        CreateDate = DateTime.Now,
            //                                        CreateDateUTC = DateTime.UtcNow,
            //                                        Direction = "O",
            //                                        LastUpdateDate = DateTime.Now,
            //                                        LastUpdateDateUTC = DateTime.UtcNow,
            //                                        NumberOfRetries = 1,
            //                                        ExpirationDate = DateTime.Now.AddDays(90),
            //                                        Status = "I",
            //                                    };
            //                                }
            //                                else
            //                                {
            //                                    IsNewLog = false;
            //                                    LogPM = new APILogsPM()
            //                                    {
            //                                        Id = Log.Id,
            //                                        CorrelationId = Log.CorrelationId,
            //                                        CreateDate = Log.CreateDate,
            //                                        CreateDateUTC = Log.CreateDateUTC,
            //                                        Direction = Log.Direction,
            //                                        EntityId = Log.EntityId,
            //                                        LastUpdateDate = Log.LastUpdateDate,
            //                                        LastUpdateDateUTC = Log.LastUpdateDateUTC,
            //                                        NumberOfRetries = Log.NumberOfRetries++,
            //                                        ObjectTableId = Log.ObjectTableId,
            //                                        ExpirationDate = Log.ExpirationDate,
            //                                        Refrence = Log.Refrence,
            //                                        Status = "I",
            //                                        Tenant = Log.Tenant,

            //                                    };
            //                                }
            //                                LogPM.Subject = "Send Schedual updates To Importer By ImporterShipments Controller";
            //                                message = "Check If Shipment Exist in Importer Tenant " + DateTime.Now + System.Environment.NewLine;
            //                                var GetURI = URI + "ImporterShipments/GetIfShipmentExists?importertenant=" + importerTenant + "&shipmentnumber=" + Shipment.ShipmentNumber;
            //                                bool IsShipmentExist = false;
            //                                using (var client = new HttpClient())
            //                                {
            //                                    client.DefaultRequestHeaders.Add("Token", Token);
            //                                    using (var apiresponse = await client.GetAsync(GetURI))
            //                                    {
            //                                        if (apiresponse.IsSuccessStatusCode)
            //                                        {

            //                                            var IsShipmentExistJsonString = apiresponse.Content.ReadAsStringAsync().Result;
            //                                            var tempResult = JsonConvert.DeserializeObject(IsShipmentExistJsonString);
            //                                            if (tempResult != null)
            //                                            {
            //                                                IsShipmentExist = (bool)tempResult;
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                                Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);
            //                                LogPM.ObjectTableId = Objecttable.Id;
            //                                LogPM.EntityId = Shipment.Id;
            //                                LogPM.Refrence = Shipment.ShipmentNumber;
            //                                LogPM.Tenant = Shipment.Tenant; 
            //                                message += "Checking If Shipment Exist in Importer Tenant Completed with value :" + IsShipmentExist + " " + DateTime.Now + System.Environment.NewLine;
            //                                using (var client = new HttpClient())
            //                                {
            //                                    string ImporterShipmentsURI = URI + "ImporterShipments";
            //                                    client.DefaultRequestHeaders.Add("Token", Token);
            //                                    client.DefaultRequestHeaders.Add("CorrelationId", ShipmentsCorrelationId);
            //                                    ICommonDataContext commoncontext = CommonDataContext.GetContext(Shipment.Tenant);
            //                                    CardRepository cardsReporistory = new CardRepository(commoncontext);
            //                                    BranchRepository branchRepository = new BranchRepository(commoncontext);
            //                                    DepartmentRepository departmentRepository = new DepartmentRepository(commoncontext);
            //                                    HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commoncontext);
            //                                    HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(hybridPartnerRepository);
            //                                    Card Shipper = cardsReporistory.GetSingleCard(Shipment.ShipperId, Shipment.Tenant);
            //                                    Card Consignee = cardsReporistory.GetSingleCard(Shipment.ConsigneeId, Shipment.Tenant);
            //                                    Branch Branch = branchRepository.GetSingleBranch(Shipment.BranchId, Shipment.Tenant);
            //                                    Department Department = departmentRepository.GetSingleDepartment(Shipment.DepartmentId, Shipment.Tenant);
            //                                    HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePMByPartnerTenant(tenant);
            //                                    TenantPM currentTenant = TenantQuery.GetSingleTenantPM(importerTenant, false);
            //                                    LogPM.PartnerName = currentTenant.CustomerName;
            //                                    string ShipperCode = "";
            //                                    string ConsigneeCode = "";
            //                                    string BranchCode = "";
            //                                    string DepartmentCode = "";
            //                                    if (Shipper != null)
            //                                    {
            //                                        ShipperCode = Shipper.Code;
            //                                    }
            //                                    if (Consignee != null)
            //                                    {
            //                                        ConsigneeCode = Consignee.Code;
            //                                    }
            //                                    if (Branch != null)
            //                                    {
            //                                        BranchCode = Branch.Code;
            //                                    }
            //                                    if (Department != null)
            //                                    {
            //                                        DepartmentCode = Department.Code;
            //                                    }
            //                                    if (IsShipmentExist)
            //                                    {
            //                                        //Update 
            //                                        #region ShipmentAM
            //                                        ShipmentAM shipmentAM = new ShipmentAM()
            //                                        {
            //                                            Id = Shipment.Id,
            //                                            ImporterTenant = importerTenant,
            //                                            Tenant = Shipment.Tenant,
            //                                            MainCarriageATA = Shipment.MainCarriageATA,
            //                                            MainCarriageETA = Shipment.MainCarriageETA,
            //                                            MainCarriageATD = Shipment.MainCarriageATD,
            //                                            OnCarriageATA = Shipment.OnCarriageATA,
            //                                            OnCarriageATD = Shipment.OnCarriageATD,
            //                                            PreCarriageATA = Shipment.PreCarriageATA,
            //                                            PreCarriageATD = Shipment.PreCarriageATD,
            //                                            //StatusId = Shipment.StatusId,
            //                                            TransportModeId = Shipment.TransportModeId,
            //                                            DirectionId = Shipment.DirectionId,
            //                                            ShipmentLevelCode = Shipment.ShipmentLevelCode,
            //                                            //ForwarderShipmentNumber = Shipment.ShipmentNumber,
            //                                            CustomerShipmentNumber = Shipment.CustomerShipmentNumber,
            //                                            ShipmentTypeId = Shipment.ShipmentTypeId,
            //                                            House = Shipment.House,
            //                                            DescriptionOfGoods = Shipment.DescriptionOfGoods,
            //                                            ConsigneeReference1 = Shipment.ConsigneeReference1,
            //                                            CustomerReference1 = Shipment.CustomerReference1,
            //                                            ConsigneeReference2 = Shipment.ConsigneeReference2,
            //                                            CustomerReference2 = Shipment.CustomerReference2,
            //                                            ShipmentCustomerTypeCode = Shipment.ShipmentCustomerTypeCode,
            //                                            IsCancelled = Shipment.IsCancelled,
            //                                            ShipperName = Shipment.ShipperName,
            //                                            CarrierTransportDocumentNumber = Shipment.CarrierTransportDocumentNumber,
            //                                            ForwarderPartnerId = Partner.Id,
            //                                            FreightPrepaidCollectId = Shipment.FreightPrepaidCollectId,
            //                                            OtherPrepaidCollectId = Shipment.OtherPrepaidCollectId,
            //                                            //CustomerProperties = new CodeProperties()
            //                                            //{
            //                                            //    Code = CustomerCode
            //                                            //},
            //                                            Branch = new CodeProperties()
            //                                            {
            //                                                Code = BranchCode
            //                                            },
            //                                            Department = new CodeProperties()
            //                                            {
            //                                                Code = DepartmentCode
            //                                            },
            //                                            Shipper = new CodeProperties()
            //                                            {
            //                                                Code = ShipperCode
            //                                            },
            //                                            //ConsigneeProperties = new CodeProperties()
            //                                            //{
            //                                            //    Code = ConsigneeCode
            //                                            //},
            //                                            FromPort = new CodeProperties()
            //                                            {
            //                                                Code = Shipment.FromPort,
            //                                                CountryCode = Shipment.FromCountryCode
            //                                            },
            //                                            ToPort = new CodeProperties()
            //                                            {
            //                                                Code = Shipment.ToPort,
            //                                                CountryCode = Shipment.ToCountryCode
            //                                            },
            //                                            PreCarriageFromPort = new CodeProperties()
            //                                            {
            //                                                Code = Shipment.PreCarriageFromPortCode,
            //                                                CountryCode = Shipment.PreCarriageFromPortCountryCode
            //                                            },
            //                                            PreCarriageToPort = new CodeProperties()
            //                                            {
            //                                                Code = Shipment.PreCarriageToPortCode,
            //                                                CountryCode = Shipment.PreCarriageToPortCountryCode
            //                                            },
            //                                            OnCarriageToPort = new CodeProperties()
            //                                            {
            //                                                Code = Shipment.OnCarriageToPortCode,
            //                                                CountryCode = Shipment.OnCarriageToPortCountryCode
            //                                            }

            //                                        };

            //                                        #endregion
            //                                        var serializedObject = JsonConvert.SerializeObject(shipmentAM);
            //                                        using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//TransactionFactory.GetTransaction())
            //                                        {
            //                                            if (IsNewLog)
            //                                            {
            //                                                IsNewLog = false;
            //                                                LogPM.CorrelationId = Guid.NewGuid().ToString();
            //                                                apiLogsService.Create(LogPM);
            //                                            }

            //                                            message += "Start Sending Updates Of Shipment To Importer Tenant " + DateTime.Now;
            //                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", RetriesCount, DateTime.Now, DateTime.UtcNow, message, LogitudeXmlSerializer.SerializeObjectToXmlString(shipmentAM), null, null, "");
            //                                            scope.Complete();
            //                                        }
            //                                        message = "";
            //                                        var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            //                                        var result = await client.PutAsync(ImporterShipmentsURI, content);
            //                                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
            //                                        {
            //                                            #region If OK
            //                                            try
            //                                            {
            //                                                using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//TransactionFactory.GetTransaction())
            //                                                {
            //                                                    var temp = result.Content.ReadAsStringAsync().Result;
            //                                                    //string ImporterShipmentNo = JsonConvert.DeserializeObject<string>(temp);
            //                                                    List<string> ImporterShipmentId_No = JsonConvert.DeserializeObject<List<string>>(temp);

            //                                                    LogPM.Status = "D";
            //                                                    var Donemsg = "Updates Of Shipment Sent To Importer Successfully " + DateTime.Now;
            //                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", RetriesCount, DateTime.Now, DateTime.UtcNow, Donemsg, null, temp, null, "");

            //                                                    // Update Forwarder Shipment

            //                                                    //var ImporterShipment = shipmentQuery.GetSinglePMWithoutComposition(ImporterShipmentId, importerTenant); // todo: What if its in other database
            //                                                    var ForwarderShipment = Shipment;//shipmentQuery.GetSinglePMWithoutComposition(Shipment.Id, tenant);
            //                                                    ForwarderShipment.CustomerShipmentNumber = ImporterShipmentId_No[1];
            //                                                    ForwarderShipment.DontAddToImportersQueue = true;
            //                                                    string msg = "Update CustomerShipmentNumber For Sent Shipment " + DateTime.Now;
            //                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", RetriesCount, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");


            //                                                    var shipmentService = new ShipmentService(objectContext, ForwarderShipment, systemEmail);
            //                                                    shipmentService.Update();
            //                                                    msg = "Update CustomerShipmentNumber Completed Successfully " + DateTime.Now;
            //                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", RetriesCount, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");

            //                                                    msg = "Update LastShipmentDateInQueue and LastShipmentDate in Forwarder CustomerTenantAccess && CustomerTenantAccessCard  " + DateTime.Now;
            //                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", RetriesCount, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");
            //                                                    customerTenantAccessCard.LastShipmentDateInQueue = Shipment.CreateDateTime;
            //                                                    customerTenantAccessCard.ChangeSetOp = ChangeSetOperation.Update;
            //                                                    customerTenantAccessInfo.LastShipmentDate = DateTime.Now;
            //                                                    CustomerTenantAccessService customerTenantAccessService = new CustomerTenantAccessService(Context, ForwarderShipment.Tenant, customerTenantAccessInfo, systemEmail);
            //                                                    customerTenantAccessService.Update();
            //                                                    msg = "Update LastShipmentDateInQueue and LastShipmentDate in Forwarder CustomerTenantAccess && CustomerTenantAccessCard Completed Successfully " + DateTime.Now;
            //                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", RetriesCount, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");
            //                                                    ShipmentsCorrelationId = Guid.NewGuid().ToString();
            //                                                    RetriesCount = 0;
            //                                                    //CheckShipmentDocuments(Shipment.Id, Shipment.Tenant, ImporterShipmentId_No[0], importerTenant, Guid.NewGuid().ToString());
            //                                                    queueservice.InitializeQueue("ImportersShipmentsDocsScheduleQueue", 0);
            //                                                    queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", Shipment.Id }, { "Tenant", tenant.ToString() }, {"CorrelationId",Guid.NewGuid().ToString()} });
            
            //                                                    scope.Complete();
            //                                                }
            //                                            }
            //                                            catch (Exception ex)
            //                                            {
            //                                                string errorMessage = ex.Message + Environment.NewLine;

            //                                                if (ex.InnerException != null)
            //                                                {

            //                                                     errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

            //                                                }

            //                                                errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
            //                                                APIException exc = new APIException()
            //                                                {
            //                                                    ErrorType = ex.GetType().Name,
            //                                                    ErrorMessage = errorMessage
            //                                                };
            //                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", RetriesCount, DateTime.Now, DateTime.UtcNow, ex.Message, null, null, LogitudeXmlSerializer.SerializeObjectToXmlString(exc), errorMessage.Substring(0, 249));
            //                                                throw new Exception(exc.ErrorType, new Exception(exc.ErrorMessage));
            //                                            }

            //                                            #endregion
            //                                        }
            //                                        if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            //                                        {
            //                                            #region If BadRequest
            //                                            var temp = result.Content.ReadAsStringAsync().Result;
            //                                            APIException EXC = JsonConvert.DeserializeObject<APIException>(temp);
            //                                            if (EXC != null)
            //                                            {
            //                                                var Failmsg = "Sending Shipment To Importer Tenant Faild " + DateTime.Now;
            //                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", RetriesCount, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
            //                                                throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
            //                                            }
            //                                            //queueservice.CompleteAsFailed();
            //                                            #endregion
            //                                        }

            //                                    }
            //                                    else
            //                                    {
            //                                        //Insert
            //                                        #region ShipmentAM
            //                                        ShipmentAM shipmentAM = new ShipmentAM()
            //                                        {
            //                                            Id = Shipment.Id,
            //                                            ImporterTenant = importerTenant,
            //                                            Tenant = Shipment.Tenant,
            //                                            MainCarriageATA = Shipment.MainCarriageATA,
            //                                            MainCarriageETA = Shipment.MainCarriageETA,
            //                                            MainCarriageATD = Shipment.MainCarriageATD,
            //                                            OnCarriageATA = Shipment.OnCarriageATA,
            //                                            OnCarriageATD = Shipment.OnCarriageATD,
            //                                            PreCarriageATA = Shipment.PreCarriageATA,
            //                                            PreCarriageATD = Shipment.PreCarriageATD,
            //                                            //StatusId = Shipment.StatusId,
            //                                            TransportModeId = Shipment.TransportModeId,
            //                                            DirectionId = Shipment.DirectionId,
            //                                            ShipmentLevelCode = Shipment.ShipmentLevelCode,
            //                                            ForwarderShipmentNumber = Shipment.ShipmentNumber,
            //                                            ShipmentTypeId = Shipment.ShipmentTypeId,
            //                                            House = Shipment.House,
            //                                            DescriptionOfGoods = Shipment.DescriptionOfGoods,
            //                                            ConsigneeReference1 = Shipment.ConsigneeReference1,
            //                                            CustomerReference1 = Shipment.CustomerReference1,
            //                                            ConsigneeReference2 = Shipment.ConsigneeReference2,
            //                                            CustomerReference2 = Shipment.CustomerReference2,
            //                                            ShipmentCustomerTypeCode = Shipment.ShipmentCustomerTypeCode,
            //                                            IsCancelled = Shipment.IsCancelled,
            //                                            ShipperName = Shipment.ShipperName,
            //                                            CarrierTransportDocumentNumber = Shipment.CarrierTransportDocumentNumber,
            //                                            ForwarderPartnerId = Partner.Id,
            //                                            FreightPrepaidCollectId = Shipment.FreightPrepaidCollectId,
            //                                            OtherPrepaidCollectId = Shipment.OtherPrepaidCollectId,
            //                                            //CustomerProperties = new CodeProperties()
            //                                            //{
            //                                            //    Code = CustomerCode
            //                                            //},
            //                                            Branch = new CodeProperties()
            //                                            {
            //                                                Code = BranchCode
            //                                            },
            //                                            Department = new CodeProperties()
            //                                            {
            //                                                Code = DepartmentCode
            //                                            },
            //                                            Shipper = new CodeProperties()
            //                                            {
            //                                                Code = ShipperCode
            //                                            },
            //                                            //ConsigneeProperties = new CodeProperties()
            //                                            //{
            //                                            //    Code = ConsigneeCode
            //                                            //},
            //                                            FromPort = new CodeProperties()
            //                                            {
            //                                                Code = Shipment.FromPort,
            //                                                CountryCode = Shipment.FromCountryCode
            //                                            },
            //                                            ToPort = new CodeProperties()
            //                                            {
            //                                                Code = Shipment.ToPort,
            //                                                CountryCode = Shipment.ToCountryCode
            //                                            },
            //                                            PreCarriageFromPort = new CodeProperties()
            //                                            {
            //                                                Code = Shipment.PreCarriageFromPortCode,
            //                                                CountryCode = Shipment.PreCarriageFromPortCountryCode
            //                                            },
            //                                            PreCarriageToPort = new CodeProperties()
            //                                            {
            //                                                Code = Shipment.PreCarriageToPortCode,
            //                                                CountryCode = Shipment.PreCarriageToPortCountryCode
            //                                            },
            //                                            OnCarriageToPort = new CodeProperties()
            //                                            {
            //                                                Code = Shipment.OnCarriageToPortCode,
            //                                                CountryCode = Shipment.OnCarriageToPortCountryCode
            //                                            }
            //                                        };
            //                                        #endregion

            //                                        var serializedObject = JsonConvert.SerializeObject(shipmentAM);
            //                                        LogPM.Subject = "Send Shipment To Importer By ImporterShipments Controller";
            //                                        if (IsNewLog)
            //                                        {
            //                                            IsNewLog = false;
            //                                            apiLogsService.Create(LogPM);
            //                                        }
            //                                        message += "Start Sending Shipment To Importer " + DateTime.Now;
            //                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, LogPM.Status, RetriesCount, DateTime.Now, DateTime.UtcNow, message, LogitudeXmlSerializer.SerializeObjectToXmlString(shipmentAM), null, null, "");
            //                                        message = "";
            //                                        var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            //                                        var result = await client.PostAsync(ImporterShipmentsURI, content);

            //                                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
            //                                        {
            //                                            #region If Ok
            //                                            using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//TransactionFactory.GetTransaction())
            //                                            {
            //                                                var temp = result.Content.ReadAsStringAsync().Result;
            //                                                List<string> ImporterShipmentId_No = JsonConvert.DeserializeObject<List<string>>(temp);
            //                                                LogPM.Status = "D";
            //                                                var Donemsg = "Shipment Sent To Importer Successfully " + DateTime.Now;
            //                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", RetriesCount, DateTime.Now, DateTime.UtcNow, Donemsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(temp), null, "");
            //                                                // Update Forwarder Shipment
            //                                                string msg = "Update CustomerShipmentNumber in Forwarder Shipment " + DateTime.Now;
            //                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", RetriesCount, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");
            //                                                //var ImporterShipment = shipmentQuery.GetSinglePMWithoutComposition(ImporterShipmentId_No[1], importerTenant);
            //                                                var ForwarderShipment = shipmentQuery.GetSinglePMWithoutComposition(Shipment.Id, tenant);
            //                                                ForwarderShipment.CustomerShipmentNumber = ImporterShipmentId_No[1];
            //                                                ForwarderShipment.DontAddToImportersQueue = true;


            //                                                var shipmentService = new ShipmentService(objectContext, ForwarderShipment, systemEmail);
            //                                                shipmentService.Update();
            //                                                msg = "Update CustomerShipmentNumber in Forwarder Shipment Completed Successfully " + DateTime.Now;
            //                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", RetriesCount, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");

            //                                                msg = "Update LastShipmentDate in Forwarder CustomerTenantAccess " + DateTime.Now;
            //                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", RetriesCount, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");
            //                                                customerTenantAccessCard.LastShipmentDateInQueue = Shipment.CreateDateTime;
            //                                                customerTenantAccessCard.ChangeSetOp = ChangeSetOperation.Update;
            //                                                customerTenantAccessInfo.LastShipmentDate = DateTime.Now;
            //                                                CustomerTenantAccessService customerTenantAccessService = new CustomerTenantAccessService(Context, ForwarderShipment.Tenant, customerTenantAccessInfo, systemEmail);
            //                                                customerTenantAccessService.Update();
            //                                                msg = "Update LastShipmentDate in Forwarder CustomerTenantAccess Completed Successfully " + DateTime.Now;
            //                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", RetriesCount, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");
            //                                                ShipmentsCorrelationId = Guid.NewGuid().ToString();
            //                                                RetriesCount = 0;
            //                                                //CheckShipmentDocuments(ForwarderShipment.Id, ForwarderShipment.Tenant, ImporterShipmentId_No[0], importerTenant, Guid.NewGuid().ToString());
            //                                                queueservice.InitializeQueue("ImportersShipmentsDocsScheduleQueue", 0);
            //                                                queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", Shipment.Id }, { "Tenant", tenant.ToString() },{"CorrelationId",Guid.NewGuid().ToString()} });
            //                                                scope.Complete();
            //                                            }
            //                                            #endregion
            //                                        }
            //                                        if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            //                                        {
            //                                            #region If BadRequest
            //                                            var temp = result.Content.ReadAsStringAsync().Result;
            //                                            APIException EXC = JsonConvert.DeserializeObject<APIException>(temp);
            //                                            if (EXC != null)
            //                                            {
            //                                                var Failmsg = "Fail To Send Updates To Importer Tenant " + DateTime.Now;
            //                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", RetriesCount, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
            //                                                throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
            //                                            }
            //                                            //queueservice.CompleteAsFailed();
            //                                            #endregion
            //                                        }
            //                                    }

            //                                }
            //                            }
            //                            var TempLog = aPILogsRepository.GetSingleAPILogsByCorrelationId(StartLogCorrelationId, tenant);
            //                            APILogsPM TempLogPM;
            //                            if (TempLog == null)
            //                            {
            //                                IsNewLog = true;
            //                                TempLogPM = new APILogsPM()
            //                                {
            //                                    Id = IdCounter.GetNumber("APILogs", tenant),
            //                                    CorrelationId = StartLogCorrelationId,
            //                                    CreateDate = DateTime.Now,
            //                                    CreateDateUTC = DateTime.UtcNow,
            //                                    Direction = "O",
            //                                    LastUpdateDate = DateTime.Now,
            //                                    LastUpdateDateUTC = DateTime.UtcNow,
            //                                    NumberOfRetries = 1,
            //                                    ExpirationDate = DateTime.Now.AddDays(90),
            //                                    Status = "I",
            //                                };
            //                            }
            //                            else
            //                            {
            //                                IsNewLog = false;
            //                                TempLogPM = new APILogsPM()
            //                                {
            //                                    Id = TempLog.Id,
            //                                    CorrelationId = TempLog.CorrelationId,
            //                                    CreateDate = TempLog.CreateDate,
            //                                    CreateDateUTC = TempLog.CreateDateUTC,
            //                                    Direction = TempLog.Direction,
            //                                    EntityId = TempLog.EntityId,
            //                                    LastUpdateDate = TempLog.LastUpdateDate,
            //                                    LastUpdateDateUTC = TempLog.LastUpdateDateUTC,
            //                                    NumberOfRetries = TempLog.NumberOfRetries++,
            //                                    ObjectTableId = TempLog.ObjectTableId,
            //                                    ExpirationDate = TempLog.ExpirationDate,
            //                                    Refrence = TempLog.Refrence,
            //                                    Status = "I",
            //                                    Tenant = TempLog.Tenant,

            //                                };
            //                            }
            //                            APILogsUtility.UpdateAPILogStatus(TempLogPM.Id, tenant, "D", RetriesCount, DateTime.Now, DateTime.UtcNow, "Sending Schedual Shipments Done Successfully", null, null, null, "");
            //                            queueservice.Complete();
            //                            customerTenantAccessCard.StatusTypeCode = "A";
            //                            customerTenantAccessCard.ChangeSetOp = ChangeSetOperation.Update;
            //                            CustomerTenantAccessService Service = new CustomerTenantAccessService(Context, tenant, customerTenantAccessInfo, systemEmail);
            //                            Service.Update();

            //                        }
            //                    }

            //                    LogDoneItem();
            //                }
            //                catch (Exception ex)
            //                {
            //                    ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, "", "WorkerRole", "", null);
            //                    if (response.MessageValues.Keys.Contains("CustomerId"))
            //                    {
            //                        //if (IsNewLog)
            //                        //{
            //                        //    apiLogsService.Create(LogPM);
            //                        //}
            //                        string errorMessage = ex.Message + Environment.NewLine;

            //                        if (ex.InnerException != null)
            //                        {

            //                             errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

            //                        }

            //                        errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
            //                        //APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", DateTime.Now, DateTime.UtcNow, ex.Message + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
            //                        string ShipmentId = response.MessageValues["CustomerId"].ToString();
            //                        if (!string.IsNullOrEmpty(ShipmentId))
            //                        {

            //                            if (response.RetryNumber <= 1)
            //                            {
            //                                queueservice.Delay(new TimeSpan(0, 0, 0, 5));
            //                            }

            //                            if (response.RetryNumber > 1 && response.RetryNumber <= 2)
            //                            {
            //                                queueservice.Delay(new TimeSpan(0, 0, 0, 10));
            //                            }
            //                            if (response.RetryNumber >= 3)
            //                            {
            //                                queueservice.CompleteAsFailed();
            //                            }

            //                        }
            //                        else
            //                        {
            //                            queueservice.CompleteAsFailed();
            //                        }
            //                    }
            //                    else
            //                    {
            //                        queueservice.CompleteAsFailed();
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                Thread.Sleep(10000);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            ConnectClient();
            //            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "importer shipments worker role start", null, null);
            //            Thread.Sleep(10000);
            //        }

            //    }
            //    else
            //    {
            //        Thread.Sleep(60000);
            //    }
            //}
        }

        //private async void CheckShipmentDocuments(string ShipmentId, int Tenant, string ImporterShipmentId, int ImporterTenant, string CorrelationId)
        //{
        //    DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(Tenant);
        //    var DocumentFilingPMs = documentsFilingQuery.GetDocumentsFilingPMsByEntityId(ShipmentId, "I", Tenant);
        //    IWebFreightContext webFreightContext = WebFreightContext.GetContext(Tenant);
        //    var aPILogsRepository = new APILogsRepository(webFreightContext);
        //    APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, Tenant);
        //    APILogsPM LogPM;
        //    bool IsNewLog = false;
        //    if (Log == null)
        //    {
        //        IsNewLog = true;

        //        LogPM = new APILogsPM()
        //        {
        //            Id = IdCounter.GetNumber("APILogs", Tenant),
        //            CorrelationId = CorrelationId,
        //            CreateDate = DateTime.Now,
        //            CreateDateUTC = DateTime.UtcNow,
        //            Direction = "O",
        //            LastUpdateDate = DateTime.Now,
        //            LastUpdateDateUTC = DateTime.UtcNow,
        //            NumberOfRetries = 1,
        //            ExpirationDate = DateTime.Now.AddDays(90),
        //            Status = "I"
        //        };
        //    }
        //    else
        //    {
        //        IsNewLog = false;

        //        LogPM = new APILogsPM()
        //        {
        //            Id = Log.Id,
        //            CorrelationId = Log.CorrelationId,
        //            CreateDate = Log.CreateDate,
        //            CreateDateUTC = Log.CreateDateUTC,
        //            Direction = Log.Direction,
        //            EntityId = Log.EntityId,
        //            LastUpdateDate = Log.LastUpdateDate,
        //            LastUpdateDateUTC = Log.LastUpdateDateUTC,
        //            NumberOfRetries = Log.NumberOfRetries++,
        //            ObjectTableId = Log.ObjectTableId,
        //            ExpirationDate = Log.ExpirationDate,
        //            Refrence = Log.Refrence,
        //            Status = "I",
        //            Tenant = Log.Tenant,

        //        };
        //    }
        //    ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(Tenant);
        //    var Objecttable = objectTabelRepository.GetObjectTableByName("DocumentsFiling", Tenant, true);
        //    LogPM.ObjectTableId = Objecttable.Id;
        //    int Retries = 0;
        //    foreach (var DocumentFilingPM in DocumentFilingPMs)
        //    {
        //        for (int i = 0; i <= Retries; i++)
        //        {
        //            i++;
        //            var DocumentFilingId = DocumentFilingPM.Id;
        //            if (DocumentFilingPM.IsSharedWithCustomer && DocumentFilingPM.HasFile)
        //            {
        //                LogPM.EntityId = DocumentFilingPM.Id;
        //                LogPM.Refrence = DocumentFilingPM.Code;
        //                LogPM.Tenant = DocumentFilingPM.Tenant;
        //                try
        //                {
        //                    var GetURI = URI + "ImporterShipmentDocuments/GetIfNew?id=" + DocumentFilingPM.CustomerDocumentId + "&tenant=" + ImporterTenant;// +"&importertenant=" + ImporterTenant;
        //                    bool IsNew = false;
        //                    using (var client = new HttpClient())
        //                    {
        //                        client.DefaultRequestHeaders.Add("Token", Token);
        //                        using (var apiresponse = await client.GetAsync(GetURI))
        //                        {
        //                            if (apiresponse.IsSuccessStatusCode)
        //                            {

        //                                var IsNewJsonString = apiresponse.Content.ReadAsStringAsync().Result;
        //                                var tempResult = JsonConvert.DeserializeObject(IsNewJsonString);
        //                                if (tempResult != null)
        //                                {
        //                                    IsNew = (bool)tempResult;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    string DocumentTypeId = "";
        //                    byte[] datainByte = null;
        //                    DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(Tenant);
        //                    var temp = documentTypeQuery.GetSinglePMByCodeAndTenant(DocumentFilingPM.DocumentTypeCode, ImporterTenant);
        //                    if (temp != null)
        //                    {
        //                        DocumentTypeId = temp.Id;
        //                    }


        //                    using (var client = new HttpClient())
        //                    {
        //                        client.DefaultRequestHeaders.Add("Token", Token);
        //                        client.DefaultRequestHeaders.Add("CorrelationId", CorrelationId);
        //                        string ImporterShipmentDocumentsURI = URI + "ImporterShipmentDocuments";

        //                        #region Get Document File
        //                        if (DocumentFilingPM.HasFile)
        //                        {
        //                            string fileName = DocumentFilingPM.DocumentId + "." + DocumentFilingPM.FileExtension;
        //                            string filePath = "tenant" + Tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), DocumentFilingPM.Folder);
        //                            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
        //                            Logitude.Server.Tools.BlobServiceReference.Response Docresponse = storageservice.Read(filePath);
        //                            if (Docresponse.Result != null)
        //                            {
        //                                datainByte = Docresponse.Result as byte[];
        //                            }
        //                        }
        //                        #endregion
        //                        Objecttable = objectTabelRepository.GetSingleObjectTable(DocumentFilingPM.ObjectTableId, Tenant, true);
        //                        if (IsNew)
        //                        {
        //                            #region New Document
        //                            #region DocumentsFilingAMProperties
        //                            DocumentsFilingAM NewDocumentFilingAM = new DocumentsFilingAM()
        //                            {
        //                                ImporterTenant = ImporterTenant,
        //                                ForwarderDocumentId = DocumentFilingPM.Id,
        //                                EntityId = ImporterShipmentId,
        //                                DontAddToQueue = true,
        //                                DocumentType = new CodeProperties()
        //                                {
        //                                    Code = DocumentFilingPM.DocumentTypeCode
        //                                },
        //                                DocumentsFilingMetaDataValues = DocumentFilingPM.DocumentsFilingMetaDataValues,
        //                                Description = DocumentFilingPM.Description,
        //                                FileSize = datainByte.Length,
        //                                ObjectTableName = Objecttable.Name,
        //                                FileData = datainByte,
        //                                Extension = DocumentFilingPM.FileExtension,
        //                                IsDigitallySigned = DocumentFilingPM.IsDigitallySigned,
        //                                SignersList = DocumentFilingPM.SignersList

        //                            };

        //                            #endregion

        //                            #region Calling API and Back From It
        //                            LogPM.Subject = "Send New Document To Importer By ImporterShipmentDocuments Controller";
        //                            if (IsNewLog)
        //                            {
        //                                apiLogsService = new APILogsService(webFreightContext, Tenant);
        //                                apiLogsService.Create(LogPM);
        //                            }
        //                            var msg = "Start Sending New Document To Importer " + DateTime.Now;
        //                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, Tenant, LogPM.Status, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(NewDocumentFilingAM), null, null, "");

        //                            var serializedObject = JsonConvert.SerializeObject(NewDocumentFilingAM);
        //                            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //                            var result = await client.PostAsync(ImporterShipmentDocumentsURI, content);
        //                            if (result.StatusCode == System.Net.HttpStatusCode.OK)
        //                            {
        //                                var temp1 = result.Content.ReadAsStringAsync().Result;
        //                                var Donemsg = "New Document Sent To Importer Successfully " + DateTime.Now;
        //                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, Tenant, "D", DateTime.Now, DateTime.UtcNow, Donemsg, null, temp1, null, "");


        //                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, Tenant, "I", DateTime.Now, DateTime.UtcNow, "Start Update CustomerDocumentId In Forwarder Document " + DateTime.Now, null, null, null, "");
        //                                try
        //                                {
        //                                    string ImporterDocId = JsonConvert.DeserializeObject<string>(temp1);
        //                                    var DocFilingPM = documentsFilingQuery.GetSinglePM(DocumentFilingId, Tenant);
        //                                    DocFilingPM.CustomerDocumentId = ImporterDocId;
        //                                    DocFilingPM.DontAddToQueue = true;
        //                                    ICommonDataContext objectContext = CommonDataContext.GetContext(DocFilingPM.Tenant);
        //                                    DocumentsFilingService documentsFilingService = new DocumentsFilingService(objectContext, DocFilingPM.Tenant);
        //                                    documentsFilingService.Update(DocFilingPM, null, User.Id);
        //                                    //queueservice.Complete();
        //                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, Tenant, "D", DateTime.Now, DateTime.UtcNow, "Forwarder Document Updated Successfully" + DateTime.Now, null, null, null, "");

        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    var Failmsg = ex.Message + " Fail To Update Forwarder Document " + DateTime.Now;
        //                                    string errorMessage = ex.Message + Environment.NewLine;

        //                                    if (ex.InnerException != null)
        //                                    {

        //                                        errorMessage = errorMessage + " (" + ex.InnerException.Message + ")" + Environment.NewLine;

        //                                    }

        //                                    errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
        //                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, Tenant, "F", DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
        //                                    throw (ex);
        //                                }
        //                            }
        //                            else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
        //                            {
        //                                var temp1 = result.Content.ReadAsStringAsync().Result;
        //                                APIException EXC = JsonConvert.DeserializeObject<APIException>(temp1);
        //                                if (EXC != null)
        //                                {
        //                                    var Failmsg = EXC.ErrorType + " Fail To Send New Document To Importer " + DateTime.Now;
        //                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, Tenant, "F", DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
        //                                    throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
        //                                }
        //                            }
        //                            #endregion
        //                            #endregion
        //                        }
        //                        else
        //                        {
        //                            #region Edit Document
        //                            #region DocumentFilingPMProperties

        //                            DocumentsFilingAM NewDocumentFilingAM = new DocumentsFilingAM()
        //                            {
        //                                ImporterTenant = ImporterTenant,
        //                                EntityId = ImporterShipmentId,
        //                                DontAddToQueue = true,
        //                                DocumentType = new CodeProperties()
        //                                {
        //                                    Code = DocumentFilingPM.DocumentTypeCode
        //                                },
        //                                DocumentsFilingMetaDataValues = DocumentFilingPM.DocumentsFilingMetaDataValues,
        //                                Description = DocumentFilingPM.Description,
        //                                FileSize = datainByte.Length,
        //                                ObjectTableName = Objecttable.Name,
        //                                FileData = datainByte,
        //                                CustomerDocumentId = DocumentFilingPM.CustomerDocumentId,
        //                                Extension = DocumentFilingPM.FileExtension,
        //                                IsDigitallySigned = DocumentFilingPM.IsDigitallySigned,
        //                                SignersList = DocumentFilingPM.SignersList


        //                            };

        //                            #endregion

        //                            #region Calling API and Back From It
        //                            LogPM.Subject = "Send Document Updates To Importer ";
        //                            if (IsNewLog)
        //                            {
        //                                apiLogsService = new APILogsService(webFreightContext, Tenant);
        //                                apiLogsService.Create(LogPM);
        //                            }
        //                            var msg = "Start Sending Document Updates To Importer " + DateTime.Now;
        //                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, Tenant, "I", DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(NewDocumentFilingAM), null, null, "");

        //                            var serializedObject = JsonConvert.SerializeObject(NewDocumentFilingAM);
        //                            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //                            var result = await client.PutAsync(ImporterShipmentDocumentsURI, content);
        //                            if (result.StatusCode == System.Net.HttpStatusCode.OK)
        //                            {
        //                                var temp1 = result.Content.ReadAsStringAsync().Result;
        //                                var Donemsg = "Document Updates Sent To Importer Successfully " + DateTime.Now;
        //                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, Tenant, "D", DateTime.Now, DateTime.UtcNow, Donemsg, null, temp1, null, "");
        //                                //queueservice.Complete();
        //                            }
        //                            else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
        //                            {
        //                                var temp1 = result.Content.ReadAsStringAsync().Result;
        //                                APIException EXC = JsonConvert.DeserializeObject<APIException>(temp1);
        //                                if (EXC != null)
        //                                {
        //                                    var Failmsg = EXC.ErrorType + " Fail To Send Document Updates To Importer " + DateTime.Now;
        //                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, Tenant, "F", DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
        //                                    throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
        //                                }
        //                            }
        //                            #endregion
        //                            #endregion
        //                        }
        //                    }

        //                }
        //                catch (Exception ex)
        //                {
        //                    var Failmsg = ex.Message + " " + DateTime.Now;
        //                    string errorMessage = ex.Message + Environment.NewLine;

        //                    if (ex.InnerException != null)
        //                    {
        //                         errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
        //                    }
        //                    errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
        //                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, Tenant, "F", DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
        //                    if (Retries <= 1)
        //                    {
        //                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, Tenant, "I", DateTime.Now, DateTime.UtcNow, Failmsg, null, null, null, null);
        //                        Retries++;
        //                    }

        //                    if (Retries > 1 && Retries <= 2)
        //                    {
        //                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, Tenant, "I", DateTime.Now, DateTime.UtcNow, Failmsg, null, null, null, null);
        //                        Retries++;
        //                    }
        //                    if (Retries >= 3)
        //                    {

        //                    }
        //                }
        //            }
        //        }                 
        //    }
            
        //}

        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("ImportersShipmentScheduleQueue", Tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }
        }
    }
}
