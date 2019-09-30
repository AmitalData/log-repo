using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
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
    public class DeclarationApprovalRequestWorkerRole : WorkerEntryPoint
    {
        IQueueService queue;
        string URI = "";

        public DeclarationApprovalRequestWorkerRole()
        {
            IGlobalContext objectContext = GlobalContext.GetContext();
            SettingRepository SettingRepository = new SettingRepository(objectContext);
            SettingQuery SettingQuery = new SettingQuery(SettingRepository);
            URI = SettingQuery.GetSinglePM().CustomerTenantsURL.TrimEnd('/') + "/api/";
        }
        public override bool OnStart()
        {

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DeclarationApprovalRequestWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("DeclarationApprovalRequestQueue", 0);

            }

            catch (Exception ex)
            {

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DeclarationApprovalRequestWorkerRole", null, ip);


            }
            return base.OnStart();
        }

        string Token;
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
                        queue = new DbQueueService();
                        queue.InitializeQueue("DeclarationApprovalRequestQueue", 0);
                        var response = queue.Receive();
                        LastActivity = DateTime.UtcNow;
                        int tenant = 0;
                        int ImporterTenant = 0;


                        if (response != null && response.MessageId != null)
                        {
                            string Id = response.MessageValues["Id"].ToString();
                            int.TryParse(response.MessageValues["Tenant"], out tenant);
                            int.TryParse(response.MessageValues["ImporterTenant"], out ImporterTenant); 
                            string CorrelationId = response.MessageValues["CorrelationId"].ToString();
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
                                    ShipmentQuery ShipmentQuery = new ShipmentQuery(tenant);
                                    ShipmentPM ShipmentPm = ShipmentQuery.GetSinglePM(Id,tenant);
                                    DeclarationApprovalRequestPM ApprovalRequestPM = new DeclarationApprovalRequestPM()
                                    {
                                        Tenant = ImporterTenant,
                                        ShipmentNumber = ShipmentPm.ShipmentNumber,
                                        DeclarationXmlData = ShipmentPm.DeclarationXMLData
                                    };
                                    var serializedObject = JsonConvert.SerializeObject(ApprovalRequestPM);
                                    LogPM.Subject = "Send Declaration Approval Request To Importer By DeclarationApprovalRequest Controller";
                                    if (IsNewLog)
                                    {
                                        apiLogsService.Create(LogPM);
                                    }
                                    var msg = "Start Sending Declaration Approval Request To Importer Tenant " + DateTime.Now;
                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(ApprovalRequestPM), null, null, "");
                                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                    var result = await client.PutAsync(URI + "DeclarationApprovalRequest", content);
                                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                    { 
                                        // VDK Logic
                                        var ResponseData = result.Content.ReadAsStringAsync().Result;
                                        var Donemsg = "Declaration Approval Request Sent To Importer Successfully " + DateTime.Now;
                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, ResponseData, null, "");
                                    }
                                    else //if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                                    {
                                        APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Content.ReadAsStringAsync().Result);
                                        if (EXC != null)
                                        {
                                            var Failmsg = EXC.ErrorType + " Fail To Send Declaration Approval Request To Importer Tenant " + DateTime.Now;
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
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Declaration Approval Request worker role start", null, null);
                Thread.Sleep(10000);
            }
           
        }
        private void ConnectClient()
        {
            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("DeclarationApprovalRequestWorkerRole", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Declaration Approval Request worker role start", null, null);
            }
        }


    }
}
