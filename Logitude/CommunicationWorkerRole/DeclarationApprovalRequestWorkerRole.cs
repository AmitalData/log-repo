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
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
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
                                    ShipmentPM ShipmentPm = ShipmentQuery.GetSinglePM(Id, tenant);
                                    DeclarationApprovalRequestPM ApprovalRequestPM = new DeclarationApprovalRequestPM()
                                    {
                                        Tenant = ImporterTenant,
                                        ForwarderShipmentNumber = ShipmentPm.ShipmentNumber,
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
                                        AddVDKExternalTaskQueue(ApprovalRequestPM, tenant);
                                        //var ResponseData = result.Content.ReadAsStringAsync().Result;
                                        var Donemsg = "Declaration Approval Request Sent To Importer Successfully, Start Sending VDK to Unif. " + DateTime.Now;
                                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, "VDK", null, "");
                                        queue.Complete();
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

        private void AddVDKExternalTaskQueue(DeclarationApprovalRequestPM approvalRequestPM, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentrepository = new DocumentRepository(commonContext);
            ObjectTableRepository objecttableRep = new ObjectTableRepository(tenant);
            ObjectTable objectTable = null;

            objectTable = objecttableRep.GetObjectTableByName("Shipment", 0, true);

            List<QueueTask> tasks = new List<QueueTask>();
            tasks.Add(new QueueTask()
            {
                Action = "StatusUpdate",
                Parameters = new List<Logitude.Server.Tools.Parameter>() {
                new Logitude.Server.Tools.Parameter { Name = "ShipmentNumber", Value = approvalRequestPM.ForwarderShipmentNumber},
                new Logitude.Server.Tools.Parameter { Name = "Code", Value = "VDK"},
                new Logitude.Server.Tools.Parameter { Name = "Date", Value = TenantServerConfigration.GetCurrentDateTime(tenant).ToShortDateString()},
                new Logitude.Server.Tools.Parameter { Name = "Time", Value = TenantServerConfigration.GetCurrentDateTime(tenant).ToShortTimeString()},
                new Logitude.Server.Tools.Parameter { Name = "Remarks", Value = "Approval Task Received"}
                }
            });
            var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = ByteData.Length,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "ExternalTasksQueue",
            };
            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            var commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                InOut = "O",
                //EntityId = OceanInsightsRequest.Id,
                ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                Subject = "Approval Task Received",
                Tenant = tenant,
                CommunicationLogTypeCode = "Q",
                CommunicationStatusTypeCode = "W",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,
                QueueName = "externaltasksqueue" + tenant + 1,
                Priority = 1,
                EntityReference = approvalRequestPM.ForwarderShipmentNumber

            };

            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            string filename = document.Id + "." + document.Extension;
            string filePath = "tenant" + commLog.Tenant + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = ByteData.Length,

            };

            storageservice.Write(ByteData, fileInfo);

            if (!string.IsNullOrEmpty(commLog.QueueName))
            {
                try
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "Before adding message to queue ImporterApprovalReceived " + DateTime.Now.ToString(), null);
                    SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, tenant);
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "after adding message to queue  ImporterApprovalReceived " + DateTime.Now.ToString(), null);

                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;

                    if (!string.IsNullOrEmpty(ex.StackTrace))
                    {
                        errorMessage += Environment.NewLine + ex.StackTrace;
                    }

                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "Exception occured while adding message to queue ImporterApprovalReceived " + DateTime.Now.ToString(), errorMessage);
                }
            }
        }

        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } });

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "SendCommunicationLogMessageToQueue Approval Request", null, null);
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
