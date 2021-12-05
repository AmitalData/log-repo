
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

        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;
                DoneItemsInRange = new Dictionary<DateTime, int>();


                var myClass = this.GetType().Name;




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

                WorkUntilQEmpty_Db();


            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }


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

                        _ReceivedBrokeredMessage = _IQueueService.Receive();

                        if (_ReceivedBrokeredMessage == null || String.IsNullOrWhiteSpace(_ReceivedBrokeredMessage.MessageId))
                        {
                            //Thread.Sleep(TimeSpan.FromSeconds(5));
                            Thread.Sleep(TimeSpan.FromSeconds(15));//not using soo mach 
                            break;
                        }
                        if (_ReceivedBrokeredMessage.RetryNumber > 5)
                        {
                            _IQueueService.CompleteAsFailed();
                        }

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

            LogMessagingUtil.Instance.Clear();

            _CommunicationLogId = _ReceivedBrokeredMessage.MessageValues["CommunicationLogId"].ToString();
            int.TryParse(_ReceivedBrokeredMessage.MessageValues["Tenant"].ToString(), out _Tenant);

            LogMessagingUtil.Instance
                .AppendLine("ProccessReceivedMessage()")
                .AppendLine("QUEUEMessageId:" + _ReceivedBrokeredMessage.MessageId)
                .AppendLine("RetryNumber:" + _ReceivedBrokeredMessage.RetryNumber)
                .AppendLine("CommunicationLogId:" + _CommunicationLogId)
                .AppendLine(",Tenant" + _Tenant);

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
                ExceptionHandler.HandleException(exc, DateTime.Now, _Tenant, "", "WorkerRole", "", null);
                _WaitingCommLog.Retries++;

                _WaitingCommLog.CommunicationStatusTypeCode = "F";

                var s = "ProccessReceivedMessage()Exception:" + exc.Message;
                _WaitingCommLog.ExceptionMessage = s.Substring(0, Math.Min(7999, s.Length));
                _CommunicationLogRep.Update(_WaitingCommLog);
                _CommunicationLogRep.SubmitChanges();
                _IQueueService.Delay(TimeSpan.FromMinutes(1));
                //throw;

            }
        }

        private void SentWAPIComm(bool forceRetryFromTester=false)
        {
            if (forceRetryFromTester ||_ReceivedBrokeredMessage.RetryNumber < 5)
            {
                LastActivity = DateTime.UtcNow;
                LogMessagingUtil.Instance.Append("DoAction(PostWebAPI)..");

                PostWebAPIAnalyzeAndSaveCommDone();//if failed throw exception
                if (!forceRetryFromTester)
                {
                    _IQueueService.Complete();
                }
                
                this.LogDoneItemInMemory();
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("RetryNumber >= 5>>> Failed ");
                _WaitingCommLog.CommunicationStatusTypeCode = "F";
                _IQueueService.Complete();
            }
            LogMessagingUtil.Instance.AppendLine(":" + _WaitingCommLog.CommunicationStatusTypeCode);
            _CommunicationLogRep.Update(_WaitingCommLog);
            _CommunicationLogRep.SubmitChanges();
        }


        private bool PostWebAPIAnalyzeAndSaveCommDone()
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

            string access_token = "";
            string token_type = "";

            try
            {


                using (var client = new HttpClient())
                {
                    //var GetURI = URI + "ImporterShipmentDocuments/GetIfNew?id=" + DocumentFilingPM.CustomerDocumentId + "&tenant=" + importerTenant;// +"&importertenant=" + importerTenant;

                    var ADD = "User-Agent: Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
                    client.DefaultRequestHeaders.Add("User-Agent", agent);

                    //var tokenUri = new Uri(new Uri(host), relativeUriToken);
                    //webApiURI = @"https://maman.wsfreeze.co.il/WebAPIExt/Token"; //HTTP/1.1;
                    string tokenReq = "grant_type=password&username=f_moshe&Password=******";
                    tokenReq = $"grant_type=password&username={HttpUtility.UrlEncode(_CourierHawbMamanCommunicationLogSettings.username)}&Password={HttpUtility.UrlEncode(_CourierHawbMamanCommunicationLogSettings.password)}";
                    
                    var content = new StringContent(tokenReq, Encoding.UTF8, "application/x-www-form-urlencoded");
                    LogMessagingUtil.Instance.AppendLine($"PostAsync({_CourierHawbMamanCommunicationLogSettings.URIToken}, {content})");
                    var task = client.PostAsync(_CourierHawbMamanCommunicationLogSettings.URIToken, content);
                    Wait4Finsh(task, 1);
                    myResultString = task.Result.Content.ReadAsStringAsync().Result;
                    //{"access_token":"zkKDt-XnqqM5uoyDwrPxDPHb_vM5hplsUKr7sT5GA2w8Vpsl_HT5eBidUriiyw3Gn-Mne0NIq2LQO7MMT525GdrFutDzIQpRKR6c7oz2GbdSdGdEY3S3nfP0W7svtmShEeUx23SbW8ysLkyAnFP-IdQhvMs2lzxzHIDrnDqm_agwq54x9UiiDa5-9ZkEWBUrN83U4B5qddiYTU0whODGvrxEE9wyrQKoygG3Gi48gwv2_TI4H9yrd2Uys9l_jBivOsRRm1oXtyGsyIq9DwDn7pmcoxUjz-yNwm_hp18Y1qi4aXk1Z8IjeKRQl_8FMUg-","token_type":"bearer","expires_in":35999,"UserName":"F_unitedf","role":"General",".issued":"Mon, 12 Nov 2018 14:48:43 GMT",".expires":"Tue, 13 Nov 2018 00:48:43 GMT"}
                    LogMessagingUtil.Instance.AppendLine($"PostAsyncResult ({myResultString})");
                    dynamic d = JsonConvert.DeserializeObject(myResultString);
                    access_token = d.access_token;
                    token_type = d.token_type;
                    //token_type=bearer
                }

                using (var client = new HttpClient())
                {
                    //var GetURI = URI + "ImporterShipmentDocuments/GetIfNew?id=" + DocumentFilingPM.CustomerDocumentId + "&tenant=" + importerTenant;// +"&importertenant=" + importerTenant;

                    //string webApiURI = host;//URI + "APIAuthentication";
                    //webApiURI = "https://maman.wsfreeze.co.il/WebAPIExt/api/baldar/CreateECTHRMessgae";
                    client.DefaultRequestHeaders.Add("User-Agent", agent);
                    var content = new StringContent(dataJson, Encoding.UTF8, "application/json");
                    //Authorization: <type> <credentials>

                    string credentials = "";
                    //Authorization: Bearer O5GRnBFMruLRIdRJAI_CQNLzXanWBQ0FO4zQGR6gkluiYOWTaop-p_UkEfq0NaoIuFC_kLfJjABjJdN5HW0_aC-kTMS63nHKUb9yiCxOOiv5UmrCvd1XLgFbBxCLwdDcCnwiCgdM_CTkhM_cFX5KWsNyWAD9i85wyk06lV-iROw2itvXo3Vir-19fMiTZnFbe_OffXJWfl2lF89zXT_MYzlOJdCqDRYELSwAPjBcPzLva5-EN4Pi2Jyu-nZs7DxW5NcEDM6JJUDk66C7VXxqz5s3Q4D4Knr14lmYMmetdAY
                    //credentials = "O5GRnBFMruLRIdRJAI_CQNLzXanWBQ0FO4zQGR6gkluiYOWTaop-p_UkEfq0NaoIuFC_kLfJjABjJdN5HW0_aC-kTMS63nHKUb9yiCxOOiv5UmrCvd1XLgFbBxCLwdDcCnwiCgdM_CTkhM_cFX5KWsNyWAD9i85wyk06lV-iROw2itvXo3Vir-19fMiTZnFbe_OffXJWfl2lF89zXT_MYzlOJdCqDRYELSwAPjBcPzLva5-EN4Pi2Jyu-nZs7DxW5NcEDM6JJUDk66C7VXxqz5s3Q4D4Knr14lmYMmetdAY";
                    client.DefaultRequestHeaders.Add("Authorization", $"{token_type} {access_token}");
                    LogMessagingUtil.Instance.AppendLine($"URIBaldarCreateECTHRMessgae.PostAsync....");
                    var task = client.PostAsync(_CourierHawbMamanCommunicationLogSettings.URIMethod, content);
                    Wait4Finsh(task, 1);
                    myResultString = task.Result.Content.ReadAsStringAsync().Result;
                    LogMessagingUtil.Instance.AppendLine($"PostAsyncResult={myResultString }");

                }



                return myResultString;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private static void Wait4Finsh(Task
          task, int TimeOutInMin)
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
                throw new Exception("Timeout SendWEBAPIMessage2MamanWR 2Min ");

            }
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


                    
                    using (var client = new WebClient())
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

            using (var client = new WebClient())
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
            //using (var client = new WebClient())
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
            //using (var client = new WebClient())
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
