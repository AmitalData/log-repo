
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

namespace CustomsWorkerRole
{
    public class SendWebAPI2MamanGWMessageECTHRDataWR : CustomsWorkerEntryPoint
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
        private IQueueService _IQueueService;
        private ICommonDataContext _ICommonDataContext;

        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;
                string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment(SBQueueNames.SendGWMessageECTHRData2MamanQ.ToString()); //Amitalqueue


                var myClass = this.GetType().Name;
                _IQueueService = new DbQueueService();
                _IQueueService.InitializeQueue(SBQueueNames.SendGWMessageECTHRData2MamanQ.ToString(), 0);


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
            while (true)
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    var receivedMessage = _IQueueService.Receive();

                    if (receivedMessage == null || String.IsNullOrWhiteSpace(receivedMessage.MessageId))
                    {
                        //Thread.Sleep(TimeSpan.FromSeconds(5));
                        Thread.Sleep(TimeSpan.FromSeconds(15));//not using soo mach 
                        break;
                    }


                    ProcessMessage_Db(receivedMessage);
                    scope.Complete();
                }
            }
        }





        private void ProcessMessage_Db(QueueResponse queueResponse)
        {
            int tenant = 0;
            try
            {

                if (queueResponse.MessageId != null)
                {

                    string communicationLogId = queueResponse.MessageValues["CommunicationLogId"].ToString();
                    int.TryParse(queueResponse.MessageValues["Tenant"].ToString(), out tenant);
                    _ICommonDataContext = CommonDataContext.GetContext(tenant);
                    CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(_ICommonDataContext);
                    CommunicationLog cl = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);

                    bool processEnebled = true;

                    if (processEnebled)
                    {
                        if (cl != null)
                        {
                            if (cl.CommunicationStatusTypeCode == "D")
                            {
                                _IQueueService.Complete();
                            }
                            else
                            {
                                SendCommunicationLog(communicationLogId, tenant, cl, communicationLogRep);
                                _IQueueService.Complete();

                                LogDoneItemInMemory();

                            }
                        }
                        else
                        {
                            if (queueResponse.RetryNumber <= 11)
                            {
                                if (queueResponse.RetryNumber < 3)
                                {
                                    _IQueueService.Delay(new TimeSpan(0, 0, 0, 1));
                                }

                                if (queueResponse.RetryNumber >= 3 && queueResponse.RetryNumber <= 5)
                                {
                                    _IQueueService.Delay(new TimeSpan(0, 0, 0, 5));
                                }

                                if (queueResponse.RetryNumber > 5 && queueResponse.RetryNumber <= 10)
                                {

                                    _IQueueService.Delay(new TimeSpan(0, 0, 0, 10));
                                    AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + queueResponse.RetryNumber
                                        + ",at utc time:" + DateTime.UtcNow + ",at FTPCommunicationLogQueue worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                    Thread.Sleep(3000);
                                }
                                if (queueResponse.RetryNumber == 11)
                                {

                                    _IQueueService.Delay(new TimeSpan(0, 0, 2, 0));
                                    AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + queueResponse.RetryNumber
                                    + ",at utc time:" + DateTime.UtcNow + ",at FTPCommunicationLog worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                    Thread.Sleep(10000);

                                }
                            }
                            else
                            {
                                _IQueueService.Complete();
                                AzureLog.SaveLogsInStorage("couldn't find communication log and the message is completed: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + queueResponse.RetryNumber
                                    + ",at utc time:" + DateTime.UtcNow + ",at FTPCommunicationLog worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                            }
                        }
                    }
                }



            }
            catch (Exception ex)
            {


                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "SendWebAPI2MamanGWMessageECTHRDataWR : ProcessMessage() Method", null);
                Thread.Sleep(10000);
            }
        }

        private void SendCommunicationLog(string communicationLogId, int tenant, CommunicationLog cl, CommunicationLogRepository communicationLogRep)
        {
            try
            {
                if (cl.Retries < 5)
                {
                    PostWebAPI(cl, communicationLogRep);
                }

                else
                {
                    if (_ICommonDataContext != null)
                    {
                        cl.CommunicationStatusTypeCode = "F";
                        cl.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
                        communicationLogRep.Update(cl);
                        communicationLogRep.SubmitChanges();
                    }

                }
            }

            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);

                //Change number of retries

                cl.Retries++;
                cl.ExceptionMessage = exc.Message;
                if (exc.InnerException != null)
                {
                    cl.ExceptionMessage = cl.ExceptionMessage + Environment.NewLine + exc.InnerException;
                }
                if (exc.StackTrace != null)
                {
                    cl.ExceptionMessage = cl.ExceptionMessage + Environment.NewLine + "Stack trace: " + exc.StackTrace;
                }
                SetNextTryDateTime(cl);
                if (_ICommonDataContext != null)
                {
                    communicationLogRep.Update(cl);
                    communicationLogRep.SubmitChanges();
                }
                throw;

            }
        }
        private void SetNextTryDateTime(CommunicationLog cl)
        {
            string newLog = null;
            DateTime date = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
            DateTime dateUtc = DateTime.UtcNow;
            switch (cl.Retries)
            {
                case 1:
                case 2:
                    {
                        cl.NextTryDateTime = date.AddSeconds(1);
                        cl.NextTryDateTimeUTC = dateUtc.AddSeconds(1);
                        _IQueueService.Delay(new TimeSpan(0, 0, 0, 1));

                        break;
                    }
                case 3:
                case 4:
                    {
                        cl.NextTryDateTime = date.AddSeconds(5);
                        cl.NextTryDateTimeUTC = dateUtc.AddSeconds(5);
                        _IQueueService.Delay(new TimeSpan(0, 0, 0, 5));
                        break;
                    }
                case 5:
                    {
                        cl.NextTryDateTime = date.AddMinutes(1);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(1);
                        _IQueueService.Delay(new TimeSpan(0, 0, 1));
                        break;
                    }
                case 6:
                case 7:
                case 8:
                    {
                        cl.NextTryDateTime = date.AddMinutes(2);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(2);
                        _IQueueService.Delay(new TimeSpan(0, 0, 2));
                        break;
                    }
                case 9:
                    {
                        cl.NextTryDateTime = date.AddMinutes(5);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(5);
                        _IQueueService.Delay(new TimeSpan(0, 0, 5));
                        break;
                    }
                case 10:
                    {
                        cl.NextTryDateTime = date.AddMinutes(10);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(10);
                        _IQueueService.Delay(new TimeSpan(0, 0, 10));
                        break;
                    }
                default:
                    {
                        cl.NextTryDateTime = date.AddMinutes(20);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(20);
                        _IQueueService.Delay(new TimeSpan(0, 0, 20));
                        break;
                    }
            }
            cl.Logs += Environment.NewLine + "Retry #" + cl.Retries + " Next Retry: " + cl.NextTryDateTimeUTC.ToString();
        }

        private void PostWebAPI(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep)
        {
            try
            {


                LogMessagingUtil.Instance.Clear();
                int tenant = waitingCommLog.Tenant;
                ICommonDataContext commoncontext = CommonDataContext.GetContext(waitingCommLog.Tenant);
                DocumentRepository documentRepository = new DocumentRepository(commoncontext);

                Document document = documentRepository.GetSingleDocument(waitingCommLog.Tenant, waitingCommLog.DocumentId);
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
                if (string.IsNullOrEmpty(waitingCommLog.LogSettings))
                {
                    throw new Exception("string.IsNullOrEmpty(waitingCommLog.LogSettings)");
                }
                var dataJson = System.Text.Encoding.UTF8.GetString(filedata.ToArray());
                var courierHawbMamanCommunicationLogSettings = JsonConvert.DeserializeObject<CourierHawbMamanCommunicationLogSettings>(waitingCommLog.LogSettings);
                if (courierHawbMamanCommunicationLogSettings == null)
                {
                    throw new Exception("(courierHawbMamanCommunicationLogSettings == null)");
                }
                LogMessagingUtil.Instance.AppendLine("courierHawbMamanCommunication DB is valid");
                GWMessageECTHRData responeGWMessageECTHRData = null;
                LogMessagingUtil.Instance.AppendLine($"Post {courierHawbMamanCommunicationLogSettings.host}");
                string webAPIResultString = null;
                try
                {
                    webAPIResultString = PostIt(courierHawbMamanCommunicationLogSettings.host, dataJson);
                }
                catch (Exception)
                {

                    throw;
                }
                LogMessagingUtil.Instance.AppendLine("webAPIResultString:"+ webAPIResultString);
                responeGWMessageECTHRData = JsonConvert.DeserializeObject<GWMessageECTHRData>(webAPIResultString);

                var myWebAPICourierHawbMamanService = new WebAPICourierGWMessageECTHRDataMamanService();
                myWebAPICourierHawbMamanService.AnalyzeResponse(courierHawbMamanCommunicationLogSettings, responeGWMessageECTHRData);




                waitingCommLog.CommunicationStatusTypeCode = "D";
                waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                waitingCommLog.DoneDateUTC = DateTime.UtcNow;
                waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
                waitingCommLog.Logs = LogMessagingUtil.Instance.ToString();
                //waitingCommLog.Logs
                communicationLogRep.Update(waitingCommLog);
                communicationLogRep.SubmitChanges();
            }
            catch (Exception e)
            {
                using (var scope = TransactionFactory.GetNewTransaction())
                {
                    waitingCommLog.Logs = e.ToString() + LogMessagingUtil.Instance.ToString();
                    //waitingCommLog.Logs
                    communicationLogRep.Update(waitingCommLog);
                    communicationLogRep.SubmitChanges();
                    scope.Complete();
                }

                throw;
            }
        }

        private static string PostIt(string host, string dataJson)
        {
            string myResultString = "";

            using (var client = new HttpClient())
            {
                //var GetURI = URI + "ImporterShipmentDocuments/GetIfNew?id=" + DocumentFilingPM.CustomerDocumentId + "&tenant=" + importerTenant;// +"&importertenant=" + importerTenant;

                string webApiURI = host;//URI + "APIAuthentication";


                var content = new StringContent(dataJson, Encoding.UTF8, "application/json");
                
                var task = client.PostAsync(webApiURI, content);
                Wait4Finsh(task, 1);
                myResultString = task.Result.Content.ReadAsStringAsync().Result;

            }



            return myResultString;
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
                throw new Exception("Timeout SendWebAPI2MamanGWMessageECTHRDataWR 2Min ");

            }
        }

    }

}
