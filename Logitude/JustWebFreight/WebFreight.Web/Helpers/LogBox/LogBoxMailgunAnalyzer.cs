using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WebFreight.Web.Helpers.TicketAnalyzer;

namespace WebFreight.Web.Helpers.LogBox
{
    public partial class LogBoxMailgunAnalyzer
    {
        private AnalyzeQueue myAnalyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        CommunicationLogRepository myCommunicationLogRepository;
        EmailUpload EmailDetails;

        int Tenant;

        public LogBoxMailgunAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {
            if (analyzeQueue != null)
            {
                this.Tenant = analyzeQueue.Tenant;
                this.myAnalyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
                this.myCommunicationLogRepository = new CommunicationLogRepository(this.Tenant);
            }
        }
        public void Run()
        {
            if (myAnalyzeQueue != null)
            {
                this.Deserialize();
            }
        }
        private void Deserialize()
        {
            try
            {
                MemoryStream memorystream = new MemoryStream(myAnalyzeQueue.MessageBody);
                XmlSerializer serializer = new XmlSerializer(typeof(EmailUpload));
                EmailDetails = (EmailUpload)serializer.Deserialize(memorystream);
            }
            catch (Exception ex)
            {
                myAnalyzeQueue.Status = "F";
                myAnalyzeQueue.ErrorMessage = "LogBox Mailgun Analyzer failed: " + ex.Message;
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                return;
            }
            if (EmailDetails != null)
            {
                this.AnalyzeData();
            }
        }

        private void AnalyzeData()
        {
            try
            {
                this.ConnectAnalyzeQueue();
            }
            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
            }
        }
        private void ConnectAnalyzeQueue()
        {
            try
            {
                if (!myAnalyzeQueue.ConnectedToTenant)
                {
                    myAnalyzeQueue.ConnectedToTenant = true;
                    analyzeQueueRepository.Update(myAnalyzeQueue);
                    analyzeQueueRepository.SubmitChanges();
                }

                if (!myAnalyzeQueue.ConnectedToEntity)
                {
                    myAnalyzeQueue.ConnectedToEntity = true;
                    analyzeQueueRepository.Update(myAnalyzeQueue);
                    analyzeQueueRepository.SubmitChanges();
                }

                AttatchmetnsProcessing();

                myAnalyzeQueue.Status = "D";
                myAnalyzeQueue.ErrorMessage = null;
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
            }

            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
            }
        }

        private void OnCatchAnalyzingError(Exception ex)
        {
            myAnalyzeQueue.ErrorMessage = ex.Message + (ex.InnerException != null ? Environment.NewLine + "InnerException: " + ex.InnerException.Message : "");
            myAnalyzeQueue.StackTrace = (ex.StackTrace != null ? Environment.NewLine + "Stack Trace: " + ex.StackTrace : "");
            myAnalyzeQueue.ErrorMessage = myAnalyzeQueue.ErrorMessage.Length > 7950 ? myAnalyzeQueue.ErrorMessage.Substring(0, 7950) : myAnalyzeQueue.ErrorMessage;
            myAnalyzeQueue.StackTrace = myAnalyzeQueue.StackTrace.Length > 7950 ? myAnalyzeQueue.StackTrace.Substring(0, 7950) : myAnalyzeQueue.StackTrace;

            if (ex.Message.StartsWith("--"))
            {
                myAnalyzeQueue.Status = "F";
            }

            else
            {
                myAnalyzeQueue.Retries++;

                if (myAnalyzeQueue.Retries >= 5)
                {
                    myAnalyzeQueue.Status = "F";
                }
            }

            if (myAnalyzeQueue.Status == "F")
            {
                if (myAnalyzeQueue.ConnectedToTenant && myAnalyzeQueue.CommunicationLogId != null)
                {
                    CommunicationLog commLog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, Tenant);
                    if (commLog != null)
                    {
                        commLog.CommunicationStatusTypeCode = "F";
                        commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                        commLog.LastStatusDateUTC = DateTime.UtcNow;
                        commLog.ExceptionMessage = myAnalyzeQueue.ErrorMessage;

                        if (myAnalyzeQueue.StackTrace != null)
                        {
                            commLog.ExceptionMessage = commLog.ExceptionMessage + Environment.NewLine + "Stack Trace: " + myAnalyzeQueue.StackTrace;
                        }

                        myCommunicationLogRepository.Update(commLog);
                        myCommunicationLogRepository.SubmitChanges();
                    }
                }
            }

            myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
            analyzeQueueRepository.Update(myAnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }

        private void AttatchmetnsProcessing()
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(Tenant);
            FilingInboxRepository myFilingInboxRepository = new FilingInboxRepository(commonContext);
            FilingInboxAttachmentRepository myFilingInboxAttachRepository = new FilingInboxAttachmentRepository(commonContext);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);

            string updatedByUserId = null;
            User user = this.GetUser();
            if (user != null)
            {
                updatedByUserId = user.Id;
            }
            else
            {
                ContactRepository contactRepository = new ContactRepository(Tenant);
                Contact contact = contactRepository.GetContactByUserTypeAndTenant("S", Tenant);
                updatedByUserId = contact.Id;
            }

            int len = !string.IsNullOrEmpty(EmailDetails.BodyHtml) ? (int)EmailDetails.BodyHtml.Length : 0;
            Byte[] mybytearray = new Byte[len];
            byte[] buffer = Encoding.UTF8.GetBytes(EmailDetails.BodyHtml);
            Stream stream = new MemoryStream(buffer);
            stream.Read(mybytearray, 0, len);

            Document documentBody = new Document()
            {
                Id = IdCounter.GetNumber("Document", Tenant),
                CreateDate = DateTime.Now,
                Extension = "html",
                FileSize = mybytearray.Length,
                Tenant = Convert.ToInt32(Tenant),
                HasFile = true,
                Folder = "filinginbox",
            };

            documentRepository.Add(documentBody);
            // Blob
            Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = documentBody.Id,
                FolderName = documentBody.Folder,
                Extension = documentBody.Extension,
                Tenant = Tenant,
                FileSize = mybytearray.Length,
                IsEncrypted = true,
            };

            Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(mybytearray.ToArray(), fileInfo);

            var mySearchFields = ""; 

            FilingInbox myFilingInbox = new FilingInbox()
            {
                Id = IdCounter.GetNumber("FilingInbox", Tenant).ToString(),
                Tenant = Tenant,
                Sender = updatedByUserId,
                Subject = EmailDetails.Subject,
                BodyDocumentId = documentBody.Id,
                IsDeleted = false,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                UpdatedByUserId = updatedByUserId,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
            };

            if (!string.IsNullOrEmpty(EmailDetails.Subject))
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, EmailDetails.Subject);
            }
            if (EmailDetails.AttachmentsFiles != null && EmailDetails.AttachmentsFiles.Count() > 0)
            {
                foreach (var item in EmailDetails.AttachmentsFiles)
                {
                    len = (int)item.ContentLength;
                    mybytearray = new Byte[len];
                    buffer = item.InputStream;
                    stream = new MemoryStream(buffer);
                    stream.Read(mybytearray, 0, len);

                    string[] fileparams = !string.IsNullOrEmpty(item.FileName) ? item.FileName.Split('.') : null;
                    var extention = fileparams != null ? fileparams[fileparams.Length - 1] : "";

                    Document document = new Document()
                    {
                        Id = IdCounter.GetNumber("Document", Tenant),
                        CreateDate = DateTime.Now,
                        Extension = extention,
                        FileSize = Convert.ToInt32(mybytearray.Length),
                        Tenant = Convert.ToInt32(Tenant),
                        HasFile = true,
                        Folder = "filinginbox",
                    };
                    documentRepository.Add(document);

                    // Blob
                    fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = Tenant,
                        FileSize = mybytearray.Length,
                        IsEncrypted = true,
                    };

                    storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                    storageservice.Write(mybytearray.ToArray(), fileInfo);

                    FilingInboxAttachment myFilingInboxAttachment = new FilingInboxAttachment()
                    {
                        Id = IdCounter.GetNumber("FilingInboxAttachment", Tenant).ToString(),
                        Tenant = Tenant,
                        FileName = item.FileName,
                        DocumentId = document.Id,
                        FilingInboxId = myFilingInbox.Id,
                    };
                    if (!string.IsNullOrEmpty(item.FileName))
                    {
                        MethodHelper.AddToSearchFields(ref mySearchFields, item.FileName);
                    }
                    myFilingInboxAttachRepository.Add(myFilingInboxAttachment);
                }
            }

            FilingInboxAttachment myFilingInboxAttachment_Body = new FilingInboxAttachment()
            {
                Id = IdCounter.GetNumber("FilingInboxAttachment", Tenant).ToString(),
                Tenant = Tenant,
                FileName = "Mail Body",
                DocumentId = documentBody.Id,
                FilingInboxId = myFilingInbox.Id,
            };
            if (!string.IsNullOrEmpty(documentBody.FileName))
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, documentBody.FileName);
            }
            myFilingInboxAttachRepository.Add(myFilingInboxAttachment_Body);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            myFilingInbox.SearchFields = mySearchFields;
            myFilingInboxRepository.Add(myFilingInbox);
            commonContext.SaveChanges();
        }

        private User GetUser()
        {
            InboundEmailGeneralHelperMethods helper = new InboundEmailGeneralHelperMethods(null);
            User user = helper.GetTenant_LogBox(EmailDetails.RecipientEmail);
            return user;
        }
    }
}