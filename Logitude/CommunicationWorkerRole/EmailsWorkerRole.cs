using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.IO;
using System.Net.Mail;
using System.Net;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;

using WebFreight.Web;

using System.Text.RegularExpressions;
using System.Text;

using System.Transactions;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.QueueService;
using System.Xml;
using System.Net.Http;
using Newtonsoft.Json;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.CToolWorkflows;
using WebFreight.Web.Helpers.CallBack;

namespace CommunicationWorkerRole
{
    public class EmailsWorkerRole : WorkerEntryPoint
    {
        private enum GateWay
        {
            Amazon = 0,
            Amital = 1,
        };
        private string communicationLogTypeCode;
        private string toPartner;
        private string queueName;
        private const int AttachmentsBytesMaximumSize = 20;
        private string callBackDetails = string.Empty;
        public EmailsWorkerRole(string communicationLogTypeCode, string toPartner)
        {
            this.communicationLogTypeCode = communicationLogTypeCode;
            this.toPartner = toPartner;
            if (communicationLogTypeCode == "E")
            {
                queueName = "EmailQueue";
            }
            else
            {
                switch (toPartner)
                {
                    case "CHAMP":
                        {
                            queueName = "champmessageoutqueue";
                            break;
                        }
                    case "GLSHK":
                        {
                            queueName = "glshkmessageoutqueue";
                            break;
                        }
                }
            }
        }
        //QueueDescription queueDescription;
        //QueueClient client;
        List<CommunicationLog> waitingcommlogs;

        public List<CommunicationLog> WaitingCommLogs
        {
            get
            {
                if (waitingcommlogs == null)
                {
                    waitingcommlogs = new List<CommunicationLog>();
                }
                return waitingcommlogs;
            }
            set { waitingcommlogs = value; }
        }
        public override void Run()
        {


            while (IsRunning)
            {
                //EventLog.WriteEntry("LogitudeBatchService", "inside EmailWR");
                if (!General.IsUpdating())
                {
                    try
                    {
                        int tenant = 0;
                        queueservice = new DbQueueService(queueName, 0);
                        //if (queueName == "EmailQueue")
                        //{
                        //    queueservice = new DbQueueService(queueName, 0);
                        //}
                        //else
                        //{
                        //    queueservice = QueueServiceManager.GetQueueService(queueName, 0);
                        //}
                        //using (TransactionScope scope = TransactionFactory.GetNewReadCommittedTransaction())
                        //{
                            var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                            LastActivity = DateTime.UtcNow;

                            if (response.MessageId != null)
                            {
                                string communicationLogId = response.MessageValues["CommunicationLogId"].ToString();
                                int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
                                callBackDetails = response.MessageValues.ContainsKey("CallBackDetails") ? response.MessageValues["CallBackDetails"] : "";
                                context = CommonDataContext.GetContext(tenant);
                                CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
                                CommunicationLog cl = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);

                                bool processEnebled = true;//IsCommunicationLogProcessEnabled(communicationLogId, tenant); since there is queueservice.delay(timespan).... ihab mohammad jalal
                                try
                                {
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
                                                var sendingEmailQuotaResult = EmailLimitationHelper.CheckEmailSendingQuotaForTenant(tenant);
                                                if (sendingEmailQuotaResult.IsQuotaExceeded)
                                                {
                                                    cl.CommunicationStatusTypeCode = "F";
                                                    cl.ExceptionMessage = sendingEmailQuotaResult.ExceptionMessage;
                                                    cl.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
                                                    communicationLogRep.Update(cl);
                                                    communicationLogRep.SubmitChanges();
                                                }
                                                else
                                                {
                                                    SendCommunicationLog(communicationLogId, tenant, cl, communicationLogRep, response);
                                                }
                                                queueservice.Complete();

                                                LogDoneItemInMemory();

                                            }
                                        }
                                        else
                                        {
                                            if (response.RetryNumber <= 4)
                                            {
                                                if (response.RetryNumber <= 1)
                                                {
                                                    queueservice.Delay(new TimeSpan(0, 0, 0, 40));
                                                }

                                                if (response.RetryNumber >= 2 && response.RetryNumber < 3)
                                                {
                                                    queueservice.Delay(new TimeSpan(0, 0, 1, 0));
                                                }

                                                //if (response.RetryNumber > 5 && response.RetryNumber <= 10)
                                                //{

                                                //    queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                                //    AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                                //        + ",at utc time:" + DateTime.UtcNow + ",at email worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                                //    Thread.Sleep(3000);
                                                //}
                                                if (response.RetryNumber == 3)
                                                {

                                                    queueservice.Delay(new TimeSpan(0, 0, 2, 0));
                                                    AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                                    + ",at utc time:" + DateTime.UtcNow + ",at email worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                                    //Thread.Sleep(10000);

                                                }
                                            }
                                            else
                                            {

                                                //cl.CommunicationStatusTypeCode = "F";
                                                //cl.ExceptionMessage = sendingEmailQuotaResult.ExceptionMessage;
                                                //cl.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
                                                //communicationLogRep.Update(cl);
                                                //communicationLogRep.SubmitChanges();

                                                queueservice.Complete();
                                                AzureLog.SaveLogsInStorage("couldn't find communication log and the message is completed: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                                    + ",at utc time:" + DateTime.UtcNow + ",at email worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                            }
                                        }

                                        // Send Kafka message to CTool
                                        if (FeatureToggleHelper.HasFeatureToggle("CTL", tenant))
                                        {
                                            EntityChangesMessageProducer.ProduceSendEmailMessage(cl);
                                        }
                                    }
                                }
                                catch (Exception insideEx)
                                {
                                    HandleEmailsExceptionRetries(response, insideEx);
                                }
                            }
                          //  scope.Complete();
                        //}
                    }
                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private void HandleEmailsExceptionRetries(QueueResponse response, Exception insideEx)
        {
            string errorMessage = insideEx.Message + Environment.NewLine;

            if (insideEx.InnerException != null)
            {

                errorMessage = errorMessage + " (" + (insideEx.InnerException.InnerException != null ? insideEx.InnerException.InnerException.Message : insideEx.InnerException.Message) + ")" + Environment.NewLine;

            }

            errorMessage = errorMessage + insideEx.StackTrace + Environment.NewLine;
            var msg = insideEx.Message + DateTime.Now;
            if (response.RetryNumber <= 1)
            {
                queueservice.Delay(new TimeSpan(0, 0, 0, 5));
            }

            if (response.RetryNumber > 1 && response.RetryNumber <= 3)
            {
                queueservice.Delay(new TimeSpan(0, 0, 0, 10));
            }
            if (response.RetryNumber >= 4)
            {
                queueservice.CompleteAsFailed();
            }
            ExceptionHandler.HandleException(insideEx, DateTime.Now, 0, null, "email worker role HandleEmailsExceptions", null, null);
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

        ICommonDataContext context; 
        private void SendCommunicationLog(string communicationLogId, int tenant, CommunicationLog cl, CommunicationLogRepository communicationLogRep, QueueResponse response)
        {


            try
            {
                if (response.RetryNumber < 30 && cl.Retries < 30)
                {
                    SendWaitingCommunicationLog(cl);  
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
                    cl.ExceptionMessage = StringHelper.TruncateLongString(cl.ExceptionMessage, 4000);

                    communicationLogRep.Update(cl);
                    communicationLogRep.SubmitChanges();
                }
                throw;

            }
        }

        public void SendWaitingCommunicationLog(CommunicationLog waitingCommLog)
        {
            CommunicationAttachmentRepository communicationAttachmentRep = new CommunicationAttachmentRepository(waitingCommLog.Tenant);

            List<CommunicationAttachment> attachmentsList = communicationAttachmentRep.GetCommunicationAttachmentsForCommLog(waitingCommLog.Id, waitingCommLog.Tenant).ToList();

            string xmlfile = "";
            if (waitingCommLog != null)
            {
                byte[] datainByte = null;
                string filename;
                if (waitingCommLog.Document != null)
                {
                    filename = waitingCommLog.DocumentId + "." + waitingCommLog.Document.Extension;

                    CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(waitingCommLog.Tenant);
                    var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, waitingCommLog.Document.Folder));

                    Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                    {
                        FileName = waitingCommLog.Document.Id,
                        FolderName = waitingCommLog.Document.Folder,
                        Extension = waitingCommLog.Document.Extension,
                        Tenant = waitingCommLog.Document.Tenant,
                        FileSize = waitingCommLog.Document.FileSize,
                    };
                    Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                    datainByte = storageservice.Read(fileInfo);


                    if (datainByte != null && waitingCommLog.CommunicationLogTypeCode == "T")
                    {
                        //blobfile.DownloadToStream(memstream);
                        Encoding encoding = Encoding.UTF8;
                        xmlfile = encoding.GetString(datainByte);

                    }

                    // using (MemoryStream memstream = new MemoryStream())
                    // {
                    //    switch (waitingCommLog.CommunicationLogTypeCode)
                    //    {
                    //        case "E":
                    //            {
                    //                //blobfile.DownloadToStream(memstream);
                    //                //datainByte = memstream.ToArray();
                    //                break;
                    //            }


                    //if (datainByte != null && waitingCommLog.CommunicationLogTypeCode == "T")
                    //            {
                    //                //blobfile.DownloadToStream(memstream);
                    //                Encoding encoding = Encoding.UTF8;
                    //                xmlfile = encoding.GetString(datainByte);
                    //                break;
                    //            }
                    //    }


                    //}

                }


                switch (waitingCommLog.CommunicationLogTypeCode)
                {
                    case "E":
                        {
                            if (datainByte != null)
                                SendHtmlDocumentByEmail(datainByte, attachmentsList, waitingCommLog);
                            break;
                        }
                    case "T":
                        {
                            if (!string.IsNullOrEmpty(xmlfile))
                            {
                                if (waitingCommLog.InOut == "O")
                                {
                                    string myTarget = "";
                                    if (!string.IsNullOrEmpty(waitingCommLog.To))
                                    {
                                        myTarget = waitingCommLog.To;
                                    }

                                    switch (myTarget.ToUpper())
                                    {
                                        case "GLSHK":
                                        case "CHAMP":
                                            {
                                                bool isUsingRestAPI = false;

                                                var iAppSettings = System.Configuration.ConfigurationManager.AppSettings;
                                                if (iAppSettings != null)
                                                {
                                                    if (iAppSettings["ChampRestAPITenants"] != null)
                                                    {
                                                        string iTenantsText = iAppSettings["ChampRestAPITenants"].ToString();
                                                        if (!string.IsNullOrEmpty(iTenantsText))
                                                        {
                                                            string[] iTenantsList = iTenantsText.Split(',');

                                                            foreach (string iTenantString in iTenantsList)
                                                            {
                                                                if (waitingCommLog.Tenant.ToString() == iTenantString.Trim())
                                                                {
                                                                    isUsingRestAPI = true;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (!isUsingRestAPI)
                                                    {
                                                        if (iAppSettings["ChampRestAPITenantKey"] != null)
                                                        {
                                                            int tenant = waitingCommLog.Tenant;
                                                            string ConfigurationString = iAppSettings["ChampRestAPITenantKey"].ToString();

                                                            int ConfigurationNumber = 0;
                                                            if (int.TryParse(ConfigurationString, out ConfigurationNumber))
                                                            {
                                                                int TenanLastNumber = int.Parse(tenant.ToString().Substring(tenant.ToString().Length - 1));

                                                                if (ConfigurationNumber >= TenanLastNumber)
                                                                {
                                                                    isUsingRestAPI = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (isUsingRestAPI)
                                                {
                                                    if (datainByte != null)
                                                    {
                                                        int index = 0;
                                                        while (index < datainByte.Length)
                                                        {
                                                            int d = datainByte[index];
                                                            if (d == 60)
                                                            {
                                                                datainByte = datainByte.Skip(index).ToArray();
                                                                break;
                                                            }

                                                            index++;
                                                        }

                                                        xmlfile = Encoding.ASCII.GetString(datainByte);
                                                    }

                                                    SendCommunicationLogToChampAPI(waitingCommLog, xmlfile);
                                                }

                                                else
                                                {
                                                    SendCommunicationLogToChamp(waitingCommLog, xmlfile);
                                                }

                                                break;
                                            }

                                        default:
                                            {
                                                throw new Exception("Sending to unknow!");
                                            }
                                    }
                                }
                            }

                            break;
                        }
                }





            }
        }



        private void SendHtmlDocumentByEmail(byte[] filedata, List<CommunicationAttachment> attachmentsList, CommunicationLog currentLog)
        {
            StringBuilder htmlTemplate = new StringBuilder();
            try
            {
                Encoding encoding = Encoding.UTF8;

                htmlTemplate.Append(encoding.GetString(filedata));
            }

            catch (Exception e)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
            }

            CommunicationLogRepository commLogrepository = new CommunicationLogRepository(context);

            if (!ValidateAttachmenstSize(attachmentsList))
            {
                SetFailCommunicationLog(currentLog, commLogrepository);
                return;
            }


            //attachements
            List<Attachment> attachements = new List<Attachment>();
            if (attachmentsList.Count != 0)
            {

                foreach (CommunicationAttachment ca in attachmentsList)
                {
                    string filename = ca.DocumentId;
                    filename += ".";
                    filename += ca.Document.Extension;

                    // Download file from Azure Storage
                    //CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(currentLog.Tenant);
                    //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filePath, ca.Document.Folder));
                    string filePath = "tenant" + ca.Tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), ca.Document.Folder);
                    byte[] dataByte = null;
                    if (ca.Document.HasFile)
                    {

                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                        BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = ca.DocumentId,
                            FolderName = ca.Document.Folder,
                            Extension = ca.Document.Extension,
                            Tenant = ca.Tenant,
                            FileSize = ca.Document.FileSize,

                        };

                        dataByte = storageservice.Read(fileInfo);
                    }
                    if (dataByte != null)
                    {
                        MemoryStream memstream = new MemoryStream(dataByte);


                        // memstream.Seek(0, SeekOrigin.Begin);

                        DocumentOutCopyRepository repository = new DocumentOutCopyRepository(context);
                        DocumentOutRepository documentOutRepository = new DocumentOutRepository(context);
                        DocumentTypeCopyRepository documentTypeCopyRepository = new DocumentTypeCopyRepository(context);
                        DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(context);
                        DocumentOutCopy docCopy = repository.GetSingleDocumentOutCopyByTenant(ca.DocumentId, ca.Tenant);

                        string name = ca.DocumentId;
                        string calculatedFileName = "";
                        if (ca.Document != null && !string.IsNullOrEmpty(ca.Document.CalculatedFileName))
                        {
                            calculatedFileName = ca.Document.CalculatedFileName.Replace(" ", "") + "." + ca.Document.Extension;
                        }
                        else if (docCopy != null)
                        {

                            DocumentOut documentOut = documentOutRepository.GetSingleDocumentOut(docCopy.DocumentOutId, ca.Tenant);
                            DocumentType documentType = documentTypeRepository.GetSingleDocumentTypes(documentOut.DocumentsFiling.DocumentTypeId, ca.Tenant);
                            DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopy(docCopy.DocumentTypeCopyId);

                            string DocumentTypeCopyNameWithDocumentTypeName = (documentType.Name != documentTypeCopy.Name ? documentType.Name + " - " + documentTypeCopy.Name : documentTypeCopy.Name);

                            name = DocumentTypeCopyNameWithDocumentTypeName;

                            string[] names = name.Split('-');
                            if (names.Count() > 1)
                            {
                                if (names[0].Trim() == names[1].Trim())
                                {
                                    name = docCopy.DocumentTypeCopy.Name;
                                }
                            }

                        }
                        else
                        {
                            DocumentsFilingRepository rep = new DocumentsFilingRepository(context);
                            DocumentsFiling docIn = rep.GetSingleDocumentsFiling(ca.DocumentId, ca.Tenant);
                            if (docIn != null)
                            {
                                name = docIn.DocumentType.Name;
                            }
                        }

                        Attachment attachemnt = new Attachment(memstream, filePath);
                        attachemnt.Name = !string.IsNullOrEmpty(calculatedFileName) ? calculatedFileName : (string.IsNullOrEmpty(ca.Document.FileName) ? name.Replace(" ", "") : ca.Document.FileName.Replace(" ", "")) + "." + ca.Document.Extension;
                        attachements.Add(attachemnt);



                    }
                }
            }

            EmailParameters parameters = new EmailParameters()
            {
                From = string.IsNullOrEmpty(currentLog.From) ? currentLog.CreatedByUser.Contact.Email : currentLog.From,
                To = currentLog.To,
                Cc = currentLog.CC,
                Bcc = currentLog.BCC,
                Attachments = attachements,
                EmailView = System.Net.Mime.MediaTypeNames.Text.Html,
                IsBodyHtml = true,
                Tenant = currentLog.Tenant,
                Body = htmlTemplate.ToString(),
                Subject = currentLog.Subject,
                Retries = currentLog.Retries,
            };



            if (!string.IsNullOrEmpty(currentLog.ReplyToList) && !string.IsNullOrWhiteSpace(currentLog.ReplyToList))
            {
                string[] replyToList = currentLog.ReplyToList.Split(';');
                foreach (string replyto in replyToList)
                {
                    parameters.ReplyToList.Add(replyto);
                }
            }


            parameters.SentByUser = GetSentByUserName(currentLog, parameters);

            if (parameters.ReplyToList != null && parameters.ReplyToList.Count > 0)
            {
                if (!string.IsNullOrEmpty(parameters.From) && parameters.From.Contains('@') && parameters.ReplyToList[0].Contains('@'))
                {
                    parameters.From = parameters.From.Split('@')[0] + "@" + parameters.ReplyToList[0].Split('@')[1];
                }
            }

            parameters.CommunicationLogId = currentLog.Id;
            parameters.CommunicationLogCreateDate = currentLog.CreateDate;
            parameters.Tenant = currentLog.Tenant;
            EmailingHelper.SendEmail(parameters);

            // After sent successfully, change status to done 

            EmailProvider provider = EmailingHelper.GetEmailProvider(parameters.Tenant, parameters.Retries, parameters.IsProviderNumberSpecified, parameters.ProviderNumber);

            if (provider != null && provider.SupportsEmailDelivery)
            {
                currentLog.CommunicationStatusTypeCode = "C";
            }
            else
            {
                currentLog.CommunicationStatusTypeCode = "D";
            }
            currentLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(currentLog.Tenant);
            currentLog.DoneDateUTC = DateTime.UtcNow;
            currentLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(currentLog.Tenant);
            currentLog.LastStatusDateUTC = DateTime.UtcNow;
            commLogrepository.Update(currentLog);
            commLogrepository.SubmitChanges();

            if (!string.IsNullOrEmpty(callBackDetails))
            {
                CallBackService.Notifiy(callBackDetails, null);
            }
        }

        private string GetSentByUserName(CommunicationLog currentLog, EmailParameters emailParameters)
        {
            string sentByUser = (currentLog?.CreatedByUser?.Contact?.EnglishName) ?? "";
            if (!string.IsNullOrEmpty(currentLog.From) && currentLog.From.Contains("no-reply")) sentByUser = "";


            if (string.IsNullOrEmpty(emailParameters.From) || !emailParameters.From.Contains('@')) return sentByUser;
            ContactRepository contactRepository = new ContactRepository(currentLog.Tenant);
            string contactMe = contactRepository.GetConactNameByemail(emailParameters.From, currentLog.Tenant);
            //if (contactMe == null && currentLog.From == "info@logitudeworld.com" && SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Simplog)) 
            //    contactMe = contactRepository.GetConactNameByemail(emailParameters.From, LogitudeSettings.LogitudeCRMTenantNumber);

            return string.IsNullOrEmpty(contactMe) ? sentByUser : contactMe;
        }

        private bool ValidateAttachmenstSize(List<CommunicationAttachment> attachmentsList)
        {
            double? attachementsSize = GetBytesAttachmentSize(attachmentsList); 
            return (attachementsSize < AttachmentsBytesMaximumSize); 
        } 

        private double? GetBytesAttachmentSize(List<CommunicationAttachment> communicationAttachments)
        {
            double? attachementsSize = 0;

            foreach (CommunicationAttachment communicationAttachment in communicationAttachments)
            {
                if (communicationAttachment.Document.FileSize != null)
                    attachementsSize = attachementsSize + communicationAttachment.Document.FileSize;
            }

            attachementsSize = GetByteSize(attachementsSize); 
            return attachementsSize;
        }

        private static void SetFailCommunicationLog(CommunicationLog communicationLog, CommunicationLogRepository communicationLogRepository)
        {
            communicationLog.CommunicationStatusTypeCode = "F";
            communicationLog.ExceptionMessage = "Maximum size of files attachments exceeded 20 MB";
            communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(communicationLog.Tenant);
            communicationLogRepository.Update(communicationLog);
            communicationLogRepository.SubmitChanges();
        }  

        private double GetByteSize(double? size)
        {

            int byteValue = 1024;
            double fileSize = 0;

            if (size != null)
            {
                fileSize = (double)(size / (byteValue * byteValue));
            }
            return fileSize;
        }


        //        public void SendEmail(string from, string to, string cc, string bcc, string subject, string body)
        //        {

        //            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
        //@"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
        //@".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        //            SmtpClient myClient;
        //            MailMessage myMessage;

        //            myClient = new SmtpClient("retail.smtp.com", 2525);
        //            myClient.Credentials = new NetworkCredential("ihab@simplogworld.com", "Saas256");
        //            myMessage = new MailMessage();
        //            Encoding encoding = Encoding.UTF8;
        //            myMessage.BodyEncoding = encoding;
        //            myMessage.IsBodyHtml = true;
        //            myMessage.Body = body;
        //            myMessage.Subject = subject;
        //            myMessage.BodyEncoding = System.Text.Encoding.UTF8;
        //            myMessage.From = new MailAddress(from);


        //            string[] emailList = to.Split(';');

        //            for (int i = 0; i < emailList.Length; i++)
        //            {
        //                if (!string.IsNullOrEmpty(emailList[i]))
        //                {
        //                    emailList[i] = emailList[i].Trim();
        //                    Regex re = new Regex(strRegex);
        //                    if (re.IsMatch(emailList[i]))
        //                    {
        //                        MailAddress mailaddress = new MailAddress(emailList[i]);
        //                        myMessage.To.Add(mailaddress);
        //                    }
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(cc))
        //            {
        //                string[] ccList = cc.Split(';');
        //                string ccCorectList = "";

        //                for (int i = 0; i < ccList.Length; i++)
        //                {
        //                    if (!string.IsNullOrEmpty(ccList[i]))
        //                    {
        //                        Regex re = new Regex(strRegex);
        //                        if (re.IsMatch(ccList[i]))
        //                        {
        //                            MailAddress mailaddress = new MailAddress(ccList[i]);
        //                            myMessage.CC.Add(mailaddress);
        //                        }

        //                    }
        //                }

        //            }



        //            //Bcc
        //            if (!string.IsNullOrEmpty(bcc))
        //            {
        //                string[] bccList = bcc.Split(';');//to be fixed to Bcc when field is ready!
        //                for (int i = 0; i < bccList.Length; i++)
        //                {
        //                    if (!string.IsNullOrEmpty(bccList[i]))
        //                    {
        //                        Regex re = new Regex(strRegex);
        //                        if (re.IsMatch(bccList[i]))
        //                        {
        //                            MailAddress mailaddress = new MailAddress(bccList[i]);
        //                            myMessage.Bcc.Add(mailaddress);
        //                            //BCC_CorectList += BccList[i] + ",";
        //                        }

        //                    }
        //                }

        //            }

        //            if (myMessage.To.Count != 0)
        //            {
        //                myClient.Send(myMessage);
        //            }



        //        }

        public override bool OnStart()
        {

            ConnectClient(); // mohammad to try reconnect in case of disconnected client. 23-7-15
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            //ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();


            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "EmailOut-" + queueName;
            DoneItemsInRange = new Dictionary<DateTime, int>();

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }
        IQueueService queueservice;
        public void ConnectClient()
        {
            try
            {

                //string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment(queueName);

                //if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
                //{
                //    queueDescription = new QueueDescription(emailQueueName);
                //    queueDescription.MaxSizeInMegabytes = 5120;
                //    queueDescription.EnableDeadLetteringOnMessageExpiration = false;
                //    queueDescription.MaxDeliveryCount = 1000;
                //    StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
                //}

                //client = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);


                queueservice = new DbQueueService(queueName, 0);//QueueServiceManager.GetQueueService(queueName, 0);
                //queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", log.Id }, { "Tenant", tenant.ToString() } });
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
            }
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {

            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        private void SendCommunicationLogToChamp(CommunicationLog waitingCommLog, string xmlfile)
        {
            string error = "";
            GateWay gateWay = GetGateWayForCommLog(waitingCommLog);
            string url = "";
            string env = "";
            if (gateWay == GateWay.Amazon)
            {
                url = LogitudeSettings.ChampURL;
                env = LogitudeSettings.ChampEnv;

                if (waitingCommLog.To == "GLSHK")
                {
                    url = LogitudeSettings.GLSHKURL;
                    env = LogitudeSettings.GLSHKEnv;
                }
            }

            else if (gateWay == GateWay.Amital)
            {
                url = "http://192.116.221.66:8732";
                env = LogitudeSettings.ChampEnv;
                if (waitingCommLog.To == "GLSHK")
                {
                    env = LogitudeSettings.GLSHKEnv;
                }
            }

            url = url + "/Design_Time_Addresses/ChampGateway/QueueGateway";

            ChampProxy.QueueGatewayClient queueGateway = new ChampProxy.QueueGatewayClient();
            queueGateway.Endpoint.Address = new System.ServiceModel.EndpointAddress(url);

            if (xmlfile.Contains("<"))
            {
                int index = xmlfile.IndexOf('<');
                if (index > 0)
                {
                    xmlfile = xmlfile.Substring(index);
                }
            }

            bool succeeded = queueGateway.Put(out error, xmlfile, env);

            if (succeeded)
            {
                //string logs = "Amazon";
                //if (gateWay == GateWay.Amital)
                //{
                //    logs = "Amital";
                //}

                CommunicationLogRepository commLogrepository = new CommunicationLogRepository(context);
                //waitingCommLog.Logs = logs;
                waitingCommLog.CommunicationStatusTypeCode = "D";
                waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                waitingCommLog.DoneDateUTC = DateTime.UtcNow;
                waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
                commLogrepository.Update(waitingCommLog);
                commLogrepository.SubmitChanges();
            }
        }

        private GateWay GetGateWayForCommLog(CommunicationLog waitingCommLog)
        {
            GateWay gateWay = GateWay.Amazon;

            if (LogitudeSettings.ChampEnv == "TEST")
            {
                gateWay = GateWay.Amazon;
            }

            else
            {
                int intId;
                string[] commLogIdSplitted = waitingCommLog.Id.Split('-');

                if (int.TryParse(commLogIdSplitted[1], out intId))
                {
                    int modl = intId % 10;

                    if (waitingCommLog.Retries < 3)
                    {
                        if (modl <= 7)
                        {
                            gateWay = GateWay.Amazon;
                        }

                        else
                        {
                            gateWay = GateWay.Amital;
                        }
                    }

                    else
                    {
                        if (modl <= 7)
                        {
                            gateWay = GateWay.Amital;
                        }

                        else
                        {
                            gateWay = GateWay.Amazon;
                        }
                    }
                }
            }

            return gateWay;
        }

        private void SendCommunicationLogToChampAPI(CommunicationLog waitingCommLog, string xmlfileText)
        {
            string iSendingURL = null;
            string iSendingPassword = null;

            if (LogitudeSettings.ChampEnv == "PROD")
            {
                iSendingURL = LogitudeSettings.ChampProdAPIURL;
                iSendingPassword = LogitudeSettings.ChampProdAPIPassword;
            }
            else
            {
                iSendingURL = LogitudeSettings.ChampTestAPIURL;
                iSendingPassword = LogitudeSettings.ChampTestAPIPassword;
            }

            bool isTestingCode = false;

            if (waitingCommLog.CreatedByUserId == "1-77675")
            {
                if (waitingCommLog.To != null)
                {
                    if (waitingCommLog.To.ToUpper() == "CHAMP")
                    {
                        isTestingCode = true;
                    }
                }
            }

            if (isTestingCode)
            {
                string iSearchFields = waitingCommLog.SearchFields;

                if (string.IsNullOrEmpty(iSearchFields))
                {
                    iSearchFields = "inside SendCommunicationLogToChampAPI";
                }

                else
                {
                    if (!iSearchFields.Contains("inside SendCommunicationLogToChampAPI"))
                    {
                        iSearchFields += ",inside SendCommunicationLogToChampAPI";
                    }
                }

                if (waitingCommLog.SearchFields != iSearchFields)
                {
                    waitingCommLog.SearchFields = iSearchFields;

                    CommunicationLogRepository commLogrepository = new CommunicationLogRepository(context);
                    commLogrepository.Update(waitingCommLog);
                    commLogrepository.SubmitChanges();
                }
            }

            // https://stackoverflow.com/questions/25352462/how-to-send-xml-content-with-httpclient-postasync
            //string iSendingURL = "https://community.champ.aero:8444/logitude/test/NO_WAIT";

            StringContent content = new StringContent(xmlfileText, Encoding.ASCII, "application/xml");
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("password", "logitudett");
                client.DefaultRequestHeaders.Add("password", iSendingPassword);

                //var iResponse = client.PostAsync(iSendingURL, content);
                //System.Threading.Tasks.Task iResponse = client.PostAsync(iSendingURL, content);
                //System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> iResponse = client.PostAsync(iSendingURL, content);

                var iResponse = client.PostAsync(iSendingURL, content);
                iResponse.Wait();
                if (iResponse != null)
                {
                    if (iResponse.Result.StatusCode == HttpStatusCode.OK)
                    {
                        waitingCommLog.CommunicationStatusTypeCode = "D";
                        waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                        waitingCommLog.DoneDateUTC = DateTime.UtcNow;
                        waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                        waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;

                        CommunicationLogRepository commLogrepository = new CommunicationLogRepository(context);
                        commLogrepository.Update(waitingCommLog);
                        commLogrepository.SubmitChanges();
                    }

                    else
                    {
                        waitingCommLog.CommunicationStatusTypeCode = "F";
                        waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                        waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
                        waitingCommLog.ExceptionMessage = iResponse.Result.StatusCode.ToString();

                        CommunicationLogRepository commLogrepository = new CommunicationLogRepository(context);
                        commLogrepository.Update(waitingCommLog);
                        commLogrepository.SubmitChanges();
                    }
                }

                else
                {
                    waitingCommLog.CommunicationStatusTypeCode = "F";
                    waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                    waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
                    waitingCommLog.ExceptionMessage = "No Response";

                    CommunicationLogRepository commLogrepository = new CommunicationLogRepository(context);
                    commLogrepository.Update(waitingCommLog);
                    commLogrepository.SubmitChanges();
                }
            }
        }
    }
}
