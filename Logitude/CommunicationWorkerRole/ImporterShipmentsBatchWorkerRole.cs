using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Logitude.BL.ShipmentsModel.EntityAMs;
using Logitude.Server.Tools;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.DataContracts;
using System.Web;
using System.Runtime.Remoting.Messaging;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace CommunicationWorkerRole
{
    public class ImporterShipmentsBatchWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
        int Tenant;
        string URI = "";//"http://localhost:9996";
        APILogsService apiLogsService;
        IWebFreightContext webFreightContext;

        public ImporterShipmentsBatchWorkerRole(string tenant)
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
            BatchServiceCode = "ImporterShipments";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            ConnectClient();
            return base.OnStart();
        }
        string Token;
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
                            queueservice.InitializeQueue("ImportersShipmentsBatchQueue", Tenant);
                            var response = queueservice.Receive();
                            LastActivity = DateTime.UtcNow;
                            int tenant = 0;
                            int importerTenant = 0;

                            if (response != null && response.MessageId != null)
                            {

                                string ShipmentId = response.MessageValues["ShipmentId"].ToString();
                                int.TryParse(response.MessageValues["Tenant"], out tenant);
                                int.TryParse(response.MessageValues["ImporterTenant"], out importerTenant);
                                string CorrelationId = response.MessageId;
                                string BatchNumber = response.MessageValues["BatchNumber"].ToString();
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
                                        QueueMessageMoreDetailsId = response.MessageId,
                                        Tenant = tenant
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
                                ICommonDataContext Context = CommonDataContext.GetContext(tenant);
                                CustomerTenantAccessCardBatchQuery customerTenantAccessCardBatchQuery = new CustomerTenantAccessCardBatchQuery(tenant);
                                var customerTenantAccessCardsBatch = customerTenantAccessCardBatchQuery.GetSinglePM(BatchNumber, tenant);
                                string CustomerId = customerTenantAccessCardsBatch.CustomerId;
                                try
                                {
                                    if (!string.IsNullOrEmpty(ShipmentId))
                                    {

                                        apiLogsService = new APILogsService(webFreightContext, tenant);
                                        ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                                        ShipmentQuery shipmentQuery = new ShipmentQuery(Tenant);
                                        var Shipment = shipmentQuery.GetSinglePM(ShipmentId, tenant);
                                        if (Shipment != null)
                                        {
                                            //var tenantQuery = new TenantQuery(tenant);
                                            //var tenantPM = tenantQuery.GetSinglePM(tenant);
                                            ////if (tenantPM == null)
                                            ////{
                                            ////    tenantQuery = new TenantQuery(tenant);
                                            ////    tenantPM = tenantQuery.GetSinglePM(tenant);
                                            ////}

                                            //if (tenantPM.IsCustomerTenantShare && (tenantPM.CustomerTenantShareImportFile ? Shipment.DirectionId.ToUpper() == "I" || Shipment.DirectionId.ToUpper() == "C" : Shipment.DirectionId.ToUpper() == "C"))
                                            //{
                                            //    CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                                            //    CustomerTenantAccessInfo customerTenantAccessInfo = customerTenantAccessQuery.GetCustomerTenantAccessInfo(tenant, Shipment.CustomerId);

                                            //    if (customerTenantAccessInfo != null && customerTenantAccessInfo.HasAccess)
                                            //    {


                                            var GetURI = URI + "ImporterShipmentsBatch/GetIfShipmentExists?importertenant=" + importerTenant + "&Tenant=" + tenant + "&shipmentnumber=" + Shipment.ShipmentNumber;
                                            bool IsShipmentExist = false;
                                            using (var client = new HttpClient())
                                            {
                                                client.DefaultRequestHeaders.Add("Token", Token);
                                                using (var apiresponse = client.GetAsync(GetURI))
                                                {
                                                    apiresponse.Wait();
                                                    if (apiresponse.Result.IsSuccessStatusCode)
                                                    {

                                                        var IsShipmentExistJsonString = apiresponse.Result.Content.ReadAsStringAsync().Result;
                                                        var tempResult = JsonConvert.DeserializeObject(IsShipmentExistJsonString);
                                                        if (tempResult != null)
                                                        {
                                                            IsShipmentExist = (bool)tempResult;
                                                        }
                                                    }
                                                }
                                            }
                                            var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);
                                            LogPM.ObjectTableId = Objecttable.Id;
                                            LogPM.EntityId = Shipment.Id;
                                            LogPM.Refrence = Shipment.ShipmentNumber;
                                            LogPM.Tenant = Shipment.Tenant;
                                            using (var client = new HttpClient())
                                            {
                                                string ImporterShipmentsURI = URI + "ImporterShipmentsBatch";
                                                client.DefaultRequestHeaders.Add("Token", Token);
                                                client.DefaultRequestHeaders.Add("CorrelationId", CorrelationId);
                                                ICommonDataContext commoncontext = CommonDataContext.GetContext(Shipment.Tenant);
                                                CardRepository cardsReporistory = new CardRepository(commoncontext);
                                                BranchRepository branchRepository = new BranchRepository(commoncontext);
                                                DepartmentRepository departmentRepository = new DepartmentRepository(commoncontext);
                                                HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commoncontext);
                                                HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(hybridPartnerRepository);
                                                Card Shipper = cardsReporistory.GetSingleCard(Shipment.ShipperId, Shipment.Tenant);
                                                Card Consignee = cardsReporistory.GetSingleCard(Shipment.ConsigneeId, Shipment.Tenant);
                                                Card Customer = cardsReporistory.GetSingleCard(Shipment.CustomerId, Shipment.Tenant);
                                                Branch Branch = branchRepository.GetSingleBranch(Shipment.BranchId, Shipment.Tenant);
                                                Department Department = departmentRepository.GetSingleDepartment(Shipment.DepartmentId, Shipment.Tenant);
                                                HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePMByPartnerTenant(tenant);
                                                TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
                                                EntityStatus status = EntityStatusRepository.GetSingleEntityStatus(Shipment.StatusId, Shipment.Tenant, true);
                                                bool TakeDate = false;
                                                CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                                                var customerTenantAccess = customerTenantAccessQuery.GetCustomerTenantAccessPMsByTenantCustomerTenant(tenant, importerTenant);
                                                if (customerTenantAccess != null)
                                                {
                                                    LogPM.PartnerName = customerTenantAccess.CompanyName + " ( " + customerTenantAccess.CustomerTenant + " )";
                                                }
                                                string ShipperCode = "";
                                                string ConsigneeCode = "";
                                                string BranchCode = "";
                                                string DepartmentCode = "";
                                                string CustomerCode = "";
                                                if (Shipper != null)
                                                {
                                                    ShipperCode = Shipper.Code;
                                                }
                                                if (Consignee != null)
                                                {
                                                    ConsigneeCode = Consignee.Code;
                                                }
                                                if (Customer != null)
                                                {
                                                    CustomerCode = Customer.Code;
                                                }
                                                if (Branch != null)
                                                {
                                                    BranchCode = Branch.Code;
                                                }
                                                if (Department != null)
                                                {
                                                    DepartmentCode = Department.Code;
                                                }
                                                var eventsQuery = new TraceEventQuery(tenant);
                                                var IsCustomsCleared = eventsQuery.CheckIfEntityIsCustomsCleared(tenant, Shipment.Id, Objecttable.Id);


                                                string statusCode = "";
                                                DateTime statusDate;
                                                string origionalstatusCode = "";
                                                DateTime? origionalstatusDate;
                                                var Code = eventsQuery.GetStatusCodeById(tenant, Shipment.Id, Shipment.StatusId);
                                                if (!string.IsNullOrEmpty(Code))
                                                {
                                                    origionalstatusCode = Code;
                                                    origionalstatusDate = Shipment.StatusDate;
                                                }
                                                else
                                                {
                                                    origionalstatusCode = "OPOP";
                                                    origionalstatusDate = DateTime.Now;
                                                }
                                                if (IsCustomsCleared != null)
                                                {
                                                    statusCode = "CCD";
                                                    statusDate = IsCustomsCleared.EventDateTime;
                                                }
                                                else
                                                {
                                                    var eventtype = eventsQuery.getCreatedEvent(tenant, Shipment.Id);
                                                    statusCode = "OPOP";
                                                    if (eventtype != null)
                                                    {
                                                        statusDate = eventtype.EventDateTime;
                                                    }
                                                    else
                                                    {
                                                        statusDate = DateTime.Now;
                                                    }
                                                }


                                                //if (status != null)
                                                //{
                                                //    if (status.Code.ToLower() == "rsh")
                                                //    {
                                                //        statusCode = status.Code;
                                                //        TakeDate = true;
                                                //    }
                                                //    else
                                                //    {
                                                //        if (status.Code.ToLower() == "opop")
                                                //        {
                                                //            TakeDate = true;
                                                //        }
                                                //        else
                                                //        {
                                                //            TakeDate = false;
                                                //        }
                                                //        statusCode = "OPOP";

                                                //    }
                                                //}
                                                //else
                                                //{
                                                //    statusCode = "";
                                                //}
                                                if (IsShipmentExist)
                                                {
                                                    //Update 
                                                    ShipmentAM shipmentAM = new ShipmentAM()
                                                    {
                                                        Id = Shipment.Id,
                                                        OriginalStatusCode = origionalstatusCode,
                                                        OriginalStatusDate = origionalstatusDate,
                                                        ImporterTenant = importerTenant,
                                                        Tenant = Shipment.Tenant,
                                                        MainCarriageATA = Shipment.MainCarriageATA,
                                                        MainCarriageETA = Shipment.MainCarriageETA,
                                                        MainCarriageATD = Shipment.MainCarriageATD,
                                                        MainCarriageETD = Shipment.MainCarriageETD,
                                                        OnCarriageATA = Shipment.OnCarriageATA,
                                                        OnCarriageATD = Shipment.OnCarriageATD,
                                                        PreCarriageATA = Shipment.PreCarriageATA,
                                                        PreCarriageATD = Shipment.PreCarriageATD,
                                                        StatusCode = statusCode,
                                                        StatusDate = statusDate,
                                                        TransportModeId = Shipment.TransportModeId,
                                                        DirectionId = Shipment.DirectionId,
                                                        ShipmentLevelCode = Shipment.ShipmentLevelCode,
                                                        ForwarderShipmentNumber = Shipment.ShipmentNumber,
                                                        CustomerShipmentNumber = Shipment.CustomerShipmentNumber,
                                                        ShipmentTypeId = Shipment.ShipmentTypeId,
                                                        House = Shipment.House,
                                                        DescriptionOfGoods = Shipment.DescriptionOfGoods,
                                                        ConsigneeReference1 = Shipment.ConsigneeReference1,
                                                        CustomerReference1 = Shipment.CustomerReference1,
                                                        ConsigneeReference2 = Shipment.ConsigneeReference2,
                                                        CustomerReference2 = Shipment.CustomerReference2,
                                                        CustomerReference3 = Shipment.CustomerReference3,
                                                        ShipmentCustomerTypeCode = Shipment.ShipmentCustomerTypeCode,
                                                        IsCancelled = Shipment.IsCancelled,
                                                        ShipperName = Shipment.ShipperName,
                                                        CarrierTransportDocumentNumber = Shipment.CarrierTransportDocumentNumber,
                                                        //ForwarderPartnerId = Partner.Id,
                                                        FreightPrepaidCollectId = Shipment.FreightPrepaidCollectId,
                                                        OtherPrepaidCollectId = Shipment.OtherPrepaidCollectId,
                                                        HasException = Shipment.HasException,
                                                        ExceptionDate = Shipment.ExceptionDate,
                                                        ExceptionDescription = Shipment.ExceptionDescription,
                                                        //IsOperationalClosed = Shipment.IsOperationalClosed,
                                                        DeclarationXMLData = Shipment.DeclarationXMLData,
                                                        IsImporterApprovalRequired = Shipment.IsImporterApprovalRequired,
                                                        VersionApproved = Shipment.VersionApproved,
                                                        ApproveDateTime = Shipment.ApproveDateTime,
                                                        ShipmentAddtionalDataXML = Shipment.ShipmentAddtionalDataXML,
                                                        Master = Shipment.Master,
                                                        Quantity = Shipment.PackagesQuantity,
                                                        Weight = Shipment.GrossWeight,
                                                        CustomsClearanceDate = Shipment.CustomsClearanceDate,
                                                        ChargeableWeightUnitCode = Shipment.ChargeableWeightUnitCode,
                                                        GrossWeightUnitCode = Shipment.GrossWeightUnitCode,
                                                        DimensionsUnitCode = Shipment.DimensionsUnitCode,
                                                        VolumeUnitCode = Shipment.VolumeUnitCode,
                                                        ForwardingPartnerTenant = Shipment.ForwardingPartnerId,
                                                        Customer = new CodeProperties()
                                                        {
                                                            Code = CustomerCode
                                                        },
                                                        Branch = new CodeProperties()
                                                        {
                                                            Code = BranchCode
                                                        },
                                                        Department = new CodeProperties()
                                                        {
                                                            Code = DepartmentCode
                                                        },
                                                        Shipper = new CodeProperties()
                                                        {
                                                            Code = ShipperCode
                                                        },
                                                        Consignee = new CodeProperties()
                                                        {
                                                            Code = ConsigneeCode
                                                        },
                                                        FromPort = new CodeProperties()
                                                        {
                                                            Code = Shipment.FromPort,
                                                            CountryCode = Shipment.FromCountryCode
                                                        },
                                                        ToPort = new CodeProperties()
                                                        {
                                                            Code = Shipment.ToPort,
                                                            CountryCode = Shipment.ToCountryCode
                                                        },
                                                        PreCarriageFromPort = new CodeProperties()
                                                        {
                                                            Code = Shipment.PreCarriageFromPortCode,
                                                            CountryCode = Shipment.PreCarriageFromPortCountryCode
                                                        },
                                                        PreCarriageToPort = new CodeProperties()
                                                        {
                                                            Code = Shipment.PreCarriageToPortCode,
                                                            CountryCode = Shipment.PreCarriageToPortCountryCode
                                                        },
                                                        OnCarriageToPort = new CodeProperties()
                                                        {
                                                            Code = Shipment.OnCarriageToPortCode,
                                                            CountryCode = Shipment.OnCarriageToPortCountryCode
                                                        },
                                                        //Incoterm = new CodeProperties()
                                                        //{
                                                        //    Code = Shipment.IncotermCode
                                                        //}

                                                    };
                                                    if (!string.IsNullOrEmpty(Shipment.IncotermCode))
                                                    {
                                                        shipmentAM.Incoterm = new CodeProperties()
                                                        {
                                                            Code = Shipment.IncotermCode
                                                        };
                                                    }
                                                    shipmentAM.ShipmentPackagesAM = new List<ShipmentPackageAM>();
                                                    foreach (var package in Shipment.ShipmentPackages)
                                                    {
                                                        var MyPackage = new ShipmentPackageAM();
                                                        MyPackage.ContainerNumber = package.ContainerNumber;
                                                        MyPackage.PackageTypeId = package.PackageTypeId;
                                                        MyPackage.PackageTypeCode = package.PackageTypeCode;
                                                        MyPackage.Quantity = package.Quantity;
                                                        MyPackage.Weight = package.Weight;
                                                        MyPackage.ShipperSeal = package.ShipperSeal;
                                                        MyPackage.Volume = package.Volume;
                                                        if (string.IsNullOrEmpty(package.PackageTypeCode))
                                                        {
                                                            PackageTypeRepository packageTypeRep = new PackageTypeRepository(commoncontext);
                                                            PackageType type = packageTypeRep.GetSinglePackageTypeByCode(package.PackageTypeId, Shipment.Tenant, true);

                                                            if (type != null)
                                                            {
                                                                MyPackage.PackageTypeCode = type.Code;
                                                            }
                                                        }

                                                        shipmentAM.ShipmentPackagesAM.Add(MyPackage);
                                                    }
                                                    //if (TakeDate)
                                                    //{
                                                    //    shipmentAM.StatusDate = Shipment.StatusDate;
                                                    //}
                                                    //else
                                                    //{
                                                    //    shipmentAM.StatusDate = DateTime.MinValue;
                                                    //}

                                                    LogPM.Subject = "Send updates Of Shipment To Importer By ImporterShipments Controller";
                                                    if (IsNewLog)
                                                    {
                                                        LogPM.CustomerId = customerTenantAccessCardsBatch.CustomerId;
                                                        LogPM.BatchNumber = customerTenantAccessCardsBatch.BatchNumber;
                                                        LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                                        LogPM.QueueType = "Shipment";
                                                        apiLogsService.Create(LogPM);
                                                    }
                                                    var msg = "Start Sending Updates Of Shipment To Importer" + DateTime.Now;
                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(shipmentAM), null, null, "");

                                                    var serializedObject = JsonConvert.SerializeObject(shipmentAM);
                                                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                                    var result = client.PutAsync(ImporterShipmentsURI, content);
                                                    result.Wait();
                                                    if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                                    {
                                                        var temp = result.Result.Content.ReadAsStringAsync().Result;
                                                        List<string> ImporterShipmentNoId = JsonConvert.DeserializeObject<List<string>>(temp);
                                                        var ForwarderShipment = shipmentQuery.GetSinglePMWithoutComposition(ShipmentId, tenant);
                                                        try
                                                        {

                                                            if (shipmentAM.IsCancelled)
                                                            {
                                                                ForwarderShipment.CustomerShipmentNumber = null;
                                                            }
                                                            else
                                                            {
                                                                ForwarderShipment.CustomerShipmentNumber = ImporterShipmentNoId[1];
                                                            }
                                                            ForwarderShipment.CustomerTenantNumber = importerTenant;
                                                            ForwarderShipment.DontAddToImportersQueue = true;
                                                            ForwarderShipment.IsHybrid = true;
                                                            var objectContext = ShipmentsContext.GetContext(ForwarderShipment.Tenant);
                                                            string systemEmail = "system@tenant" + ForwarderShipment.Tenant + ".com";
                                                            var shipmentService = new ShipmentService(objectContext, ForwarderShipment, systemEmail);
                                                            shipmentService.Update();
                                                            customerTenantAccessCardsBatch = customerTenantAccessCardBatchQuery.GetSinglePM(BatchNumber, tenant);
                                                            customerTenantAccessCardsBatch.Totalsucceeded++;
                                                            CustomerTenantAccessCardsBatchService customerTenantAccessCardsBatchService = new CustomerTenantAccessCardsBatchService(Context, tenant, customerTenantAccessCardsBatch);
                                                            customerTenantAccessCardsBatchService.Update();
                                                            queueservice.Complete();

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string errorMessage = ex.Message + Environment.NewLine;

                                                            if (ex.InnerException != null)
                                                            {

                                                                errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                                                            }

                                                            errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                                            throw new Exception(ex.Message, new Exception(errorMessage));
                                                        }

                                                        queueservice.InitializeQueue("ImportersShipmentsDocsQueueBuilderQueue", 0);

                                                        queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", Shipment.Id }, { "Tenant", tenant.ToString() }, { "CustomerId", customerTenantAccessCardsBatch.CustomerId }, { "BatchNumber", customerTenantAccessCardsBatch.BatchNumber } }, tenant, null, customerTenantAccessCardsBatch.CustomerId, customerTenantAccessCardsBatch.BatchNumber);

                                                        var ResponseData = result.Result.Content.ReadAsStringAsync().Result;
                                                        var Donemsg = "Updates Of Shipment Sent To Importer Successfully , Total Succeeded = " + customerTenantAccessCardsBatch.Totalsucceeded + " " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, ResponseData, null, "");
                                                        //if (response.RetryNumber == 0)
                                                        //{

                                                        //}
                                                    }
                                                    else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                                    {
                                                        APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
                                                        if (EXC != null)
                                                        {
                                                            var Failmsg = EXC.ErrorType + " Fail To Send Shipment Updates To Importer Tenant " + DateTime.Now;
                                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                                            throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                                        }
                                                    }
                                                    //else
                                                    //{
                                                    //    Exception EXC = JsonConvert.DeserializeObject<Exception>(result.Content.ReadAsStringAsync().Result);
                                                    //    var Failmsg = EXC.Message;
                                                    //    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", DateTime.Now, DateTime.UtcNow, Failmsg + DateTime.Now, null, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), EXC.Message.Substring(0, 249));
                                                    //    throw EXC;

                                                    //}
                                                }
                                                else
                                                {
                                                    //Insert
                                                    ShipmentAM shipmentAM = new ShipmentAM()
                                                    {
                                                        Id = Shipment.Id,
                                                        ImporterTenant = importerTenant,
                                                        Tenant = Shipment.Tenant,
                                                        OriginalStatusCode = origionalstatusCode,
                                                        OriginalStatusDate = origionalstatusDate,
                                                        MainCarriageATA = Shipment.MainCarriageATA,
                                                        MainCarriageETA = Shipment.MainCarriageETA,
                                                        MainCarriageATD = Shipment.MainCarriageATD,
                                                        MainCarriageETD = Shipment.MainCarriageETD,
                                                        OnCarriageATA = Shipment.OnCarriageATA,
                                                        OnCarriageATD = Shipment.OnCarriageATD,
                                                        PreCarriageATA = Shipment.PreCarriageATA,
                                                        PreCarriageATD = Shipment.PreCarriageATD,
                                                        StatusCode = statusCode,
                                                        StatusDate = statusDate,
                                                        TransportModeId = Shipment.TransportModeId,
                                                        DirectionId = Shipment.DirectionId,
                                                        ShipmentLevelCode = Shipment.ShipmentLevelCode,
                                                        ForwarderShipmentNumber = Shipment.ShipmentNumber,
                                                        ShipmentTypeId = Shipment.ShipmentTypeId,
                                                        House = Shipment.House,
                                                        DescriptionOfGoods = Shipment.DescriptionOfGoods,
                                                        ConsigneeReference1 = Shipment.ConsigneeReference1,
                                                        CustomerReference1 = Shipment.CustomerReference1,
                                                        ConsigneeReference2 = Shipment.ConsigneeReference2,
                                                        CustomerReference2 = Shipment.CustomerReference2,
                                                        CustomerReference3 = Shipment.CustomerReference3,
                                                        ShipmentCustomerTypeCode = Shipment.ShipmentCustomerTypeCode,
                                                        IsCancelled = Shipment.IsCancelled,
                                                        ShipperName = Shipment.ShipperName,
                                                        ConsigneeName = Shipment.ConsigneeName,
                                                        CarrierTransportDocumentNumber = Shipment.CarrierTransportDocumentNumber,
                                                        //ForwarderPartnerId = Partner.Id,
                                                        FreightPrepaidCollectId = Shipment.FreightPrepaidCollectId,
                                                        OtherPrepaidCollectId = Shipment.OtherPrepaidCollectId,
                                                        HasException = Shipment.HasException,
                                                        ExceptionDate = Shipment.ExceptionDate,
                                                        ExceptionDescription = Shipment.ExceptionDescription,
                                                        Master = Shipment.Master,
                                                        //IsOperationalClosed = Shipment.IsOperationalClosed,
                                                        Quantity = Shipment.PackagesQuantity,
                                                        Weight = Shipment.GrossWeight,
                                                        CustomsClearanceDate = Shipment.CustomsClearanceDate,
                                                        ChargeableWeightUnitCode = Shipment.ChargeableWeightUnitCode,
                                                        GrossWeightUnitCode = Shipment.GrossWeightUnitCode,
                                                        DimensionsUnitCode = Shipment.DimensionsUnitCode,
                                                        VolumeUnitCode = Shipment.VolumeUnitCode,
                                                        ForwardingPartnerTenant = Shipment.ForwardingPartnerId,
                                                        Customer = new CodeProperties()
                                                        {
                                                            Code = CustomerCode
                                                        },
                                                        Branch = new CodeProperties()
                                                        {
                                                            Code = BranchCode
                                                        },
                                                        Department = new CodeProperties()
                                                        {
                                                            Code = DepartmentCode
                                                        },
                                                        Shipper = new CodeProperties()
                                                        {
                                                            Code = ShipperCode
                                                        },
                                                        Consignee = new CodeProperties()
                                                        {
                                                            Code = ConsigneeCode
                                                        },
                                                        FromPort = new CodeProperties()
                                                        {
                                                            Code = Shipment.FromPort,
                                                            CountryCode = Shipment.FromCountryCode
                                                        },
                                                        ToPort = new CodeProperties()
                                                        {
                                                            Code = Shipment.ToPort,
                                                            CountryCode = Shipment.ToCountryCode
                                                        },
                                                        PreCarriageFromPort = new CodeProperties()
                                                        {
                                                            Code = Shipment.PreCarriageFromPortCode,
                                                            CountryCode = Shipment.PreCarriageFromPortCountryCode
                                                        },
                                                        PreCarriageToPort = new CodeProperties()
                                                        {
                                                            Code = Shipment.PreCarriageToPortCode,
                                                            CountryCode = Shipment.PreCarriageToPortCountryCode
                                                        },
                                                        OnCarriageToPort = new CodeProperties()
                                                        {
                                                            Code = Shipment.OnCarriageToPortCode,
                                                            CountryCode = Shipment.OnCarriageToPortCountryCode
                                                        },
                                                        //Incoterm = new CodeProperties()
                                                        //{
                                                        //    Code = Shipment.IncotermCode
                                                        //}
                                                    };
                                                    if (!string.IsNullOrEmpty(Shipment.IncotermCode))
                                                    {
                                                        shipmentAM.Incoterm = new CodeProperties()
                                                        {
                                                            Code = Shipment.IncotermCode
                                                        };
                                                    }
                                                    shipmentAM.ShipmentPackagesAM = new List<ShipmentPackageAM>();
                                                    foreach (var package in Shipment.ShipmentPackages)
                                                    {
                                                        var MyPackage = new ShipmentPackageAM();
                                                        MyPackage.ContainerNumber = package.ContainerNumber;
                                                        MyPackage.PackageTypeId = package.PackageTypeId;
                                                        MyPackage.PackageTypeCode = package.PackageTypeCode;
                                                        MyPackage.Quantity = package.Quantity;
                                                        MyPackage.Weight = package.Weight;
                                                        MyPackage.ShipperSeal = package.ShipperSeal;
                                                        MyPackage.Volume = package.Volume;
                                                        if (string.IsNullOrEmpty(package.PackageTypeCode))
                                                        {
                                                            PackageTypeRepository packageTypeRep = new PackageTypeRepository(commoncontext);
                                                            PackageType type = packageTypeRep.GetSinglePackageTypeByCode(package.PackageTypeId, Shipment.Tenant, true);

                                                            if (type != null)
                                                            {
                                                                MyPackage.PackageTypeCode = type.Code;
                                                            }
                                                        }

                                                        shipmentAM.ShipmentPackagesAM.Add(MyPackage);
                                                    }
                                                    //if (TakeDate)
                                                    //{
                                                    //    shipmentAM.StatusDate = Shipment.StatusDate;
                                                    //}
                                                    //else
                                                    //{
                                                    //    shipmentAM.StatusDate = null;
                                                    //}

                                                    LogPM.Subject = "Send Shipment To Importers By ImporterShipments Controller";
                                                    if (IsNewLog)
                                                    {
                                                        LogPM.CustomerId = customerTenantAccessCardsBatch.CustomerId;
                                                        LogPM.BatchNumber = customerTenantAccessCardsBatch.BatchNumber;
                                                        LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                                        LogPM.QueueType = "Shipment";
                                                        apiLogsService.Create(LogPM);
                                                    }
                                                    var msg = "Start Sending Shipment To Importers " + DateTime.Now;
                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, LogPM.Status, response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(shipmentAM), null, null, "");


                                                    var serializedObject = JsonConvert.SerializeObject(shipmentAM);
                                                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                                    var result = client.PostAsync(ImporterShipmentsURI, content);
                                                    result.Wait();
                                                    if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                                    {
                                                        var temp = result.Result.Content.ReadAsStringAsync().Result;
                                                        List<string> ImporterShipmentNoId = JsonConvert.DeserializeObject<List<string>>(temp);

                                                        var Donemsg = "Shipment Sent To Importer Successfully " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, temp, null, "");

                                                        // Update Forwarder Shipment
                                                        msg = "Update CustomerShipmentNumber Of Forwarder Shipment " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");

                                                        //var ImporterShipment = shipmentQuery.GetSinglePMWithoutComposition(ImporterShipmentId, importerTenant);
                                                        var ForwarderShipment = shipmentQuery.GetSinglePMWithoutComposition(ShipmentId, tenant);
                                                        string systemEmail = "system@tenant" + ForwarderShipment.Tenant + ".com";
                                                        try
                                                        {

                                                            if (shipmentAM.IsCancelled)
                                                            {
                                                                ForwarderShipment.CustomerShipmentNumber = null;
                                                            }
                                                            else
                                                            {
                                                                ForwarderShipment.CustomerShipmentNumber = ImporterShipmentNoId[1];
                                                            }
                                                            ForwarderShipment.CustomerTenantNumber = importerTenant;
                                                            ForwarderShipment.DontAddToImportersQueue = true;
                                                            ForwarderShipment.IsHybrid = true;
                                                            var objectContext = ShipmentsContext.GetContext(ForwarderShipment.Tenant);

                                                            var shipmentService = new ShipmentService(objectContext, ForwarderShipment, systemEmail);
                                                            shipmentService.Update();

                                                            msg = "Update CustomerShipmentNumber Of Forwarder Shipment Completed Successfully " + DateTime.Now;
                                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string errorMessage = ex.Message + Environment.NewLine;

                                                            if (ex.InnerException != null)
                                                            {

                                                                errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                                                            }

                                                            errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                                            throw new Exception(ex.Message, new Exception(errorMessage));
                                                        }




                                                        // Update Last Shipment Date
                                                        msg = "Update LastShipmentDate in Forwarder CustomerTenantAccess  " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");

                                                        CustomerTenantAccessCardQuery customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(ForwarderShipment.Tenant);
                                                        customerTenantAccessQuery = new CustomerTenantAccessQuery(ForwarderShipment.Tenant);
                                                        var tempCard = customerTenantAccessCardQuery.GetSingleCustomerTenantAccessCardPMById(ForwarderShipment.CustomerId, ForwarderShipment.Tenant);
                                                        customerTenantAccess = customerTenantAccessQuery.GetSinglePM(tempCard.CustomerTenantAccessId, ForwarderShipment.Tenant);
                                                        customerTenantAccess.LastShipmentDate = DateTime.Now;
                                                        CustomerTenantAccessService customerTenantAccessService = new CustomerTenantAccessService(Context, ForwarderShipment.Tenant, customerTenantAccess, systemEmail);
                                                        customerTenantAccessService.Update();
                                                        customerTenantAccessCardsBatch = customerTenantAccessCardBatchQuery.GetSinglePM(BatchNumber, tenant);
                                                        customerTenantAccessCardsBatch.Totalsucceeded++;
                                                        CustomerTenantAccessCardsBatchService customerTenantAccessCardsBatchService = new CustomerTenantAccessCardsBatchService(Context, tenant, customerTenantAccessCardsBatch);
                                                        customerTenantAccessCardsBatchService.Update();
                                                        msg = "Update LastShipmentDate in Forwarder CustomerTenantAccess Completed Successfully , Total Succeeded = " + customerTenantAccessCardsBatch.Totalsucceeded + " " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, null, null, null, "");


                                                        queueservice.InitializeQueue("ImportersShipmentsDocsQueueBuilderQueue", 0);//"ImportersShipmentsDocsScheduleQueue", 0);
                                                        queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", Shipment.Id }, { "Tenant", tenant.ToString() }, { "CustomerId", customerTenantAccessCardsBatch.CustomerId }, { "BatchNumber", customerTenantAccessCardsBatch.BatchNumber } }, tenant, null, customerTenantAccessCardsBatch.CustomerId, customerTenantAccessCardsBatch.BatchNumber);
                                                        queueservice.Complete();
                                                        //if (response.RetryNumber == 0)
                                                        //{

                                                        //}
                                                    }
                                                    else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                                    {
                                                        APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
                                                        var Failmsg = EXC.ErrorType + " Faild To Send Updates To Importer Tenant " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                                        if (EXC != null)
                                                        {
                                                            throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                                        }
                                                        //queueservice.CompleteAsFailed();
                                                    }
                                                    //else 
                                                    //{
                                                    //    var temp = result.Content.ReadAsStringAsync().Result;
                                                    //    Exception EXC = JsonConvert.DeserializeObject<Exception>(result.Content.ReadAsStringAsync().Result); 
                                                    //    LogPM.Status = "F"; 
                                                    //    var Failmsg = EXC.Message;
                                                    //    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", DateTime.Now, DateTime.UtcNow, Failmsg, null, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), EXC.Message.Substring(0, 249));

                                                    //    throw EXC;

                                                    //}
                                                }

                                            }
                                            //    }
                                            //    queueservice.Complete();
                                            //}
                                            //queueservice.Complete();
                                        }
                                    }

                                    LogDoneItemInMemory();
                                }
                                catch (Exception ex)
                                {


                                    ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, "", "WorkerRole", "", null);
                                    if (response.MessageValues.Keys.Contains("ShipmentId"))
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
                                        var msg = ex.Message + DateTime.Now;

                                        //APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, LogPM.Status, DateTime.Now, DateTime.UtcNow, msg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249):errorMessage)); 
                                        //string ShipmentId = response.MessageValues["ShipmentId"].ToString();
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
                                                customerTenantAccessCardsBatch = customerTenantAccessCardBatchQuery.GetSinglePM(BatchNumber, tenant);
                                                customerTenantAccessCardsBatch.TotalFailed++;
                                                CustomerTenantAccessCardsBatchService customerTenantAccessCardsBatchService = new CustomerTenantAccessCardsBatchService(Context, tenant, customerTenantAccessCardsBatch);
                                                customerTenantAccessCardsBatchService.Update();
                                                if (IsNewLog)
                                                {
                                                    LogPM.Subject = "Send New Shipment To Importer By ImporterShipmentDocuments Controller";
                                                    LogPM.BatchNumber = BatchNumber;
                                                    LogPM.CustomerId = CustomerId;
                                                    LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                                    LogPM.QueueType = "Shipment";
                                                    LogPM.Tenant = tenant;
                                                    apiLogsService = new APILogsService(webFreightContext, tenant);
                                                    apiLogsService.Create(LogPM);
                                                }
                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

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
                                        customerTenantAccessCardsBatch = customerTenantAccessCardBatchQuery.GetSinglePM(BatchNumber, tenant);
                                        if (customerTenantAccessCardsBatch.TotalShipment == (customerTenantAccessCardsBatch.Totalsucceeded + customerTenantAccessCardsBatch.TotalFailed))
                                        {
                                            customerTenantAccessCardsBatch.Status = "Done";
                                            customerTenantAccessCardsBatch.DoneDate = DateTime.Now;
                                        }
                                        else if (customerTenantAccessCardsBatch.TotalFailed > 0)
                                        {
                                            customerTenantAccessCardsBatch.Status = "Failed";
                                        }
                                        else
                                        {
                                            customerTenantAccessCardsBatch.Status = "InProgress";
                                        }
                                        //if (customerTenantAccessCardsBatch.TotalShipment == (customerTenantAccessCardsBatch.Totalsucceeded + customerTenantAccessCardsBatch.TotalFailed))
                                        //{
                                        //    customerTenantAccessCardsBatch.DoneDate = DateTime.Now;
                                        //}

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
                queueservice.InitializeQueue("ImportersShipmentsBatchQueue", Tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }
        }




    }
}
