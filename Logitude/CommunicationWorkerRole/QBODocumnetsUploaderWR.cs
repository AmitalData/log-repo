using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using System.IO;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Microsoft.Practices.Unity;
using Simplog.Data.InvoiceModel.Repositories;
using Intuit.Ipp.Core;
using Intuit.Ipp.DataService;
using Intuit.Ipp.Data;
using System.Net;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Intuit.Ipp.Security;
using Intuit.Ipp.OAuth2PlatformClient;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;

namespace CommunicationWorkerRole
{
    public class QBODocumnetsUploaderWR : WorkerEntryPoint
    {
        QBODocumnetsUploaderParmeters parmeters;

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "QBODocumnetsUploaderWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            return base.OnStart();
        }

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        RunQBODocumnetUploaderWR();
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "QBO Documnet Uploader execution log queue worker role start", null, null);
                        parmeters.QueueService.CompleteAsFailed();
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void RunQBODocumnetUploaderWR()
        {
            parmeters = new QBODocumnetsUploaderParmeters();
            parmeters.QueueService = new DbQueueService(parmeters.QueueName, 0);
            var queueResponse = parmeters.QueueService.Receive();

            if (queueResponse != null && queueResponse.MessageId != null)
            {
                try
                {
                    MapQueueResponse(queueResponse);
                    InitializeServices();
                    HandelAPInvoiceAPDNCNDocumnet();
                    parmeters.QueueService.Complete();
                }
                catch (Exception ex)
                {
                    this.HandelException(ex);
                }
            }
            else
            {
                Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void MapQueueResponse(QueueResponse queueResponse)
        {
            parmeters.Tenant = int.Parse(queueResponse.MessageValues["Tenant"].ToString());
            parmeters.DocumentsFilingId = queueResponse.MessageValues["EntityId"].ToString();
            parmeters.IsDocumentUploaded = bool.Parse(queueResponse.MessageValues["IsDocumentUploaded"].ToString());
            parmeters.IsDocumentDeleted = bool.Parse(queueResponse.MessageValues["IsDocumentDeleted"].ToString());
            parmeters.DocumentCode = queueResponse.MessageValues["DocumentCode"].ToString();
        }

        private void InitializeServices()
        {
            parmeters.ObjectContext = WebFreightContext.GetContext(parmeters.Tenant);
            parmeters.ObjectTableRepository = new ObjectTableRepository(parmeters.ObjectContext);
            parmeters.CommonContext = CommonDataContext.GetContext(parmeters.Tenant);
            parmeters.APInvoiceRepository = new APInvoiceRepository(parmeters.Tenant);
            parmeters.Storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            parmeters.DocumentsFilingRepository = new DocumentsFilingRepository(parmeters.CommonContext);
            parmeters.DocumentsFilingQuery = new DocumentsFilingQuery(parmeters.DocumentsFilingRepository);
            parmeters.CommLogRepository = new CommunicationLogRepository(parmeters.CommonContext);
            parmeters.ContactQuery = new ContactQuery(parmeters.Tenant);
            parmeters.ContentTypes = GetQBOContentTypes();
            this.GetLoggedContactAndTenant();
            this.GetAPInvoiceObjectTableIdByName();
        }

        public Dictionary<string, string> GetQBOContentTypes()
        {
            Dictionary<string, string> contentTypes = new Dictionary<string, string>();
            contentTypes.Add("ai", "application/postscript");
            contentTypes.Add("csv", "text/csv");
            contentTypes.Add("doc", "application/msword");
            contentTypes.Add("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            contentTypes.Add("eps", "application/postscript");
            contentTypes.Add("gif", "image/gif");
            contentTypes.Add("jpeg", "image/jpeg");
            contentTypes.Add("jpg", "image/jpg");
            contentTypes.Add("ods", "application/vnd.oasis.opendocument.spreadsheet");
            contentTypes.Add("pdf", "application/pdf");
            contentTypes.Add("png", "image/png");
            contentTypes.Add("rtf", "text/rtf");
            contentTypes.Add("tif", "image/tiff");
            contentTypes.Add("txt", "text/plain");
            contentTypes.Add("xls", "application/vnd.ms-excel");
            contentTypes.Add("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            contentTypes.Add("xml", "text/xml");

            return contentTypes;
        }

        private void GetLoggedContactAndTenant()
        {
            string email = "system@tenant" + parmeters.Tenant + ".com";
            parmeters.LoggedContact = parmeters.ContactQuery.GetContactByNameAndTenant(email, parmeters.Tenant, true);
            if (parmeters.LoggedContact == null)
            {
                parmeters.LoggedContact = parmeters.ContactQuery.GetContactByEmailOnly(email, parmeters.Tenant);
            }
            parmeters.LoggedTenant = (from a in parmeters.CommonContext.Tenants.Include("AccountingSetting") where a.Id == parmeters.Tenant select a).FirstOrDefault();
        }

        private void GetAPInvoiceObjectTableIdByName()
        {
            var objectTable = parmeters.ObjectTableRepository.GetObjectTableByName("APInvoice", parmeters.Tenant, true);
            parmeters.APInvoiceObjectTableId = objectTable?.Id;
        }

        private void HandelAPInvoiceAPDNCNDocumnet()
        {
            if (string.IsNullOrEmpty(parmeters.DocumentsFilingId))
            {
                return;
            }
            DocumentsFilingPM documentsFilingPM = GetDocumentsFilingPM(parmeters.DocumentsFilingId);
            if (documentsFilingPM == null)
            {
                return;
            }

            if (parmeters.IsDocumentUploaded)
            {
                HandelUploadingDocument(documentsFilingPM);
            }
            else if (parmeters.IsDocumentDeleted)
            {
                return;
            }
        }

        private DocumentsFilingPM GetDocumentsFilingPM(string documentsFilingId)
        {
            if (string.IsNullOrEmpty(documentsFilingId))
            {
                return null;
            }

            return parmeters.DocumentsFilingQuery?.GetSinglePM(documentsFilingId, parmeters.Tenant);
        }

        private void HandelUploadingDocument(DocumentsFilingPM documentFiling)
        {
            ObjectTable objectTable = parmeters.ObjectTableRepository?.GetSingleObjectTable(documentFiling.ObjectTableId, parmeters.Tenant, false);
            if (!IsShipmentOrAPInvoiceObjectTable(objectTable))
            {
                return;
            }

            this.GetAPInvoice(GetInvoiceIdFromDocumentsFiling(objectTable.Name, documentFiling));
            if (!IsAPInvoiceSentToQBO())
            {
                return;
            }
            var documentsFilings = this.GetAPInvoiceDocumentFilings(documentFiling);
            this.HandleSendingDocumentsFilingsToQBO(documentsFilings);
        }

        private bool IsShipmentOrAPInvoiceObjectTable(ObjectTable objectTable)
        {
            if (objectTable == null)
            {
                return false;
            }
            if (objectTable.Name == parmeters.APInvoiceObjectTableName)
            {
                return true;
            }
            if (objectTable.Name == parmeters.ShipmentObjectTableName)
            {
                return true;
            }

            return false;
        }

        private string GetInvoiceIdFromDocumentsFiling(string objectTableName, DocumentsFilingPM documentFiling)
        {
            if (objectTableName == parmeters.APInvoiceObjectTableName)
            {
                return documentFiling.EntityId;
            }
            if (objectTableName == parmeters.ShipmentObjectTableName)
            {
                return documentFiling.ChildEntityId;
            }
            return "";
        }

        private void GetAPInvoice(string invoiceId)
        {
            if (string.IsNullOrEmpty(invoiceId))
            {
                return;
            }
            parmeters.APInvoice = parmeters.APInvoiceRepository.GetSingleAPInvoice(invoiceId, parmeters.Tenant);
        }


        private bool IsAPInvoiceSentToQBO()
        {
            if (parmeters.APInvoice == null)
            {
                return false;
            }
            if (parmeters.APInvoice.TransferStatusCode != "TR")
            {
                return false;
            }
            if (string.IsNullOrEmpty(parmeters.APInvoice.ExternalAccountingEntityId))
            {
                return false;
            }
            return true;
        }

        private List<DocumentsFiling> GetAPInvoiceDocumentFilings(DocumentsFilingPM documentFiling)
        {
            var documentsFilings = (from a in parmeters.CommonContext.DocumentsFilings.Include("DocumentType")
                                    where a.Tenant == parmeters.Tenant && a.EntityId == documentFiling.EntityId
                                    && a.ObjectTableId == documentFiling.ObjectTableId
                                    && a.IsDeleted == false && a.IsTransferdToQBO == null
                                    && (a.DocumentType != null && a.DocumentType.Code == parmeters.DocumentCode)
                                    select a).ToList();

            return documentsFilings;
        }

        private void HandleSendingDocumentsFilingsToQBO(List<DocumentsFiling> documentsFilings)
        {
            if (!IsInvoiceHaveDocumentsFilings(documentsFilings))
            {
                return;
            }
            foreach (DocumentsFiling documentFiling in documentsFilings)
            {
                this.RunSendingAttachmentToQBOProcess(documentFiling);
            }
        }

        private bool IsInvoiceHaveDocumentsFilings(List<DocumentsFiling> documentsFilings)
        {
            if (documentsFilings == null)
            {
                return false;
            }
            if (!(documentsFilings.Count() >= 1))
            {
                return false;
            }
            return true;
        }


        private void RunSendingAttachmentToQBOProcess(DocumentsFiling documentFiling)
        {
            Document document = GetDocument(documentFiling.DocumentId);
            this.BuildCommunicationLog(string.Concat(document.FileName, ".", document.Extension));
            byte[] attachmentFileBytes = GetFileDataBytes(document);
            this.SendAttachmentQBO(attachmentFileBytes, document, documentFiling);
        }

        private Document GetDocument(string documentId)
        {
            Document document = null;
            if (string.IsNullOrEmpty(documentId))
            {
                return document;
            }
            document = (from doc in parmeters.CommonContext.Documents
                        where doc.Id == documentId && doc.Tenant == parmeters.Tenant
                        select doc).FirstOrDefault();

            return document;
        }

        private void BuildCommunicationLog(string communicationLogAdditionalFields)
        {
            CommunicationsParams communicationLogParams = new CommunicationsParams()
            {
                Tenant = parmeters.Tenant,
                From = "Logitude",
                To = "QBO",
                CommunicationLogTypeCode = "T",
                Priority = 1,
                InOut = "O",
                Status = "W",
                LoggingUserId = this.parmeters.LoggedContact?.Id,
                LoggingObjectTableId = parmeters.APInvoiceObjectTableId,
                LoggingEntityId = parmeters.APInvoice?.Id,
                LoggingEntityReference = parmeters.APInvoice?.InternalNumber,
                Subject = "QBO Attachments Upload",
                FolderName = "QBO",
                AdditionalFields = communicationLogAdditionalFields,
                ByteData = new byte[] { }

            };
            parmeters.CommunicationLogId = Communications.AddCommunicationLog(communicationLogParams);
        }

        private byte[] GetFileDataBytes(Document document)
        {
            if (document == null)
            {
                return null;
            }
            BlobFileInfo fileInfo = MapBlobFileInfo(document);
            byte[] buffer = parmeters.Storageservice.Read(fileInfo);
            return buffer;
        }

        private BlobFileInfo MapBlobFileInfo(Document document)
        {
            return new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = parmeters.Tenant,
                FileSize = document.FileSize,
            };
        }

        private void SendAttachmentQBO(byte[] bytesStream, Document document, DocumentsFiling documentFiling)
        {
            try
            {
                this.HandelSendingAttachmentStreamToQBO(bytesStream, document, documentFiling);
            }
            catch (Exception exception)
            {
                HandelException(exception);
            }
        }

        private void HandelSendingAttachmentStreamToQBO(byte[] bytesStream, Document document, DocumentsFiling documentFiling)
        {
            var response = SendAttachmentStreamToQBO(bytesStream, document, documentFiling);
            if (response != null)
            {
                UpdateDocument(documentFiling, true);
                UpdateCommunicationLogAsDone();
            }
            else
            {
                Exception ex = null;
                UpdateCommunicationLogAsFiled(ex);
            }
        }

        private Attachable SendAttachmentStreamToQBO(byte[] bytesStream, Document document, DocumentsFiling documentFiling)
        {
            ServiceContext serviceContext = GetServiceContext();
            DataService commonServiceQBO = new DataService(serviceContext);
            Attachable attachable = this.GetQBOAttachableObject();
            Attachable attachableRsponse = null;
            using (MemoryStream stream = new MemoryStream(bytesStream))
            {
                attachable.ContentType = GetContentType(document, documentFiling);
                attachable.FileName = string.Concat(document.FileName, ".", document.Extension);
                attachableRsponse = commonServiceQBO.Upload(attachable, stream);
                stream.Close();
            }

            return attachableRsponse;
        }

        public ServiceContext GetServiceContext()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Setting setting = null;
            using (TransactionScope scope = Simplog.Server.Infrastructure.Helpers.TransactionFactory.GetNewTransaction())
            {
                SettingRepository mySettingRepository = new SettingRepository();
                setting = mySettingRepository.GetSingleSetting("1");
                scope.Complete();
            }

            AccountingSettingQuery query = new AccountingSettingQuery(int.Parse(parmeters.Tenant.ToString()));
            AccountingSettingPM entityPM = query.GetSingleAccountingSettingPMById(int.Parse(parmeters.Tenant.ToString()));
            return GetServiceContextAuth2(entityPM, setting);
        }
        private ServiceContext GetServiceContextAuth2(AccountingSettingPM entityPM, Setting mySetting)
        {
            OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(GetAccessToken(entityPM, mySetting));
            ServiceContext serviceContext = new ServiceContext(entityPM.QBOrealMeID, IntuitServicesType.QBO, oauthValidator);
            serviceContext.IppConfiguration.BaseUrl.Qbo = "https://quickbooks.api.intuit.com/";

            return serviceContext;
        }

        public string GetAccessToken(AccountingSettingPM entityPM, Setting mySetting)
        {
            var oauth2Client = new OAuth2Client(mySetting.QBOClientID,
                    mySetting.QBOClientSecret,
                    "https://developer.intuit.com/v2/OAuth2Playground/RedirectUrl",
                    "production");

            var previousRefreshToken = entityPM.RefreshToken;
            var tokenResp = oauth2Client.RefreshTokenAsync(previousRefreshToken);
            tokenResp.Wait();
            var tokenResponse = tokenResp.Result;

            if (!String.IsNullOrEmpty(tokenResponse.Error) || String.IsNullOrEmpty(tokenResponse.RefreshToken) || String.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                throw new Exception("Refresh token failed - " + tokenResponse.Error);
            }
            if (previousRefreshToken != tokenResponse.RefreshToken)
            {
                this.UpdateQBORefreshToken(tokenResponse, entityPM);
            }

            return tokenResponse.AccessToken;
        }

        private void UpdateQBORefreshToken(dynamic response, AccountingSettingPM entityPM)
        {
            entityPM.RefreshToken = response.RefreshToken;
            AccountingSetting accountingSetting = parmeters.CommonContext.AccountingSettings.Where(p => p.Id == entityPM.Id).FirstOrDefault();
            if (accountingSetting != null)
            {
                this.UpdateAccountingSettings(response, accountingSetting);
            }
        }

        private void UpdateAccountingSettings(dynamic response, AccountingSetting accountingSetting)
        {
            accountingSetting.RefreshToken = response.RefreshToken;
            parmeters.CommonContext.AccountingSettings.Attach(accountingSetting);
            parmeters.CommonContext.SetAsModified(accountingSetting);
            parmeters.CommonContext.SaveChanges();
        }

        private Attachable GetQBOAttachableObject()
        {
            Attachable attachable = new Attachable();
            attachable.AttachableRef = new AttachableRef[1];
            attachable.AttachableRef[0] = new AttachableRef();
            attachable.AttachableRef[0].EntityRef = new ReferenceType();
            attachable.AttachableRef[0].EntityRef.type = objectNameEnumType.Bill.ToString();
            attachable.AttachableRef[0].EntityRef.name = objectNameEnumType.Bill.ToString();
            attachable.AttachableRef[0].EntityRef.Value = parmeters.APInvoice.ExternalAccountingEntityId;

            return attachable;
        }

        private string GetContentType(Document document, DocumentsFiling documentFiling)
        {

            if (!parmeters.ContentTypes.ContainsKey(document.Extension.ToLower()))
            {
                this.HandleNotSupportedTypesInQBOAttachments(document, documentFiling);
            }
            return (parmeters.ContentTypes[document.Extension.ToLower()]);
        }

        private void HandleNotSupportedTypesInQBOAttachments(Document document, DocumentsFiling documentFiling)
        {
            UpdateDocument(documentFiling, false);
            throw new Exception(document.Extension + " files are not supported in the QBO Attachments API");
        }

        private void UpdateDocument(DocumentsFiling documentFiling, bool isTransferdToQBO)
        {
            documentFiling.IsTransferdToQBO = isTransferdToQBO;
            parmeters.DocumentsFilingRepository.Update(documentFiling);
            parmeters.DocumentsFilingRepository.SubmitChanges();
        }

        private void UpdateCommunicationLogAsDone()
        {
            CommunicationLog waitingCommLog = parmeters.CommLogRepository.GetSingleCommunicationLog(parmeters.CommunicationLogId, parmeters.Tenant);
            waitingCommLog.CommunicationStatusTypeCode = "D";
            waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(parmeters.Tenant);
            waitingCommLog.DoneDateUTC = DateTime.UtcNow;
            waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(parmeters.Tenant);
            waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
            parmeters.CommLogRepository.Update(waitingCommLog);
            parmeters.CommLogRepository.SubmitChanges();
        }

        private void UpdateCommunicationLogAsFiled(Exception exception)
        {
            CommunicationLog waitingCommLog = parmeters.CommLogRepository.GetSingleCommunicationLog(parmeters.CommunicationLogId, parmeters.Tenant);
            waitingCommLog.CommunicationStatusTypeCode = "F";
            waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(parmeters.Tenant);
            waitingCommLog.DoneDateUTC = DateTime.UtcNow;
            waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(parmeters.Tenant);
            waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
            waitingCommLog.ExceptionMessage = exception == null ? "Faild to Send Attachment " : exception.Message;
            parmeters.CommLogRepository.Update(waitingCommLog);
            parmeters.CommLogRepository.SubmitChanges();
        }


        private void HandelException(Exception exception)
        {
            ExceptionHandler.HandleException(exception, DateTime.Now, parmeters.Tenant, "", "QBO Documnet Uploader code WorkerRole Run Method", "", null);
            UpdateCommunicationLogAsFiled(exception);
            parmeters.QueueService.CompleteAsFailed();
            Thread.Sleep(10000);
        }
    }

    public class QBODocumnetsUploaderParmeters
    {
        public DbQueueService QueueService = null;
        public int Tenant;
        public string DocumentsFilingId = "";
        public bool IsDocumentUploaded = false;
        public bool IsDocumentDeleted = false;
        public string DocumentCode = "";
        public string APInvoiceObjectTableName = "APInvoice";
        public string ShipmentObjectTableName = "Shipment";
        public string QueueName = "QBODocumnetsUploaderQueue";
        public IWebFreightContext ObjectContext = null;
        public ObjectTable ObjectTable = null;
        public APInvoice APInvoice = null;
        public APInvoiceRepository APInvoiceRepository = null;
        public ICommonDataContext CommonContext = null;
        public ObjectTableRepository ObjectTableRepository = null;
        public IBlobService Storageservice = null;
        public DocumentsFilingRepository DocumentsFilingRepository = null;
        public DocumentsFilingQuery DocumentsFilingQuery = null;
        public Dictionary<string, string> ContentTypes = null;
        public CommunicationLogRepository CommLogRepository = null;
        public Tenant LoggedTenant = null;
        public ContactPM LoggedContact = null;
        public ContactQuery ContactQuery = null;
        public string CommunicationLogId = null;
        public string APInvoiceObjectTableId = null;
    }

}