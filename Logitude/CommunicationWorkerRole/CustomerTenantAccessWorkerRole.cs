using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.Tools.EntityService;
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
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.WebServices;

namespace CommunicationWorkerRole
{
    public class CustomerTenantAccessWorkerRole : WorkerEntryPoint
    {
        IQueueService queue;
        string URI = "";//"http://localhost:9996/api/CustomerTenantAccessRequestApprovalController";//// controller

        public CustomerTenantAccessWorkerRole
            ()
        {
            IGlobalContext objectContext = GlobalContext.GetContext();
            SettingRepository SettingRepository = new SettingRepository(objectContext);
            SettingQuery SettingQuery = new SettingQuery(SettingRepository);
            URI = SettingQuery.GetSinglePM().CustomerTenantsURL.TrimEnd('/') + "/api/";
        }
        public override bool OnStart()
        {

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CustomerTenantAccess";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("CustomerTenantAccessQueue", 0);

            }

            catch (Exception ex)
            {

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "CustomerTenantAccess Role", null, ip);


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
                        queue.InitializeQueue("CustomerTenantAccessQueue", 0);
                        var response = queue.Receive();
                        LastActivity = DateTime.UtcNow;
                        int tenant = 0;
                        int CustomerTenant = 0;
                        bool IsCustomsActivated = false ;
                        bool IsExportActivated = false;


                        if (response != null && response.MessageId != null)
                        {
                            string Id = response.MessageValues["Id"].ToString();
                            int.TryParse(response.MessageValues["Tenant"], out tenant);
                            int.TryParse(response.MessageValues["CustomerTenant"], out CustomerTenant);
                            bool.TryParse(response.MessageValues["IsCustomsActivated"], out IsCustomsActivated);
                            bool.TryParse(response.MessageValues["IsExportActivated"], out IsExportActivated);
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
                                     
                                     

                                    CustomerTenantAccessRequestAM customerTenantAccessRequest = new CustomerTenantAccessRequestAM()
                                    {
                                        CustomerTenant = CustomerTenant,
                                        PartnerTenant = tenant,
                                        IsExportActivated = IsExportActivated,
                                        IsCustomsActivated = IsCustomsActivated
                                    };

                                    UpdateCustomerTenantAccessRequests(customerTenantAccessRequest);

                                    var serializedObject = JsonConvert.SerializeObject(customerTenantAccessRequest);
                                    LogPM.Subject = "Start To Send Response To Importer By CustomerTenantAccessRequestApproval Controller";
                                    if (IsNewLog)
                                    {
                                        apiLogsService.Create(LogPM);
                                    }
                                    var msg = "Start Sending Response To Importer Tenant " + DateTime.Now;
                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(customerTenantAccessRequest), null, null, "");
                                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                    var result = client.PutAsync(URI + "CustomerTenantAccessRequestApproval", content);
                                    result.Wait();
                                    if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                                        HybridPartnerQuery hybridPartnerQuery = new HybridPartnerQuery(tenant);
                                        CustomerTenantAccessPM temp = customerTenantAccessQuery.GetCustomerTenantAccessPMsByTenantCustomerTenant(tenant, CustomerTenant);
                                        var hybridPartner = hybridPartnerQuery.GetSinglePMByPartnerTenant(tenant);
                                        ContactRepository contactRepository = new ContactRepository(tenant);
                                        Contact User = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant, true);
                                        if (temp != null)
                                        {
                                            //if (IsPrivateLabelCustomer)
                                            //{

                                            //}
                                            TenantRepository TenantRep = new TenantRepository(tenant);
                                            LogBoxTenantSettingRepository LBTenantRep = new LogBoxTenantSettingRepository(tenant);
                                            var MainTenant = TenantRep.GetSingleTenant(tenant);
                                            var LBTenant = LBTenantRep.GetSingleLBTenant(tenant);
                                            Contact LogBoxUser = contactRepository.GetSingleContact(LBTenant.LogBoxAdminUserId, tenant);
                                            var domain = "@logbox.co.il";
                                            if (LogBoxUser != null && !string.IsNullOrEmpty(LogBoxUser.Email))
                                            {
                                                var EmailParts = LogBoxUser.Email.Split('@');
                                                if (EmailParts.Length == 2)
                                                {
                                                    domain = "@" + EmailParts[1];
                                                }
                                            }

                                            var NoReplyemail = "no-reply" + domain;
                                            var SalesEmail = "sales" + domain;
                                            string emailMessage = "";
                                            StringBuilder HtmlTemplate = new StringBuilder();
                                            HtmlTemplate.Append("<p style='text-align:left'>");
                                            HtmlTemplate.Append("<b>Hi " + temp.ContactName + "</b>");
                                            HtmlTemplate.Append("<br />");
                                            HtmlTemplate.Append("<b>" + (hybridPartner != null ? hybridPartner.Name : "") + " approved your request. You will start receiving your shipments and documents data into your account </b>");
                                            HtmlTemplate.Append("<br />");
                                            HtmlTemplate.Append("<b>Have a great day ! </b>");
                                            HtmlTemplate.Append("<br />");
                                            HtmlTemplate.Append("<b>" + MainTenant.Company + "</b>");
                                            emailMessage = "The agent < " + (hybridPartner != null ? hybridPartner.Name : "") + " > approve the request for < " + temp.CompanyName + " > - < " + temp.CustomerTenant + " >";//HtmlTemplate.ToString();
                                            var subject = "Approved request for < " + temp.CompanyName + " > - < " + temp.CustomerTenant + " >";//"Your request for data has been approved by " + (hybridPartner != null ? hybridPartner.Name : "");
                                            EmailCommunicationParams emailParams = new EmailCommunicationParams()
                                            {
                                                From = NoReplyemail,
                                                To = "sales@logbox.co.il",//temp.CompanyEmail,
                                                //BCC = SalesEmail,
                                                Subject = subject,
                                                EmailBody = emailMessage,
                                                LoggingUserId = User.Id,
                                                Tenant = tenant,
                                            };
                                            if (hybridPartner.Name.ToLower() != "dsv")
                                            {
                                                Communications.AddEmailCommunicationLogQueue(emailParams, tenant);
                                            }

                                        }
                                        queue.Complete();
                                        CustomerTenantAccessRepository repo = new CustomerTenantAccessRepository(tenant);
                                        var CTA = repo.GetSingleCustomerTenantAccess(Id, tenant);
                                        CTA.Status = "A";
                                        repo.Update(CTA);
                                        repo.SubmitChanges();
                                        LogPM.Status = "D";
                                        var ResponseData = result.Result.Content.ReadAsStringAsync().Result;
                                        var Donemsg = "Response Sent To Importer Successfully " + DateTime.Now;
                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, ResponseData, null, "");
                                    }
                                    else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                    {
                                        APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
                                        if (EXC != null)
                                        {
                                            var Failmsg = EXC.ErrorType + " Fail To Send Response To Importer Tenant " + DateTime.Now;
                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                            throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                        }
                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "WorkerRole", "", null);
                                string errorMessage = ex.Message + Environment.NewLine;

                                if (ex.InnerException != null)
                                {

                                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                                }

                                errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;


                                //APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", DateTime.Now, DateTime.UtcNow, "Faild To Send Response To Importer " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

                                if (response.MessageValues.Keys.Contains("Id"))
                                {
                                    string RequestId = response.MessageValues["Id"].ToString();
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
                            LogDoneItemInMemory();
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
                ConnectClient();
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "importer shipments worker role start", null, null);
                Thread.Sleep(10000);
            }
           
        }

        private void UpdateCustomerTenantAccessRequests(CustomerTenantAccessRequestAM customerTenantAccessRequest)
        {
            CustomerTenantAccessRequestQuery customerTenantAccessRequestQuery = new CustomerTenantAccessRequestQuery(customerTenantAccessRequest.PartnerTenant);
            HybridPartnerQuery hybridPartnerQuery = new HybridPartnerQuery(customerTenantAccessRequest.PartnerTenant); 

            List<string> customerTenantAccessRequestIds = customerTenantAccessRequestQuery.GetCustomerTenantAccessRequestForwarderIdsByTenant(customerTenantAccessRequest.CustomerTenant);
            List<string> forwarderIds = hybridPartnerQuery.GetPartnersForRequest(customerTenantAccessRequest.PartnerTenant, customerTenantAccessRequestIds);
            IQueryable<CustomerTenantAccessRequestPM> customerTenantAccessRequestList = customerTenantAccessRequestQuery.GetCustomerTenantAccessRequestByTenantAndForwarderIds(customerTenantAccessRequest.CustomerTenant, forwarderIds);
  
            UpdateCustomerTenantAccessRequestsList(customerTenantAccessRequest, customerTenantAccessRequestList);
             
        }

        private void UpdateCustomerTenantAccessRequestsList(CustomerTenantAccessRequestAM customerTenantAccessRequest, IQueryable<CustomerTenantAccessRequestPM> customerTenantAccessRequestList)
        {
            ICommonDataContext context = CommonDataContext.GetContext(customerTenantAccessRequest.PartnerTenant);
            CustomerTenantAccessRequestService customerTenantAccessRequestService = new CustomerTenantAccessRequestService(context, customerTenantAccessRequest.CustomerTenant);

            foreach (var request in customerTenantAccessRequestList)
            {
                request.IsCustoms = customerTenantAccessRequest.IsCustomsActivated;
                request.IsExport = customerTenantAccessRequest.IsExportActivated;
                customerTenantAccessRequestService.Update(request);
            }
        }
 
        private void ConnectClient()
        {
            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("CustomerTenantAccessQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }
        }


    }
}
