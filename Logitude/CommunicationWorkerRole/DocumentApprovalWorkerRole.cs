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
using Logitude.BL.CommonDataModel.Tools.EntityService;


namespace CommunicationWorkerRole
{
    public class DocumentApprovalWorkerRole : WorkerEntryPoint
    {
        IQueueService queue;
        string URI = "";//"http://localhost:9996/api/CustomerTenantAccess";
        APILogsService apiLogsService;

        public DocumentApprovalWorkerRole()
        {
            IGlobalContext objectContext = GlobalContext.GetContext();
            SettingRepository SettingRepository = new SettingRepository(objectContext);
            SettingQuery SettingQuery = new SettingQuery(SettingRepository);
            URI = SettingQuery.GetSinglePM().ForwarderTenantsURL.TrimEnd('/') + "/api/";
        }
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DocumentApproval";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("DocumentApprovalQueue", 0);

            }

            catch (Exception ex)
            {

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DocumentApprovalQueue Role", null, ip);


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
                        queue.InitializeQueue("DocumentApprovalQueue", 0);
                        var response = queue.Receive();
                        LastActivity = DateTime.UtcNow;
                        int tenant = 0;


                        if (response != null && response.MessageId != null)
                        {

                            string Id = response.MessageValues["Id"].ToString();
                            int.TryParse(response.MessageValues["Tenant"], out tenant);
                            string CorrelationId = response.MessageId;
                            string ApprovedByUserName = response.MessageValues["ApprovedByUserName"].ToString();
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
                                if (!string.IsNullOrEmpty(Id))
                                {
                                    ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                                    var ForwarderShipment = shipmentQuery.GetSinglePMWithoutComposition(Id, tenant);
                                    apiLogsService = new APILogsService(webFreightContext, tenant);
                                    ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                                    ShipmentAdditionalCloudDataRepository Repo = new ShipmentAdditionalCloudDataRepository(tenant);
                                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                                    HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commoncontext);
                                    HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(hybridPartnerRepository); 
                                    HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePM(ForwarderShipment.ForwarderPartnerId);
                                    var Data = Repo.GetSingleShipmentAdditionalCloudData(Id, tenant);
                                    if (Data != null)
                                    {
                                        var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);
                                        LogPM.ObjectTableId = Objecttable.Id;
                                        LogPM.EntityId = Data.Id;
                                        LogPM.Tenant = Data.Tenant;

                                        #region RegulerAddEdit

                                        using (var client = new HttpClient())
                                        {
                                            string ImporterShipmentsURI = URI + "DocumentApproval";
                                            client.DefaultRequestHeaders.Add("Token", Token);
                                            client.DefaultRequestHeaders.Add("CorrelationId", CorrelationId);
                                           
                                            
                                            LogPM.Subject = "Send Document Approval To Forwarder By DocumentApproval Controller";
                                            if (IsNewLog)
                                            {
                                                LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                                                LogPM.QueueType = "DocumentApproval";
                                                LogPM.Refrence = ForwarderShipment.ForwarderShipmentNumber;
                                                apiLogsService.Create(LogPM);
                                            }
                                            var msg = "Start Sending Document Approval To Forwarder " + DateTime.Now;
                                            ShipmentAdditionalCloudDataAM DataAM = new ShipmentAdditionalCloudDataAM()
                                            {
                                                ShipmentNumber = ForwarderShipment.ForwarderShipmentNumber,
                                                Tenant = Partner == null ? 0 : (int)Partner.PartnerTenant,
                                                Code = "VAD",
                                                //Date = Data.ApproveDateTime != null ? Data.ApproveDateTime.Value.ToShortDateString() : "",
                                                //Time = Data.ApproveDateTime != null ? Data.ApproveDateTime.Value.ToShortTimeString() : "",
                                                Remarks = ApprovedByUserName,
                                                Direction = ForwarderShipment.DirectionId
                                            };
                                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(DataAM), null, null, "");
                                            
                                            var serializedObject = JsonConvert.SerializeObject(DataAM);
                                            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                                            var result = client.PostAsync(ImporterShipmentsURI, content);
                                            result.Wait();
                                            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                            {
                                                msg = "Document Approval sent To Forwarder " + DateTime.Now;
                                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(DataAM), null, null, "");
                                                queue.Complete();
                                            }
                                            else
                                            {
                                                APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
                                                if (EXC != null)
                                                {
                                                    var Failmsg = EXC.ErrorType + " Fail To Send Document Approval To Forwarder Tenant " + DateTime.Now;
                                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                                    throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
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
                                if (response.MessageValues.Keys.Contains("Id"))
                                {
                                    string errorMessage = ex.Message + Environment.NewLine;

                                    if (ex.InnerException != null)
                                    {
                                        errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
                                    }

                                    errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                    var msg = ex.Message + DateTime.Now;
                                    if (!string.IsNullOrEmpty(Id))
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
                                                LogPM.Subject = "Send Document Approval To Forwarder By DocumentApproval Controller";
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
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DocumentApproval worker role start", null, null);
                Thread.Sleep(10000);
            }
        }

        private void ConnectClient()
        {
            try
            {
                queue = new DbQueueService();
                queue.InitializeQueue("DocumentApprovalQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DocumentApproval worker role start", null, null);
            }
        }
    }
}
