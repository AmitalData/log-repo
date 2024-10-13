
using CustomsWorkerRole.L2U;

using Logitude.Server.Tools.Models;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
//using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnifreightIIG.UServer;
using CustomsWorkerRole.Queue;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System.Net.Http;
using Logitude.Customs.BL.Messaging.Maman;
using Microsoft.Practices.Unity;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.Messaging;
using Logitude.Customs.BL.Messaging.ILOVS;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using System.Web;
using Logitude.Customs.BL.Messaging.Customs.PerformanceLogger;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;

namespace CustomsWorkerRole
{
    /* 
     * ///couriernet_global _global _global
Insert into BATCHSERVICESDEFINITIONS (CODE,CLASSNAME) values ('SendWEBAPIMessage2MamanWR','SendWEBAPIMessage2MamanWR');
Insert into BATCHSERVICESDEFINITIONMODS (CODE,INACTIVE,NUMBEROFTHREADS) values ('SendWEBAPIMessage2MamanWR',0,1);
     */

    //public class SendWebAPI2MamanGWMessageECTHRDataWR
    public class SendWEBAPIMessage2MamanWR
        : CustomsWorkerEntryPoint
    {
        const int MaxRetries = 16;
        
        QueueDescription _QueueDescription;
        QueueClient _QueueClient;
        public override void Run()
        {

            while (true)
            {

                if (!General.IsUpdating())
                {
                    try
                    {

                        WorkOnce();
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "SendDataToAmitalWR : Run() Method", null);
                        Thread.Sleep(10000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }


            }

        }
        bool _OnStartDone = false;
        private DbQueueService _IQueueService;
        private ICommonDataContext _ICommonDataContext;
        private ICommonDataContext _Context;
        private CommunicationLogRepository _CommunicationLogRep;
        private int _Tenant;
        private string _CommunicationLogId;
        private CommunicationLog _WaitingCommLog;
        private QueueResponse _ReceivedBrokeredMessage;
        private string myClass;
        public SendWEBAPIMessage2MamanWR()
        {

        }
        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;
                DoneItemsInRange = new Dictionary<DateTime, int>();


                this.myClass = this.GetType().Name;

                var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(1);
                var customsEnvironmentSettingPM = customsEnvironmentSettingQueryService.GetEnvironmentSettingPM() ?? new CustomsEnvironmentSettingPM();
                
                if (CustomDbQueueService.SupportedRabbitMQList.Contains(SBQueueNames.SendWEBAPIMessage2MamanQ.ToString()) && CustomDbQueueService.IsFeatureOnRABBITMQ_Communication() && customsEnvironmentSettingPM.UseRabbitMQ)
                {
                    base.WorkerQueueType = WorkerQueueType.RabbitMQ;
                }
                else
                {
                    base.WorkerQueueType = WorkerQueueType.DB;
                }




            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "amital send data worker role start", null, null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.


            return base.OnStart();
        }


        public override void WorkOnce()
        {

            try
            {
                OnStart();

                

                switch (base.WorkerQueueType)
                {

                    case WorkerQueueType.RabbitMQ:
                        WorkUntilPrcossesStop_RabbitMQ();
                        break;
                    case WorkerQueueType.DB:
                    default:
                        {
                            WorkUntilQEmpty_Db();
                        }
                        break;
                }
                


            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }


        }

        private void WorkUntilPrcossesStop_RabbitMQ()
        {
            var rabbitMQConsumerService = new RabbitMQConsumerService(myClass, SBQueueNames.SendWEBAPIMessage2MamanQ.ToString());
            rabbitMQConsumerService.WorkUntilPrcossesStop_RabbitMQ(
                (CustomDBQueueMessage customDBQueueMessage) =>
                {
                    _ReceivedBrokeredMessage= customDBQueueMessage.MyQueueResponse;

                     _CommunicationLogId = customDBQueueMessage.Properties["CommunicationLogId"].ToString();
                    int.TryParse(customDBQueueMessage.Properties["Tenant"].ToString(), out _Tenant);

                    LogMessagingUtil.Instance
                        .AppendLine("SendWEBAPIMessage2MamanWR:ProccessReceivedMessage()")
                        .AppendLine("QUEUEMessageId:" + customDBQueueMessage.MessageId)
                        .AppendLine("RetryNumber:" + customDBQueueMessage.Retries)
                        .AppendLine("CommunicationLogId:" + _CommunicationLogId)
                        .AppendLine(",Tenant" + _Tenant);

                    //ProccessReceivedMessage();

                    _Context = CommonDataContext.GetContext(_Tenant);
                    _CommunicationLogRep = new CommunicationLogRepository(_Context);


                    _WaitingCommLog = _CommunicationLogRep.GetSingleCommunicationLog(_CommunicationLogId, _Tenant);
                    if (_WaitingCommLog == null)
                    {
                        var myEx = new Exception("GetSingleCommunicationLog(_CommunicationLogId:" + _CommunicationLogId + " , _Tenant:" + _Tenant.ToString() + ") == null");
                        ExceptionHandler.HandleException(myEx, DateTime.Now, _Tenant, "", "WorkerRole", "", null);
                        return false;
                    }


                    PostWebAPIAnalyzeAndSaveCommDone(_CommunicationLogRep, _WaitingCommLog);

                    return true;
                },
                base.LogDoneItemInMemory
                );
        }

        private void WorkUntilQEmpty_Db()
        {
            while (!WorkerRoleServiceLocator.PleaseShutDown)
            {
                
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        _IQueueService = new DbQueueService();
                        _IQueueService.InitializeQueue(SBQueueNames.SendWEBAPIMessage2MamanQ.ToString(), 0);

                        using (TransactionScope scopeRecive = TransactionFactory.GetNewReadCommittedTransaction())
                        {
                            _ReceivedBrokeredMessage = _IQueueService.Receive(nextRunDelayInSec: CustomsWorkerRole.Utils.GenUtil.GetQueueTimeOutInMin() * 60);
                            scopeRecive.Complete();
                        }

                        if (_ReceivedBrokeredMessage == null || String.IsNullOrWhiteSpace(_ReceivedBrokeredMessage.MessageId))
                        {
                            QueueThreadStateService.Upsert(QueueThreadStateService.GetWRKey(this.GetType().Name), "Sleep...");
                            //Thread.Sleep(TimeSpan.FromSeconds(5));
                            //Thread.Sleep(TimeSpan.FromSeconds(15));//not using soo mach 
                            Thread.Sleep(TimeSpan.FromSeconds(CustomsWorkerRole.Utils.GenUtil.IfNoQueue_ServerWaitTimeInSec()));
                            break;
                        }
                        if (_ReceivedBrokeredMessage.RetryNumber > MaxRetries)
                        {
                            if (true)
                            {
                                _IQueueService.Complete();
                            }
                            else
                            {
                                _IQueueService.CompleteAsFailed();
                            }                     
                        }
                        InitParams();

                        ProccessReceivedMessage();
                        scope.Complete();
                    }
                }
                finally
                {
                    PerformanceM.SleepMSAfterEachQueuePeek();
                }
            }
        }



        
        public void ProccessReceivedMessage()
        {
            //InitParams();

            LogMessagingUtil.Instance
                .AppendLine("ProccessReceivedMessage()")
                .AppendLine("QUEUEMessageId:" + _ReceivedBrokeredMessage.MessageId)
                .AppendLine("RetryNumber:" + _ReceivedBrokeredMessage.RetryNumber)
                .AppendLine("CommunicationLogId:" + _CommunicationLogId)
                .AppendLine(",Tenant" + _Tenant);
            QueueThreadStateService.Upsert(QueueThreadStateService.GetWRKey(this.GetType().Name), $"CommLog:{_CommunicationLogId},QId:{_ReceivedBrokeredMessage.MessageId}");

            _Context = CommonDataContext.GetContext(_Tenant);
            _CommunicationLogRep = new CommunicationLogRepository(_Context);


            _WaitingCommLog = _CommunicationLogRep.GetSingleCommunicationLog(_CommunicationLogId, _Tenant);
            if (_WaitingCommLog == null)
            {

                var myEx = new Exception("GetSingleCommunicationLog(_CommunicationLogId:" + _CommunicationLogId + " , _Tenant:" + _Tenant.ToString() + ") == null");
                ExceptionHandler.HandleException(myEx, DateTime.Now, _Tenant, "", "WorkerRole", "", null);
                _IQueueService.Complete(); //Stop Try !!
                return;
            }



            try
            {
                SentWAPIComm();

            }
            catch (Exception exc)
            {

                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(exc);
                //ExceptionHandler.HandleException(exc, DateTime.Now, _Tenant, "", "WorkerRole", "", null);
                _WaitingCommLog.Retries++;
                if (_WaitingCommLog.Retries > MaxRetries)
                {
                    _WaitingCommLog.CommunicationStatusTypeCode = "F";
                }
                _WaitingCommLog.LastStatusDate = DateTime.Now;
                _WaitingCommLog.LastStatusDateUTC = DateTime.UtcNow;

                var s = $" Retries:{_WaitingCommLog.Retries}  ProccessReceivedMessage()Exception:" + exc.Message;
                s+= exc.StackTrace.ToString();
                _WaitingCommLog.ExceptionMessage = s.Substring(0, Math.Min(7999, s.Length));
                _CommunicationLogRep.Update(_WaitingCommLog);
                _CommunicationLogRep.SubmitChanges();
                //_IQueueService.Delay(TimeSpan.FromMinutes(1));
                //throw;

            }
        }

        private void InitParams()
        {
            LogMessagingUtil.Instance.Clear();

            _CommunicationLogId = _ReceivedBrokeredMessage.MessageValues["CommunicationLogId"].ToString();
            int.TryParse(_ReceivedBrokeredMessage.MessageValues["Tenant"].ToString(), out _Tenant);

            LogMessagingUtil.Instance
                .AppendLine("ProccessReceivedMessage()")
                .AppendLine("QUEUEMessageId:" + _ReceivedBrokeredMessage.MessageId)
                .AppendLine("RetryNumber:" + _ReceivedBrokeredMessage.RetryNumber)
                .AppendLine("CommunicationLogId:" + _CommunicationLogId)
                .AppendLine(",Tenant" + _Tenant);
        }
        
        private void SentWAPIComm(bool forceRetryFromTester=false)
        {
            if (forceRetryFromTester ||_ReceivedBrokeredMessage.RetryNumber < MaxRetries)
            {
                LastActivity = DateTime.UtcNow;
                LogMessagingUtil.Instance.Append("DoAction(PostWebAPI)..");

                PostWebAPIAnalyzeAndSaveCommDone(_CommunicationLogRep, _WaitingCommLog);//if failed throw exception
                if (!forceRetryFromTester)
                {
                    _IQueueService.Complete();
                }
                
                this.LogDoneItemInMemory();
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine($"RetryNumber >= {MaxRetries}>>> Failed ");
                _WaitingCommLog.CommunicationStatusTypeCode = "F";
                _IQueueService.Complete();
            }
            LogMessagingUtil.Instance.AppendLine(":" + _WaitingCommLog.CommunicationStatusTypeCode);
            _CommunicationLogRep.Update(_WaitingCommLog);
            _CommunicationLogRep.SubmitChanges();
        }


        private static bool PostWebAPIAnalyzeAndSaveCommDone(CommunicationLogRepository _CommunicationLogRep,CommunicationLog _WaitingCommLog)
        {
            try
            {


                //LogMessagingUtil.Instance.Clear();
                int tenant = _WaitingCommLog.Tenant;
                ICommonDataContext commoncontext = CommonDataContext.GetContext(_WaitingCommLog.Tenant);
                DocumentRepository documentRepository = new DocumentRepository(commoncontext);

                Document document = documentRepository.GetSingleDocument(_WaitingCommLog.Tenant, _WaitingCommLog.DocumentId);
                Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = document.FileSize,
                };

                Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                byte[] filedata = storageservice.Read(fileInfo);

                if (filedata == null)
                {
                    throw new Exception("The file data was not found!");
                }
                if (string.IsNullOrEmpty(_WaitingCommLog.LogSettings))
                {
                    throw new Exception("string.IsNullOrEmpty(waitingCommLog.LogSettings)");
                }
                var dataJson = System.Text.Encoding.UTF8.GetString(filedata.ToArray());
                var courier2MamanCommSettings = JsonConvert.DeserializeObject<CourierWEBAPICommSettings>(_WaitingCommLog.LogSettings);
                if (courier2MamanCommSettings == null)
                {
                    throw new Exception("(courierHawbMamanCommunicationLogSettings == null)");
                }
                LogMessagingUtil.Instance.AppendLine("courierHawbMamanCommunication DB is valid");
                GWMessageECTHRData responeGWMessageECTHRData = null;
                LogMessagingUtil.Instance.AppendLine($"Post {courier2MamanCommSettings.URIMethod}");
                string webAPIResultString = null;
                var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
                var @intrface = customsPartnerFtpDetails.GetAllInterfaceDetails().First(r => r.Code == courier2MamanCommSettings.MessageCode);

                switch (@intrface.WEBAPICredentialType)
                {
                 
                    case CourierWEBAPICredentialType.Bearer:
                        {
                            var service = new WebAPI2BearerMamanMessage(courier2MamanCommSettings);
                            webAPIResultString = service.PostIt(dataJson);
                        }
                        break;
                    case CourierWEBAPICredentialType.NetworkCredential:
                        {
                            var service = new WebAPINetworkCredentialMessage(courier2MamanCommSettings);
                            webAPIResultString = service.PostIt(dataJson);
                        }
                        break;
                    default:
                        throw new Exception("@PostWebAPIAnalyzeAndSaveCommDone():intrface.WEBAPICredentialType IS UNKNOWN");
                        break;
                }
                
                LogMessagingUtil.Instance.AppendLine("webAPIResultString:" + webAPIResultString);


                //                var myWebAPICourierHawbMamanService = new WebAPICourierGWMessageECTHRDataMamanService();
                //myWebAPICourierHawbMamanService.AnalyzeResponse(courier2MamanCommSettings, responeGWMessageECTHRData);

                IWebAPIMessage2MamanAnalyzer analyzer = null;
                


                
                if (!string.IsNullOrWhiteSpace(@intrface.ResponseCode))
                {
                    courier2MamanCommSettings.RqstCommLogID = _WaitingCommLog.Id;
                    analyzer = new SetInAnalyzeQResponseService();
                }
                else
                {
                    throw new Exception("All send web api must have @intrface.ResponseCode ");
                    ///analyzer = customsPartnerFtpDetails.GetResponseService(courier2MamanCommSettings.MessageCode);
                }

                analyzer.AnalyzeResponse(courier2MamanCommSettings, webAPIResultString);



                _WaitingCommLog.CommunicationStatusTypeCode = "D";
                _WaitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(_WaitingCommLog.Tenant);
                _WaitingCommLog.DoneDateUTC = DateTime.UtcNow;
                _WaitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(_WaitingCommLog.Tenant);
                _WaitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
                _WaitingCommLog.Logs = LogMessagingUtil.Instance.ToString();
                //waitingCommLog.Logs
                _CommunicationLogRep.Update(_WaitingCommLog);
                _CommunicationLogRep.SubmitChanges();
            }
            catch (Exception e)
            {


                throw;
            }
            return true;
        }

        public void DebugStep(string communicationLogId, string @interface, int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                _Tenant = tenant;
                _CommunicationLogId = communicationLogId;
                _Context = CommonDataContext.GetContext(_Tenant);
                _CommunicationLogRep = new CommunicationLogRepository(_Context);

                _WaitingCommLog = _CommunicationLogRep.GetSingleCommunicationLog(_CommunicationLogId, _Tenant);
                SentWAPIComm(true);
                scope.Complete();
            }
        }
    }

    public class WebAPI2BearerMamanMessage
    {
        const string relativeUriToken = "Token";
        const string BEARER_TOKEN = "Bearer";
        const string agent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
        //StringBuilder _StringBuilder = new StringBuilder();
        private CourierWEBAPICommSettings _CourierHawbMamanCommunicationLogSettings;

        public WebAPI2BearerMamanMessage(CourierWEBAPICommSettings courierHawbMamanCommunicationLogSettings)
        {
            this._CourierHawbMamanCommunicationLogSettings = courierHawbMamanCommunicationLogSettings;
        }

        public string PostIt(string dataJson)
        {
            string myResultString = "";
            int retryCount = 2;

            try
            {
                for (int attempt = 0; attempt < retryCount; attempt++)
                {
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            client.Timeout = TimeSpan.FromMinutes(MyWebClient.TimeOutFromMinutes);

                            var tokenManager = TokenManager.GetInstance(_CourierHawbMamanCommunicationLogSettings);
                            var token = tokenManager.Token;

                            NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Attempt {attempt + 1}: Token: {token.AccessToken}");

                            // Prepare the request
                            client.DefaultRequestHeaders.Add("User-Agent", agent);
                            var content = new StringContent(dataJson, Encoding.UTF8, "application/json");
                            client.DefaultRequestHeaders.Add("Authorization", $"{token.Token_Type} {token.AccessToken}");

                            LogMessagingUtil.Instance.AppendLine("URIBaldarCreateECTHRMessgae.PostAsync....");
                            var task = client.PostAsync(_CourierHawbMamanCommunicationLogSettings.URIMethod, content);
                            Wait4Finsh(task, 3);

                            var response = task.Result;

                            // Log response status code and headers before attempting to read the body
                            NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Response Status Code: {response.StatusCode}");
                            NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Response Headers: {response.Headers}");

                            // Log the content type to understand what is expected
                            if (response.Content.Headers.ContentType != null)
                            {
                                NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Content Type: {response.Content.Headers.ContentType.MediaType}");
                            }

                            // Attempt to read the response content
                            var responseContent = response.Content.ReadAsStringAsync().Result;
                            NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"[DEBUG] Raw Response Content: {responseContent}");

                            // Now you can check if the response starts with '<', indicating it might be XML
                            if (responseContent.Trim().StartsWith("<"))
                            {
                                NetCommonHelper.Logger.DevLog.Instance.WriteTrace("[WARNING] The response appears to be XML or HTML instead of JSON.");
                            }

                            if (response.IsSuccessStatusCode)
                            {
                                // If the request succeeds, return the result
                                myResultString = response.Content.ReadAsStringAsync().Result;
                                LogMessagingUtil.Instance.AppendLine($"PostAsyncResult={myResultString}");
                                return myResultString;  // Success, return the response
                            }
                            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                            {
                                NetCommonHelper.Logger.DevLog.Instance.WriteTrace("401 Unauthorized detected. Refreshing token...");
                                tokenManager.RefreshToken();
                            }
                            else
                            {
                                NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Request failed with status code: {response.StatusCode}");
                            }
                        }
                        catch (Exception ex)
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Attempt {attempt + 1} failed with exception: {ex.Message}");
                            if (attempt == retryCount - 1)
                            {
                                throw new Exception($"An error occurred on the last retry: {ex.Message}");
                            }
                        }
                    }
                }

                // After all attempts fail, throw an exception
                throw new Exception("All retry attempts failed.");
            }
            catch (Exception ex)
            {
                // Log the basic exception message
                NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"An error occurred: {ex.Message}");

                // Log the full exception details including the stack trace
                NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Exception Details: {ex.ToString()}");

                // Log the inner exception details if available
                if (ex.InnerException != null)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Inner Exception: {ex.InnerException.Message}");
                    NetCommonHelper.Logger.DevLog.Instance.WriteTrace($"Inner Exception Details: {ex.InnerException.ToString()}");
                }

                // Re-throw the exception to ensure it propagates up the stack
                LogMessagingUtil.Instance.AppendLine($"An error occurred: {ex.Message}");
                LogMessagingUtil.Instance.AppendLine($"Exception Details: {ex.ToString()}");
                throw;
            }
        }
        private static void Wait4Finsh(Task
          task, int TimeOutInMin)
        {
            try
            {
                var ts = Stopwatch.StartNew();
                //task.Start();
                while (ts.Elapsed < TimeSpan.FromMinutes(TimeOutInMin))
                {
                    task.Wait(TimeSpan.FromSeconds(1));
                    if (task.IsCompleted)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (!task.IsCompleted)
                {
                    task.Dispose();
                    throw new Exception($"Timeout SendWEBAPIMessage2MamanWR {TimeOutInMin} Min ");

                }
            }
            catch (AggregateException ae)
            {
                throw new Exception($"Exception(s) occurred during task execution: {string.Join(", ", ae.InnerExceptions.Select(inner => inner.Message))}");
                throw;
            }
        }
    }


    class MyWebClient : WebClient
    {
        public const int TimeOutFromMinutes = 3;
            

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = (int)TimeSpan.FromMinutes(TimeOutFromMinutes).TotalMilliseconds;
            return w;
        }
    }

    public class WebAPINetworkCredentialMessage
    {
        private CourierWEBAPICommSettings _CourierCommunicationLogSettings;

        public WebAPINetworkCredentialMessage(CourierWEBAPICommSettings courierHawbMamanCommunicationLogSettings)
        {
            this._CourierCommunicationLogSettings = courierHawbMamanCommunicationLogSettings;
        }

        public string PostIt(string dataJson)
        {
            string myResultString = "";

            string access_token = "";
            string token_type = "";

            try
            {


               
                {
                    //שם המשתמש: ovrs\crmamital
                    //סיסמה: Amital123456

                    var myCredentials = new NetworkCredential("", "", "");
                    myCredentials.UserName = _CourierCommunicationLogSettings.username;//  @"ovrs\crmamital";
                    myCredentials.Password = _CourierCommunicationLogSettings.password;// "Amital123456";


                    
                    using (var client = new MyWebClient())
                    {
                        
                        client.UseDefaultCredentials = false;
                        client.Credentials = myCredentials;
                        client.Encoding = System.Text.Encoding.UTF8;

                        var dataString = dataJson;
                        //client.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=UTF-8");
                        client.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                        myResultString =
                        client.UploadString(
                            new Uri(_CourierCommunicationLogSettings.URIMethod /*@"http://81.218.57.34:9094/api/Courier/UpdateHawbStatus"*/), 
                            "POST", 
                            dataString);

                        Console.WriteLine("success");
                    }


                }



                return myResultString;
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        public static void OVSUpdateHawbStatusTester()
        {
            //שם המשתמש: ovrs\crmamital
            //סיסמה: Amital123456

            var myCredentials = new NetworkCredential("", "", "");
            myCredentials.UserName = @"ovrs\crmamital";
            myCredentials.Password = "Amital123456";


            var postData = @"{""CourierCompanyVat"":""514193408"",""CourierHawbNumber"":""99994668068"",""CourierHawbDate"":""2019-02-12T00: 00:00"",""MawbPrefix"":""114"",""Mawb"":15381173,""Hawb"":""1514112"",""FlightNumber"":316,""FltDate"":null,""EstimatedArrivalDate"":""2018-12-24T20:00:00"",""PackageQuantity"":1,""Weight"":0.30,""GoodValueInUSD"":12.0,""Description"":""IBOX 2331"",""ImporterName"":""Kobi Cohen"",""ImporterAddress"":""Dekel 27 2nd avenu 13 ddk Tel Aviv ISRAEL"",""DistributionLine"":"""",""DistributionCompanyVat"":"""",""DistributorHP"":null,""DistributorName"":null,""DeclarationNumber"":""19041052508346"",""CustomsSuspention"":"""",""Preclearence"":false}";

            using (var client = new MyWebClient())
            {
                client.UseDefaultCredentials = false;
                client.Credentials = myCredentials;
                var url = //"http://localhost:93/Api/Test";
                    @"http://81.218.57.34:9094/api/Courier/SpecialActionReporting";
                //var json = client.DownloadString(url);

                var dataString = postData;//JsonConvert.SerializeObject(vm);
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.UploadString(new Uri(@"http://81.218.57.34:9094/api/Courier/UpdateHawbStatus"), "POST", dataString);

                Console.WriteLine("success");
            }


        }

        public static void OVSUpdateHawbStatusTesterNotWork()
        {
            //שם המשתמש: ovrs\crmamital
            //סיסמה: Amital123456

            var myCredentials = new NetworkCredential("", "", "");
            myCredentials.UserName = @"ovrs\crmamital";
            myCredentials.Password = "Amital123456";
            //using (var client = new MyWebClient())
            //{
            //    client.UseDefaultCredentials = false;
            //    client.Credentials = myCredentials;
            //    var url = //"http://localhost:93/Api/Test";
            //        @"http://81.218.57.34:9094/api/Courier/SpecialActionReporting";
            //    var json = client.DownloadString(url);
            //    Console.WriteLine("success");
            //}







            WebRequest request = WebRequest.Create(@"http://81.218.57.34:9094/api/Courier/UpdateHawbStatus");

            request.Method = "POST";
            request.UseDefaultCredentials = false;
            request.PreAuthenticate = true;
            request.Credentials = myCredentials;

            // Create POST data and convert it to a byte array.
            var postData = @"{""CourierCompanyVat"":""514193408"",""CourierHawbNumber"":""99376529290"",""CourierHawbDate"":""2019 - 02 - 12T00: 00:00"",""MawbPrefix"":""114"",""Mawb"":15381173,""Hawb"":""1514112"",""FlightNumber"":316,""FltDate"":null,""EstimatedArrivalDate"":""2018 - 12 - 24T20: 00:00"",""PackageQuantity"":1,""Weight"":0.30,""GoodValueInUSD"":12.0,""Description"":""IBOX 2331"",""ImporterName"":""Kobi Cohen"",""ImporterAddress"":""Dekel 27 2nd avenu 13 ddk Tel Aviv ISRAEL"",""DistributionLine"":"""",""DistributionCompanyVat"":"""",""DistributorHP"":null,""DistributorName"":null,""DeclarationNumber"":""19041052508346"",""CustomsSuspention"":"""",""Preclearence"":false}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();



        }

        public static void OVSSpecialActionReportingTester()
        {
            //שם המשתמש: ovrs\crmamital
            //סיסמה: Amital123456

            var myCredentials = new NetworkCredential("", "", "");
            myCredentials.UserName = @"ovrs\crmamital";
            myCredentials.Password = "Amital123456";
            //using (var client = new MyWebClient())
            //{
            //    client.UseDefaultCredentials = false;
            //    client.Credentials = myCredentials;
            //    var url = //"http://localhost:93/Api/Test";
            //        @"http://81.218.57.34:9094/api/Courier/SpecialActionReporting";
            //    var json = client.DownloadString(url);
            //    Console.WriteLine("success");
            //}







            WebRequest request = WebRequest.Create(@"http://81.218.57.34:9094/api/Courier/SpecialActionReporting");

            request.Method = "POST";
            request.UseDefaultCredentials = false;
            request.PreAuthenticate = true;
            request.Credentials = myCredentials;

            // Create POST data and convert it to a byte array.
            var postData = @"{""MessageType"":""C"",""CourierCompanyVat"":""61340333"",""CourierHawbNumber"":""514193408"",""CourierHawbDate"":""2019 - 01 - 31T00: 00:00"",""SpecialActionCode"":""2"",""LabelText1"":"""",""LabelText2"":"""",""LabelText3"":"""",""LabelText4"":"""",""LabelText5"":""""}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();



        }

    }

}
