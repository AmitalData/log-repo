using Devart.Common;
using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityAMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;


namespace CommunicationWorkerRole
{
    public class ForwarderShipmentWorkerRole : WorkerEntryPoint
    {
        IQueueService queue;
        string URI = "";//"http://localhost:9996/api/CustomerTenantAccess";
        APILogsService apiLogsService;

        public ForwarderShipmentWorkerRole()
        {
            IGlobalContext objectContext = GlobalContext.GetContext();
            SettingRepository SettingRepository = new SettingRepository(objectContext);
            SettingQuery SettingQuery = new SettingQuery(SettingRepository);
            URI = SettingQuery.GetSinglePM().ForwarderTenantsURL.TrimEnd('/') + "/api/";
        }
        public override bool OnStart()
        {


            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ForwarderShipment";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("ForwarderShipmentQueue", 0);

            }

            catch (Exception ex)
            {

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "ForwarderShipmentQueue Role", null, ip);


            }
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
                    //PrimaryKey = "813d985d-f2ae-45b0-b49b-e52526ef9782",
                    //SecondaryKey = "1e5f5740-ed88-437b-916e-a7041470b2a2"
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
                        queue = new DbQueueService();
                        queue.InitializeQueue("ForwarderShipmentQueue", 0);
                        var response = queue.Receive();
                        LastActivity = DateTime.UtcNow;
                        int tenant = 0;


                        if (response != null && response.MessageId != null)
                        {

                            string ShipmentId = response.MessageValues["ShipmentId"].ToString();
                            int.TryParse(response.MessageValues["Tenant"], out tenant);
                            string CorrelationId = response.MessageId;
                            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);

                            #region APILogs
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
                                if (!string.IsNullOrEmpty(ShipmentId))
                                {

                                    apiLogsService = new APILogsService(webFreightContext, tenant);
                                    ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                                    ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);

                                    //var Shipment = shipmentQuery.GetSinglePM(ShipmentId, tenant);
                                    var ForwarderShipment = shipmentQuery.GetSinglePM(ShipmentId, tenant);
                                    if (ForwarderShipment != null)
                                    {
                                        var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);
                                        LogPM.ObjectTableId = Objecttable.Id;
                                        LogPM.EntityId = ForwarderShipment.Id;
                                        LogPM.Refrence = ForwarderShipment.ShipmentNumber;
                                        LogPM.Tenant = ForwarderShipment.Tenant;

                                        #region RegulerAddEdit


                                        using (var client = new HttpClient())
                                        {
                                            string ImporterShipmentsURI = URI + "ForwarderShipments";
                                            client.DefaultRequestHeaders.Add("Token", Token);
                                            client.DefaultRequestHeaders.Add("CorrelationId", CorrelationId);
                                            ICommonDataContext commoncontext = CommonDataContext.GetContext(ForwarderShipment.Tenant);
                                            CardRepository cardsReporistory = new CardRepository(commoncontext);
                                            BranchRepository branchRepository = new BranchRepository(commoncontext);
                                            DepartmentRepository departmentRepository = new DepartmentRepository(commoncontext);
                                            HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commoncontext);
                                            HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(hybridPartnerRepository);
                                            Card Shipper = cardsReporistory.GetSingleCard(ForwarderShipment.ShipperId, ForwarderShipment.Tenant);
                                            Card Consignee = cardsReporistory.GetSingleCard(ForwarderShipment.ConsigneeId, ForwarderShipment.Tenant);
                                            Card Customer = cardsReporistory.GetSingleCard(ForwarderShipment.CustomerId, ForwarderShipment.Tenant);
                                            Branch Branch = branchRepository.GetSingleBranch(ForwarderShipment.BranchId, ForwarderShipment.Tenant);
                                            Department Department = departmentRepository.GetSingleDepartment(ForwarderShipment.DepartmentId, ForwarderShipment.Tenant);
                                            HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePM(ForwarderShipment.ForwarderPartnerId);
                                            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
                                            EntityStatus status = EntityStatusRepository.GetSingleEntityStatus(ForwarderShipment.StatusId, ForwarderShipment.Tenant, true);

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

                                            // 
                                            if (ForwarderShipment.DirectionId.ToUpper() == "E")
                                            {
                                                NewAExporterShipmentAM newAExporterShipmentAM = new NewAExporterShipmentAM()
                                                {
                                                    Id = ForwarderShipment.Id,
                                                    ExporterTenant = tenant,
                                                    Tenant = (int)Partner.PartnerTenant,
                                                    TransportModeId = ForwarderShipment.TransportModeId,
                                                    DirectionId = ForwarderShipment.DirectionId,
                                                    CustomerShipmentNumber = ForwarderShipment.ShipmentNumber,
                                                    ShipmentTypeId = ForwarderShipment.ShipmentTypeId,
                                                    ConsigneeName = ForwarderShipment.ConsigneeName,
                                                    InvoiceReference = ForwarderShipment.PrivateLabelInvoiceNumber,
                                                    CustomerReference = ForwarderShipment.CustomerReference1,
                                                    IncludePickup = ForwarderShipment.PrivateLabelIncludePickup,
                                                    IncludeDelivery = ForwarderShipment.PrivateLabelIncludeDelivery,
                                                    DangerousGoods = ForwarderShipment.IsDangerous,
                                                    ReqFlightDate = ForwarderShipment.RequestedFlightDate,
                                                    Quantity = ForwarderShipment.PackagesQuantity,
                                                    Weight = ForwarderShipment.GrossWeight,
                                                    SendUpdatesToAgentEnabled = ForwarderShipment.SendUpdatesToAgentEnabled,
                                                    Customer = new CodeProperties()
                                                    {
                                                        Code = CustomerCode
                                                    },

                                                    Shipper = new CodeProperties()
                                                    {
                                                        Code = ShipperCode
                                                    },

                                                    FromPort = new CodeProperties()
                                                    {
                                                        Code = ForwarderShipment.FromPort,
                                                        CountryCode = ForwarderShipment.FromCountryCode
                                                    },
                                                    ToPort = new CodeProperties()
                                                    {
                                                        Code = ForwarderShipment.ToPort,
                                                        CountryCode = ForwarderShipment.ToCountryCode
                                                    },

                                                };

                                                newAExporterShipmentAM.ShipmentPackages = new List<Packages>();
                                                foreach (var item in ForwarderShipment.ShipmentPackages)
                                                {

                                                    Packages MyPackage = new Packages();
                                                    MyPackage.Quantity = item.Quantity;
                                                    //  MyPackage.GrossWeight = item.GrossWeight;
                                                    MyPackage.Length = item.Length;
                                                    MyPackage.Width = item.Width;
                                                    MyPackage.Height = item.Height;

                                                }


                                                LogPM.Subject = "Send Shipment To Forwarder By ForwarderShipments Controller";
                                                if (IsNewLog)
                                                {
                                                    //LogPM.CustomerId = CustomerId;
                                                    LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                                    LogPM.QueueType = "Shipment";
                                                    apiLogsService.Create(LogPM);
                                                }
                                                var msg = "Start Sending Shipment To Forwarder " + DateTime.Now;
                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(newAExporterShipmentAM), null, null, "");

                                                var serializedObject = JsonConvert.SerializeObject(newAExporterShipmentAM);
                                                var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                                ImporterShipmentsURI = URI + "ForwarderExportShipments";
                                                var result = client.PostAsync(ImporterShipmentsURI, content);
                                                result.Wait();
                                                if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                                {
                                                    var temp1 = result.Result.Content.ReadAsStringAsync().Result;
                                                    msg = "Shipment sent To Forwarder " + DateTime.Now;
                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(newAExporterShipmentAM), temp1, null, "");
                                                    queue.Complete();
                                                }
                                                else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                                {
                                                    APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
                                                    if (EXC != null)
                                                    {
                                                        var Failmsg = EXC.ErrorType + " Fail To Send Shipment To Forwarder Tenant " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                                        throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                                    }
                                                }

                                            }
                                            else
                                            {


                                                ShipmentAM shipmentAM = new ShipmentAM()
                                                {
                                                    Id = ForwarderShipment.Id,
                                                    ImporterTenant = tenant,
                                                    Tenant = (int)Partner.PartnerTenant,
                                                    MainCarriageATA = ForwarderShipment.MainCarriageATA,
                                                    MainCarriageETA = ForwarderShipment.MainCarriageETA,
                                                    MainCarriageATD = ForwarderShipment.MainCarriageATD,
                                                    OnCarriageATA = ForwarderShipment.OnCarriageATA,
                                                    OnCarriageATD = ForwarderShipment.OnCarriageATD,
                                                    PreCarriageATA = ForwarderShipment.PreCarriageATA,
                                                    PreCarriageATD = ForwarderShipment.PreCarriageATD,
                                                    StatusCode = status.Code,
                                                    StatusDate = ForwarderShipment.StatusDate,
                                                    TransportModeId = ForwarderShipment.TransportModeId,
                                                    DirectionId = ForwarderShipment.DirectionId,
                                                    ShipmentLevelCode = ForwarderShipment.ShipmentLevelCode,
                                                    ForwarderShipmentNumber = ForwarderShipment.ForwarderShipmentNumber,
                                                    CustomerShipmentNumber = ForwarderShipment.ShipmentNumber,
                                                    ShipmentTypeId = ForwarderShipment.ShipmentTypeId,
                                                    House = ForwarderShipment.House,
                                                    Master = ForwarderShipment.Master,
                                                    DescriptionOfGoods = ForwarderShipment.DescriptionOfGoods,
                                                    ConsigneeReference1 = ForwarderShipment.ConsigneeReference1,
                                                    CustomerReference1 = ForwarderShipment.CustomerReference1,
                                                    ConsigneeReference2 = ForwarderShipment.ConsigneeReference2,
                                                    CustomerReference2 = ForwarderShipment.CustomerReference2,
                                                    ShipmentCustomerTypeCode = ForwarderShipment.ShipmentCustomerTypeCode,
                                                    IsCancelled = ForwarderShipment.IsCancelled,
                                                    ShipperName = ForwarderShipment.ShipperName,
                                                    CarrierTransportDocumentNumber = ForwarderShipment.CarrierTransportDocumentNumber,
                                                    //ForwarderPartnerId = Partner.Id,
                                                    FreightPrepaidCollectId = ForwarderShipment.FreightPrepaidCollectId,
                                                    OtherPrepaidCollectId = ForwarderShipment.OtherPrepaidCollectId,
                                                    Notes = ForwarderShipment.Notes,
                                                    ShipmentAddtionalDataXML = ForwarderShipment.ShipmentAddtionalDataXML,
                                                    //IsOperationalClosed = Shipment.IsOperationalClosed,
                                                    ForwarderCode = Partner.PartnerTenant,
                                                    Quantity = ForwarderShipment.PackagesQuantity,
                                                    Weight = ForwarderShipment.GrossWeight,
                                                    SendUpdatesToAgentEnabled = ForwarderShipment.SendUpdatesToAgentEnabled,
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
                                                        Code = ForwarderShipment.FromPort,
                                                        CountryCode = ForwarderShipment.FromCountryCode
                                                    },
                                                    ToPort = new CodeProperties()
                                                    {
                                                        Code = ForwarderShipment.ToPort,
                                                        CountryCode = ForwarderShipment.ToCountryCode
                                                    },
                                                    PreCarriageFromPort = new CodeProperties()
                                                    {
                                                        Code = ForwarderShipment.PreCarriageFromPortCode,
                                                        CountryCode = ForwarderShipment.PreCarriageFromPortCountryCode
                                                    },
                                                    PreCarriageToPort = new CodeProperties()
                                                    {
                                                        Code = ForwarderShipment.PreCarriageToPortCode,
                                                        CountryCode = ForwarderShipment.PreCarriageToPortCountryCode
                                                    },
                                                    OnCarriageToPort = new CodeProperties()
                                                    {
                                                        Code = ForwarderShipment.OnCarriageToPortCode,
                                                        CountryCode = ForwarderShipment.OnCarriageToPortCountryCode
                                                    }

                                                };
                                                shipmentAM.ShipmentPackagesAM = new List<ShipmentPackageAM>();
                                                foreach (var item in ForwarderShipment.ShipmentPackages)
                                                {
                                                    ShipmentPackageAM MyPackage = new ShipmentPackageAM();
                                                    MyPackage.ContainerNumber = item.ContainerNumber;
                                                    MyPackage.Quantity = item.Quantity;
                                                    MyPackage.Weight = item.Weight;
                                                    MyPackage.PackageTypeId = item.PackageTypeId;
                                                    MyPackage.PackageTypeCode = item.PackageTypeCode;
                                                    shipmentAM.ShipmentPackagesAM.Add(MyPackage);
                                                }
                                            

                                            LogPM.Subject = "Send Shipment To Forwarder By ForwarderShipments Controller";
                                            if (IsNewLog)
                                            {
                                                //LogPM.CustomerId = CustomerId;
                                                LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                                LogPM.QueueType = "Shipment";
                                                apiLogsService.Create(LogPM);
                                            }
                                            var msg = "Start Sending Shipment To Forwarder " + DateTime.Now;
                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(shipmentAM), null, null, "");

                                            var serializedObject = JsonConvert.SerializeObject(shipmentAM);
                                            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                                            var result = client.PostAsync(ImporterShipmentsURI, content);
                                            result.Wait();
                                                if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                                {
                                                    var temp1 = result.Result.Content.ReadAsStringAsync().Result;
                                                    msg = "Shipment sent To Forwarder " + DateTime.Now;
                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(shipmentAM), temp1, null, "");
                                                    queue.Complete();
                                                }
                                                else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                                {
                                                    APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
                                                    if (EXC != null)
                                                    {
                                                        var Failmsg = EXC.ErrorType + " Fail To Send Shipment To Forwarder Tenant " + DateTime.Now;
                                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                                        throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                                    }
                                                }
                                            }



                                        }
                                        #endregion

                                    }
                                }

                                LogDoneItemInMemory();
                            }
                            catch (Exception ex)
                            {
                                #region HandleException
                                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "WorkerRole", "", null);
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
                                            queue.Delay(new TimeSpan(0, 0, 0, 5));
                                        }

                                        if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                                        {
                                            queue.Delay(new TimeSpan(0, 0, 0, 10));
                                        }
                                        if (response.RetryNumber >= 3)
                                        {
                                            queue.CompleteAsFailed();
                                            if (IsNewLog)
                                            {
                                                LogPM.Subject = "Send Shipment To Forwarder By ForwarderShipment Controller";
                                                //LogPM.CustomerId = CustomerId;
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
                                        queue.CompleteAsFailed();
                                    }
                                }
                                else
                                {
                                    queue.CompleteAsFailed();
                                }
                                #endregion
                            }
                        }
                        else
                        {
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
                string errorMessage = ex.Message + Environment.NewLine;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + ex.InnerException.Message + ")" + Environment.NewLine;

                }

                errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;

                ConnectClient();
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "forwarder shipments worker role start", null, null);
                Thread.Sleep(10000);
            }
        }

        private void ConnectClient()
        {
            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("ForwarderShipmentQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "forwarder shipment worker role start", null, null);
            }
        }
    }
}
