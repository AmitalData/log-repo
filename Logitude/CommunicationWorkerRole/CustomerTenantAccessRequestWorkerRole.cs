using Devart.Common;
using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
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
    public class CustomerTenantAccessRequestWorkerRole : WorkerEntryPoint
    {
        IQueueService queue;
        string URI = "";//"http://localhost:9996/api/CustomerTenantAccess";

        public CustomerTenantAccessRequestWorkerRole()
        {
            IGlobalContext objectContext = GlobalContext.GetContext();
            SettingRepository SettingRepository = new SettingRepository(objectContext);
            SettingQuery SettingQuery = new SettingQuery(SettingRepository);
            URI = SettingQuery.GetSinglePM().ForwarderTenantsURL.TrimEnd('/') + "/api/";
        }
        public override bool OnStart()
        {


            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CustomerTenantAccessRequest";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("CustomerTenantAccessRequestQueue", 0);

            }

            catch (Exception ex)
            {

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "CustomerTenantAccessRequest Role", null, ip);


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
                        queue.InitializeQueue("CustomerTenantAccessRequestQueue", 0);
                        var response = queue.Receive();
                        LastActivity = DateTime.UtcNow;
                        int tenant = 0;


                        if (response != null && response.MessageId != null)
                        {
                            
                            string RequestId = response.MessageValues["RequestId"].ToString();
                            int.TryParse(response.MessageValues["Tenant"], out tenant);
                            string CorrelationId = response.MessageId;
                            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
                            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                            APILogsService apiLogsService = new APILogsService(webFreightContext, tenant);
                            bool IsNewLog = false;
                            var aPILogsRepository = new APILogsRepository(webFreightContext);
                            APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, tenant);
                            APILogsPM LogPM;
                            if (Log == null)
                            {
                                IsNewLog = true;
                                var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);
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
                                    ObjectTableId = Objecttable.Id,
                                    ExpirationDate = DateTime.Now.AddDays(90),
                                    Status = "I",
                                    Tenant = tenant
                                    //
                                };
                            }
                            else
                            {
                                IsNewLog = false;
                                var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);
                                LogPM = new APILogsPM()
                                {
                                    Id = Log.Id,
                                    CorrelationId = Log.CorrelationId,
                                    CreateDate = Log.CreateDate,
                                    CreateDateUTC = Log.CreateDateUTC,
                                    Direction = Log.Direction,
                                    LastUpdateDate = Log.LastUpdateDate,
                                    LastUpdateDateUTC = Log.LastUpdateDateUTC,
                                    NumberOfRetries = Log.NumberOfRetries++,
                                    ObjectTableId = Log.ObjectTableId,
                                    ExpirationDate = Log.ExpirationDate,
                                    Status = "I",
                                    Tenant = Log.Tenant,

                                };
                            }
                            try
                            {
                                using (var client = new HttpClient())
                                {
                                    
                                    client.DefaultRequestHeaders.Add("Token", Token);
                                    client.DefaultRequestHeaders.Add("CorrelationId", CorrelationId);
                                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                                    CustomerTenantAccessRequestQuery customerTenantAccessRequestQuery = new CustomerTenantAccessRequestQuery(tenant);
                                    var customerTenantAccessRequest = customerTenantAccessRequestQuery.GetSinglePM(RequestId, tenant);
                                    TenantQuery tenantQuery = new TenantQuery(tenant);
                                    var Tenant = tenantQuery.GetSinglePMByCustomerId(tenant);
                                    HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commoncontext);
                                    HybridPartner Partner = hybridPartnerRepository.GetSingleHybridPartner(customerTenantAccessRequest.ForwarderId);
                                    CustomerQuery customerQuery = new CustomerQuery(tenant);
                                    var customer = customerQuery.GetSinglePMForLogBox(Tenant.CustomerId, tenant);
                                    
                                    CustomerTenantAccessAM customerTenantAccessAM = new CustomerTenantAccessAM()
                                    {
                                        Tenant = Partner.PartnerTenant,
                                        CustomerTenant = customerTenantAccessRequest.Tenant,
                                        CompanyName = Tenant.Company,
                                        CompanyEmail = Tenant.Email,
                                        CompanyVat = !string.IsNullOrEmpty(Tenant.VatNumber) ? Tenant.VatNumber.Trim() : Tenant.VatNumber,
                                        ContactName = Tenant.CustomerName,
                                        ContactMobile = Tenant.CustomerMobile,
                                        ContactPhone = Tenant.CustomerPhone,
                                        IsPrivateLabelCustomer = false,
                                        StockTypeCode = Tenant.StockTypeCode,///
                                    };
                                    GlobalTenant Globaltenant = null;
                                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                                    {
                                        GlobalTenantRepository globaltenantRep = new GlobalTenantRepository();
                                        Globaltenant = globaltenantRep.GetGlobalTenantsByTenant(customerTenantAccessAM.CustomerTenant);
                                        if (Globaltenant != null && !string.IsNullOrEmpty(Globaltenant.PrivateLabelId))
                                        {
                                            customerTenantAccessAM.IsPrivateLabelCustomer = true;
                                        }
                                        scope.Complete();
                                    }
                                    var serializedObject = JsonConvert.SerializeObject(customerTenantAccessAM);
                                    LogPM.Subject = "Start To Send Request To Forwarder By CustomerTenantAccess Controller";
                                    
                                    if (IsNewLog)
                                    {
                                        apiLogsService.Create(LogPM);
                                    }
                                   
                                    var msg = "Start Sending Request To Forwarder Tenant " + DateTime.Now;
                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, LogPM.Status, response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(customerTenantAccessAM), null, null, "");
                                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                    var result = client.PostAsync(URI + "CustomerTenantAccess", content);
                                    result.Wait();
                                    if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        queue.Complete();
                                        LogPM.Status = "D";
                                        var ResponseData = result.Result.Content.ReadAsStringAsync().Result;
                                        var Donemsg = "Request Sent To Forwarder Successfully " + DateTime.Now;
                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, ResponseData, null, "");
                                    }
                                    else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                    {
                                        APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
                                        if (EXC != null)
                                        {
                                            var Failmsg = EXC.ErrorType + " Fail To Send Request To Forwarder Tenant " + DateTime.Now;
                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                            throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                        }
                                    }
                                }
                                LogDoneItemInMemory();

                            }
                            catch (Exception ex)
                            {
                                
                                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "WorkerRole", "", null);
                                string errorMessage = ex.Message + Environment.NewLine;

                                if (ex.InnerException != null)
                                {

                                    errorMessage = errorMessage + " (" + ex.InnerException.Message + ")" + Environment.NewLine;

                                }

                                errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                               
                                //APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", DateTime.Now, DateTime.UtcNow, "Faild To Send Request To Forwarder " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
                                if (response.MessageValues.Keys.Contains("RequestId"))
                                {
                                    if (!string.IsNullOrEmpty(RequestId))
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
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "importer shipments worker role start", null, null);
                Thread.Sleep(10000);
            } 
        }

        private void ConnectClient()
        {
            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("CustomerTenantAccessRequestQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }
        }
    }
}
