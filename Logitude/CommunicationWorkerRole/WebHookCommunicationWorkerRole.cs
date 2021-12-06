using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.WebHook;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using WebFreight.Web.Helpers.AutomationModel;

namespace CommunicationWorkerRole
{
    public class WebHookCommunicationWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
        ICommonDataContext context;
        CommunicationLogRepository communicationLogRep;
        public override void Run()
        {
            while (IsRunning)
            {
                StartWork();
            }
        }

        public void StartWork()
        {
            if (General.IsUpdating())
            {
                Thread.Sleep(60000);
            }
            try
            {
                InitializeQueueService();
                QueueResponse queueResponse = queueservice.Receive(new TimeSpan(0, 0, 0, 10));

                if (queueResponse.MessageId != null)
                {
                    GetStartedInQueueMessage(queueResponse);
                }
            }
            catch (Exception ex)
            {
                ConnectClient();
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "WebHookCommunicationLog worker role start", null, null);
                Thread.Sleep(10000);
            }

        }

        private void InitializeQueueService()
        {
            queueservice = new DbQueueService();
            queueservice.InitializeQueue("WebHookCommunicationLogQueue", 0);
        }

        private void GetStartedInQueueMessage(QueueResponse queueResponse)
        {
            int tenant = 0;
            string communicationLogId = queueResponse.MessageValues["CommunicationLogId"].ToString();
            int.TryParse(queueResponse.MessageValues["Tenant"].ToString(), out tenant);
            CommunicationLog communicationLog = GetCommunicationLogByIdAndTenant(communicationLogId, tenant);

            if (communicationLog != null)
                HandleCommunicationLog(tenant, communicationLog);
            else
                HandleRetryMechanism(queueResponse, tenant, communicationLogId);
        }

        private CommunicationLog GetCommunicationLogByIdAndTenant(string communicationLogId, int tenant)
        {
            context = CommonDataContext.GetContext(tenant);
            communicationLogRep = new CommunicationLogRepository(context);
            CommunicationLog communicationLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
            return communicationLog;
        }

        private void HandleCommunicationLog(int tenant, CommunicationLog communicationLog)
        {
            if (communicationLog.CommunicationStatusTypeCode == "D")
            {
                queueservice.Complete();
            }
            else
            {
                SendCommunicationLog(tenant, communicationLog, communicationLogRep);
                queueservice.Complete();
                LogDoneItemInMemory();
            }
        }

        private void SendCommunicationLog(int tenant, CommunicationLog communicationLog, CommunicationLogRepository communicationLogRepository)
        {
            try
            {
                if (communicationLog.Retries < 5)
                {
                    SendWaitingCommunicationLog(communicationLog, communicationLogRepository);
                }
                else
                {
                    UpdateCommunicationLogAsFailed(communicationLog, communicationLogRepository);
                }
            }
            catch (WebHookServiceException webHookServiceException)
            {
                HandleWebHookServiceException(communicationLog, communicationLogRepository, webHookServiceException);
                throw;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception, DateTime.Now, tenant, "", "WorkerRole", "", null);
                HandleCommunicationLogException(communicationLog, communicationLogRepository, exception);
                throw;
            }
        }

        private void SendWaitingCommunicationLog(CommunicationLog waitingCommunicationLog, CommunicationLogRepository communicationLogRepository)
        {
            int tenant = waitingCommunicationLog.Tenant;
            ICommonDataContext commoncontext = CommonDataContext.GetContext(waitingCommunicationLog.Tenant);
            DocumentRepository documentRepository = new DocumentRepository(commoncontext);
            Document document = documentRepository.GetSingleDocument(waitingCommunicationLog.Tenant, waitingCommunicationLog.DocumentId);
            byte[] filedata = GetFileDataFromStorageByDocumentId(tenant, document);

            if (filedata == null)
            {
                throw new Exception("The file data was not found!");
            }

            byte[] fileDataWithBodyWord = ConcatinateFileDataWithBodyWord(filedata);

            if (!string.IsNullOrEmpty(waitingCommunicationLog.LogSettings))
            {
                UploadFileDataIntoWebHook(waitingCommunicationLog, document, fileDataWithBodyWord);
            }

            UpdateCommunicationLogAsDone(waitingCommunicationLog, communicationLogRepository);
        }

        private static byte[] GetFileDataFromStorageByDocumentId(int tenant, Document document)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = document.FileSize,
            };

            Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            byte[] filedata = storageservice.Read(fileInfo);
            return filedata;
        }

        private static byte[] ConcatinateFileDataWithBodyWord(byte[] filedata)
        {
            byte[] bodyByteArrayWord = Encoding.UTF8.GetBytes("body=");

            byte[] fileDataWithBodyWord = new byte[bodyByteArrayWord.Length + filedata.Length];
            System.Buffer.BlockCopy(bodyByteArrayWord, 0, fileDataWithBodyWord, 0, bodyByteArrayWord.Length);
            System.Buffer.BlockCopy(filedata, 0, fileDataWithBodyWord, bodyByteArrayWord.Length, filedata.Length);
            return fileDataWithBodyWord;
        }

        private static void UploadFileDataIntoWebHook(CommunicationLog waitingCommunicationLog, Document document, byte[] fileDataWithBodyWord)
        {
            WebHookCommunicationLogSettings settingsData = JsonConvert.DeserializeObject<WebHookCommunicationLogSettings>(waitingCommunicationLog.LogSettings);
            if (settingsData != null)
            {
                string webHookURL = settingsData.URL;
                WebHookAuthorization webHookAuthorization = settingsData.WebHookAuthorization;
                string fileName = (!string.IsNullOrEmpty(settingsData.Filename) ? settingsData.Filename : document.Id) + "." + document.Extension;

                WebHookService webHookService = new WebHookService(webHookURL, webHookAuthorization);

                string responseMessage = webHookService.Upload(fileName, fileDataWithBodyWord);
                waitingCommunicationLog.Logs += Environment.NewLine + DateTime.Now.ToString() + " : " + responseMessage;

            }
        }

        private static void UpdateCommunicationLogAsDone(CommunicationLog communicationLog, CommunicationLogRepository communicationLogRepository)
        {
            communicationLog.CommunicationStatusTypeCode = "D";
            communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(communicationLog.Tenant);
            communicationLog.DoneDateUTC = DateTime.UtcNow;
            communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(communicationLog.Tenant);
            communicationLog.LastStatusDateUTC = DateTime.UtcNow;
            communicationLogRepository.Update(communicationLog);
            communicationLogRepository.SubmitChanges();
        }

        private void UpdateCommunicationLogAsFailed(CommunicationLog communicationLog, CommunicationLogRepository communicationLogRepository)
        {
            if (context != null)
            {
                communicationLog.CommunicationStatusTypeCode = "F";
                communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(communicationLog.Tenant);
                communicationLogRepository.Update(communicationLog);
                communicationLogRepository.SubmitChanges();
            }
        }

        private void HandleWebHookServiceException(CommunicationLog communicationLog, CommunicationLogRepository communicationLogRepository, WebHookServiceException webHookServiceException)
        {
            communicationLog.Retries++;
            communicationLog.ExceptionMessage = webHookServiceException.Message;
            SetNextTryDateTime(communicationLog);
            if (context != null)
            {
                communicationLogRepository.Update(communicationLog);
                communicationLogRepository.SubmitChanges();
            }
        }

        private void HandleCommunicationLogException(CommunicationLog communicationLog, CommunicationLogRepository communicationLogRepository, Exception exception)
        {
            communicationLog.Retries++;
            communicationLog.ExceptionMessage = exception.Message;
            if (exception.InnerException != null)
            {
                communicationLog.ExceptionMessage = communicationLog.ExceptionMessage + Environment.NewLine + exception.InnerException;
            }
            if (exception.StackTrace != null)
            {
                communicationLog.ExceptionMessage = communicationLog.ExceptionMessage + Environment.NewLine + "Stack trace: " + exception.StackTrace;
            }
            SetNextTryDateTime(communicationLog);
            if (context != null)
            {
                communicationLogRepository.Update(communicationLog);
                communicationLogRepository.SubmitChanges();
            }
        }

        private void SetNextTryDateTime(CommunicationLog communicationLog)
        {
            double delayInSeconds;
            switch (communicationLog.Retries)
            {
                case 1:
                case 2:
                    {
                        delayInSeconds = 1;
                        break;
                    }
                case 3:
                case 4:
                    {
                        delayInSeconds = 5;
                        break;
                    }
                case 5:
                    {
                        delayInSeconds = 1 * 60;
                        break;
                    }
                case 6:
                case 7:
                case 8:
                    {
                        delayInSeconds = 2 * 60;
                        break;
                    }
                case 9:
                    {
                        delayInSeconds = 5 * 60;
                        break;
                    }
                case 10:
                    {
                        delayInSeconds = 10 * 60;
                        break;
                    }
                default:
                    {
                        delayInSeconds = 20 * 60;
                        break;
                    }
            }

            if (delayInSeconds <= 59)
                HandleDelayInSeconds(communicationLog, delayInSeconds);
            else
                HandleDelayInMinutes(communicationLog, delayInSeconds);

            communicationLog.Logs += Environment.NewLine + "Retry #" + communicationLog.Retries + " Next Retry: " + communicationLog.NextTryDateTimeUTC.ToString();
        }

        private void HandleDelayInSeconds(CommunicationLog communicationLog, double delayInSeconds)
        {
            DateTime date = TenantServerConfigration.GetCurrentDateTime(communicationLog.Tenant);
            DateTime dateUtc = DateTime.UtcNow;
            communicationLog.NextTryDateTime = date.AddSeconds(delayInSeconds);
            communicationLog.NextTryDateTimeUTC = dateUtc.AddSeconds(delayInSeconds);
            queueservice.Delay(new TimeSpan(0, 0, 0, (int)delayInSeconds));
        }

        private void HandleDelayInMinutes(CommunicationLog communicationLog, double delayInSeconds)
        {
            DateTime date = TenantServerConfigration.GetCurrentDateTime(communicationLog.Tenant);
            DateTime dateUtc = DateTime.UtcNow;
            double delayInMinutes = delayInSeconds / 60;
            communicationLog.NextTryDateTime = date.AddMinutes(delayInMinutes);
            communicationLog.NextTryDateTimeUTC = dateUtc.AddMinutes(delayInMinutes);
            queueservice.Delay(new TimeSpan(0, 0, (int)delayInMinutes));
        }

        private void HandleRetryMechanism(QueueResponse queueResponse, int tenant, string communicationLogId)
        {
            if (queueResponse.RetryNumber > 11)
            {
                queueservice.Complete();
                AddLogToAzureStorage(tenant, queueResponse, communicationLogId);
            }
            else
            {
                if (queueResponse.RetryNumber < 3)
                {
                    queueservice.Delay(new TimeSpan(0, 0, 0, 1));
                }
                else if (queueResponse.RetryNumber >= 3 && queueResponse.RetryNumber <= 5)
                {
                    queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                }
                else if (queueResponse.RetryNumber > 5 && queueResponse.RetryNumber <= 10)
                {
                    queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                    AddLogToAzureStorage(tenant, queueResponse, communicationLogId);
                    Thread.Sleep(3000);
                }
                else if (queueResponse.RetryNumber == 11)
                {
                    queueservice.Delay(new TimeSpan(0, 0, 2, 0));
                    AddLogToAzureStorage(tenant, queueResponse, communicationLogId);
                    Thread.Sleep(10000);
                }
            }
        }

        private static void AddLogToAzureStorage(int tenant, QueueResponse queueResponse, string communicationLogId)
        {
            string log = "couldn't find communication log and the message is completed: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" 
                        + queueResponse.RetryNumber + ",at utc time:" + DateTime.UtcNow + ",at WebHookCommunicationLog worker role.";
            AzureLog.SaveLogsInStorage(log, "L", DateTime.UtcNow, "", "", 0, null, null, null);
        }

        public void ConnectClient()
        {
            try
            {
                InitializeQueueService();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
            }
        }

        public override bool OnStart()
        {
            if (!string.IsNullOrEmpty(ThreadId)) 
            {
                return true; 
            }
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "WebHookCommunicationWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            try
            {
                InitializeQueueService();
            }
            catch (Exception ex)
            {
                HandleExceptionOnStart(ex);
            }
            return base.OnStart();
        }

        private static void HandleExceptionOnStart(Exception ex)
        {
            string ip = "";
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }
            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "WebHookCommunicationWorkerRole Role", null, ip);
        }
    }

    public class WebHookCommunicationWorkerRoleWinService : Logitude.Server.Tools.WorkerEntryPointDoneLog
    {
        WebHookCommunicationWorkerRole WebHookCommunicationWorkerRole;
        public WebHookCommunicationWorkerRoleWinService()
        {
            WebHookCommunicationWorkerRole = new WebHookCommunicationWorkerRole();
        }

        public override void StartMe()
        {
            throw new NotImplementedException();
        }

        public override void WorkOnce()
        {
            WebHookCommunicationWorkerRole.OnStart();
            WebHookCommunicationWorkerRole.StartWork();
        }
    }
}