using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
    public class DocumentFilingBackupWR : WorkerEntryPoint
    {
        IQueueService queueservice;
        public override void Run()
        {


            while (IsRunning)
            {

                if (!General.IsUpdating())
                {
                    try
                    {
                        int tenant = 0;
                        queueservice = new DbQueueService();
                        queueservice.InitializeQueue("DocumentFillingBackupQueue", 0);

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
                                                + ",at utc time:" + DateTime.UtcNow + ",at DocumentFillingBackup worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                            Thread.Sleep(3000);
                                        }
                                        if (response.RetryNumber == 11)
                                        {

                                            queueservice.Delay(new TimeSpan(0, 0, 2, 0));
                                            AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                            + ",at utc time:" + DateTime.UtcNow + ",at DocumentFillingBackup worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                            Thread.Sleep(10000);

                                        }
                                    }
                                    else
                                    {
                                        queueservice.Complete();
                                        AzureLog.SaveLogsInStorage("couldn't find communication log and the message is completed: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                            + ",at utc time:" + DateTime.UtcNow + ",at DocumentFillingBackup worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DocumentFillingBackup worker role start", null, null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
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

                    cl.CommunicationStatusTypeCode = "D";
                    cl.DoneDate = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
                    cl.DoneDateUTC = DateTime.UtcNow;
                    cl.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
                    cl.LastStatusDateUTC = DateTime.UtcNow;
                    communicationLogRep.Update(cl);
                    communicationLogRep.SubmitChanges();
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
            DocumentsFilingBackupHelper.UploadDocumentToFTP(waitingCommLog.EntityId, waitingCommLog.Tenant);
            //int tenant = waitingCommLog.Tenant;
            //ICommonDataContext commoncontext = CommonDataContext.GetContext(waitingCommLog.Tenant);
            //DocumentRepository documentRepository = new DocumentRepository(commoncontext);
            //DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(commoncontext);
            //DocumentsFiling documentFilingPOCO = documentsFilingRepository.GetSingleDocumentsFiling(waitingCommLog.EntityId);
            //Document document = documentRepository.GetSingleDocument(documentFilingPOCO.Tenant, documentFilingPOCO.DocumentId);
            //Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
            //{
            //    FileName = document.Id,
            //    FolderName = document.Folder,
            //    Extension = document.Extension,
            //    Tenant = tenant,
            //    FileSize = document.FileSize,
            //};

            //Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            //byte[] filedata = storageservice.Read(fileInfo);

            //if (filedata != null)
            //{
            //    DocumentFilingBackupSettingQuery documentFilingBackupSettingQuery = new DocumentFilingBackupSettingQuery(tenant);
            //    DocumentFilingBackupSettingPM settingsPM = documentFilingBackupSettingQuery.GetSinglePM(tenant, tenant);
            //    FTPDetail ftpDetail = null;
            //    FTPDetailRepository ftpDetailRepository = new FTPDetailRepository(commoncontext);
            //    if (!string.IsNullOrEmpty(settingsPM.FTPDetailId))
            //        ftpDetail = ftpDetailRepository.GetSingleFTPDetail(settingsPM.FTPDetailId, tenant);

            //    if (ftpDetail != null)
            //    {
            //        string ftpHostIP = @"ftp://" + ftpDetail.Host;
            //        string ftpUserName = ftpDetail.UserName;
            //        string ftpPassword = ftpDetail.Password;
            //        string ftpFolderName = ftpDetail.Folder;

            //        string fileName = (!string.IsNullOrEmpty(document.FileName) ? document.FileName : document.Id) + "_" + documentFilingPOCO.Code + "." + document.Extension;
            //        if (!ftpDetail.UseSFTP)
            //        {
            //            FTPService ftpService = new FTPService(ftpHostIP, ftpUserName, ftpPassword);
            //            ftpService.Upload(fileName, ftpFolderName, filedata, true, true);
            //        }
            //        else
            //        {
            //            string p_status = "";
            //            string p_message = "";
            //            SFTPService sftpService = new SFTPService();
            //            sftpService.Logon(ftpDetail.Host, ftpUserName, ftpPassword, "22", ftpFolderName, out p_status, out p_message);
            //            if (p_status == "0")
            //            {
            //                if (p_status == "0")
            //                {
            //                    sftpService.Upload(fileName, filedata, true, true, out p_status, out p_message);
            //                }
            //                else
            //                    throw new Exception("SFTP upload file failed: " + p_message);
            //            }
            //            else
            //                throw new Exception("SFTP Login failed: " + p_message);
            //        }

            //        documentFilingPOCO.BackedupExternally = true;
            //        documentFilingPOCO.LastBackupDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            //        documentsFilingRepository.Update(documentFilingPOCO);
            //        documentsFilingRepository.SubmitChanges();
            //    }
            //    else
            //    {
            //        throw new Exception("ftp credentials is not definded!");
            //    }

            //}
            //else
            //{
            //    //throw new Exception("The file data was not found!");
            //}
        }

        #endregion


        public void ConnectClient()
        {
            try
            {

                queueservice = new DbQueueService();
                queueservice.InitializeQueue("DocumentFillingBackupQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
            }
        }
        public override bool OnStart()
        {

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DocumentFillingBackupWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("DocumentFillingBackupQueue", 0);

            }

            catch (Exception ex)
            {

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DocumentFillingBackup Role", null, ip);


            }
            return base.OnStart();
        }
    }
}