using Logitude.Server.Tools;
using Logitude.Server.Tools.FTP;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
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
using System.Threading.Tasks;
using System.Web;
using WebFreight.Web.WebServices;

namespace CommunicationWorkerRole
{
    public class FTPCommunicationWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
        public override void Run()
        {


            while (IsRunning)
            {
                WorkOnce1();
            }
        }
        
        public void WorkOnce1()
        {
            if (!General.IsUpdating())
            {
                try 
                {
                    int tenant = 0;
                    int d = 0;
                    queueservice = new DbQueueService();
                    queueservice.InitializeQueue("FTPCommunicationLogQueue", 0);

                    var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                    LastActivity = DateTime.UtcNow;

                    if (response.MessageId != null)
                    {

                        string communicationLogId = response.MessageValues["CommunicationLogId"].ToString();
                        int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
                        context = CommonDataContext.GetContext(tenant);
                        CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
                        CommunicationLog cl = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);

                        bool processEnebled = true;

                        if (processEnebled)
                        {
                            if (cl != null)
                            {
                                if (cl.CommunicationStatusTypeCode == "D")
                                {
                                    queueservice.Complete();
                                }
                                else
                                {
                                    SendCommunicationLog(communicationLogId, tenant, cl, communicationLogRep);
                                    queueservice.Complete();

                                    LogDoneItemInMemory();

                                }
                            }
                            else
                            {
                                if (response.RetryNumber <= 11)
                                {
                                    if (response.RetryNumber < 3)
                                    {
                                        queueservice.Delay(new TimeSpan(0, 0, 0, 1));
                                    }

                                    if (response.RetryNumber >= 3 && response.RetryNumber <= 5)
                                    {
                                        queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                    }

                                    if (response.RetryNumber > 5 && response.RetryNumber <= 10)
                                    {

                                        queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                        AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                            + ",at utc time:" + DateTime.UtcNow + ",at FTPCommunicationLogQueue worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                        Thread.Sleep(3000);
                                    }
                                    if (response.RetryNumber == 11)
                                    {

                                        queueservice.Delay(new TimeSpan(0, 0, 2, 0));
                                        AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                        + ",at utc time:" + DateTime.UtcNow + ",at FTPCommunicationLog worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                        Thread.Sleep(10000);

                                    }
                                }
                                else
                                {
                                    queueservice.Complete();
                                    AzureLog.SaveLogsInStorage("couldn't find communication log and the message is completed: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                        + ",at utc time:" + DateTime.UtcNow + ",at FTPCommunicationLog worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ConnectClient();
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "FTPCommunicationLog worker role start", null, null);
                    Thread.Sleep(10000);
                }

            }
            else
            {
                Thread.Sleep(60000);
            }
        }

        public bool IsCommunicationLogProcessEnabled(string communicationLogId, int tenant)
        {
            bool isEnabled = true;
            context = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
            CommunicationLog cl = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);


            DateTime requestDate = DateTime.UtcNow;
            if (requestDate < cl.NextTryDateTimeUTC)
            {
                isEnabled = false;

            }

            return isEnabled;
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
                        queueservice.Delay(new TimeSpan(0, 0, 0, 1));

                        break;
                    }
                case 3:
                case 4:
                    {
                        cl.NextTryDateTime = date.AddSeconds(5);
                        cl.NextTryDateTimeUTC = dateUtc.AddSeconds(5);
                        queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                        break;
                    }
                case 5:
                    {
                        cl.NextTryDateTime = date.AddMinutes(1);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(1);
                        queueservice.Delay(new TimeSpan(0, 0, 1));
                        break;
                    }
                case 6:
                case 7:
                case 8:
                    {
                        cl.NextTryDateTime = date.AddMinutes(2);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(2);
                        queueservice.Delay(new TimeSpan(0, 0, 2));
                        break;
                    }
                case 9:
                    {
                        cl.NextTryDateTime = date.AddMinutes(5);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(5);
                        queueservice.Delay(new TimeSpan(0, 0, 5));
                        break;
                    }
                case 10:
                    {
                        cl.NextTryDateTime = date.AddMinutes(10);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(10);
                        queueservice.Delay(new TimeSpan(0, 0, 10));
                        break;
                    }
                default:
                    {
                        cl.NextTryDateTime = date.AddMinutes(20);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(20);
                        queueservice.Delay(new TimeSpan(0, 0, 20));
                        break;
                    }
            }
            cl.Logs += Environment.NewLine + "Retry #" + cl.Retries + " Next Retry: " + cl.NextTryDateTimeUTC.ToString();
        }

        #region SendCommunicationLog

        ICommonDataContext context;
        private void SendCommunicationLog(string communicationLogId, int tenant, CommunicationLog cl, CommunicationLogRepository communicationLogRep)
        {


            try
            {
                if (cl.Retries < 5)
                {
                    SendWaitingCommunicationLog(cl, communicationLogRep);
                }

                else
                {
                    if (context != null)
                    {
                        cl.CommunicationStatusTypeCode = "F";
                        cl.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
                        communicationLogRep.Update(cl);
                        communicationLogRep.SubmitChanges();
                    }

                }
            }
			catch (FTPServiceException exc)
			{
				cl.Retries++;
				cl.ExceptionMessage = exc.Message;
				SetNextTryDateTime(cl);
				if (context != null)
				{
					communicationLogRep.Update(cl);
					communicationLogRep.SubmitChanges();
				}

				throw;
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
                if (context != null)
                {
                    communicationLogRep.Update(cl);
                    communicationLogRep.SubmitChanges();
                }
                throw;

            }
        }

        private void SendWaitingCommunicationLog(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep)
        {
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

            if (filedata != null)
            {
                if (!string.IsNullOrEmpty(waitingCommLog.LogSettings))
                {
                    Logitude.XSD.Artemus.CommunicationLogSettings settingsData = JsonConvert.DeserializeObject<Logitude.XSD.Artemus.CommunicationLogSettings>(waitingCommLog.LogSettings);
                    if (settingsData != null)
                    {
                        string ftpHostIP = @"ftp://" + settingsData.host;
                        string ftpUserName = settingsData.username;
                        string ftpPassword = settingsData.password;
                        string ftpFolderName = settingsData.folder;
                        string fileName = (!string.IsNullOrEmpty(settingsData.filename) ? settingsData.filename : document.Id) + "." + document.Extension;
                        string p_message = "";
                        if (!settingsData.UseSFTP)
                        {

                            ////ftp://192.116.221.106/temp1
                            //string hostIP = @"ftp://" + settingsData.host;

                            FTPService ftpService = new FTPService(ftpHostIP, settingsData.username, settingsData.password);

                            ftpService.Upload(fileName, settingsData.folder, filedata, out p_message);
                            waitingCommLog.Logs += Environment.NewLine + DateTime.Now.ToString() + " : " + p_message;
                        }
                        else
                        {
                            ftpHostIP = settingsData.host;
                            string p_status = "";

                            SFTPService sftpService = new SFTPService();
                            sftpService.Logon(ftpHostIP, ftpUserName, ftpPassword, "22", ftpFolderName, out p_status, out p_message);
                            waitingCommLog.Logs += p_message;
                            if (p_status == "0")
                            {

                                sftpService.Upload(fileName, filedata, true, true, out p_status, out p_message);

                                if (p_status == "-1")
                                    throw new FTPServiceException("SFTP upload file failed: " + p_message);
                            }
                            else
                                throw new FTPServiceException("SFTP Login failed: " + p_message);

                            waitingCommLog.Logs += p_message;
                        }
                    }
                }

                waitingCommLog.CommunicationStatusTypeCode = "D";
                waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                waitingCommLog.DoneDateUTC = DateTime.UtcNow;
                waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
                communicationLogRep.Update(waitingCommLog);
                communicationLogRep.SubmitChanges();

            }
            else
            {
                throw new Exception("The file data was not found!");
            }
        }

        #endregion


        public void ConnectClient()
        {
            try
            {


                queueservice = new DbQueueService();
                queueservice.InitializeQueue("FTPCommunicationLogQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
            }
        }
        public override bool OnStart()
        {
            if (!string.IsNullOrEmpty(ThreadId)) return true;
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "FTPCommunicationWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("FTPCommunicationLogQueue", 0);

            }

            catch (Exception ex)
            {

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "FTPCommunicationWorkerRole Role", null, ip);


            }
            return base.OnStart();
        }
    }



    /*
Insert into BATCHSERVICESDEFINITIONS (CODE,CLASSNAME) values ('FTPCommunicationWorkerRoleWinService','FTPCommunicationWorkerRoleWinService');
Insert into BATCHSERVICESDEFINITIONMODS (CODE,INACTIVE,NUMBEROFTHREADS) values ('FTPCommunicationWorkerRoleWinService',0,1);

     */
    public class FTPCommunicationWorkerRoleWinService : Logitude.Server.Tools.WorkerEntryPointDoneLog
    {
        FTPCommunicationWorkerRole _FTPCommunicationWorkerRole;
        public FTPCommunicationWorkerRoleWinService()
        {
            _FTPCommunicationWorkerRole = new FTPCommunicationWorkerRole();
        }
        public override void StartMe()
        {
            throw new NotImplementedException();
        }

        public override void WorkOnce()
        {
            _FTPCommunicationWorkerRole.OnStart();
            _FTPCommunicationWorkerRole.WorkOnce1();
        }
    }
}