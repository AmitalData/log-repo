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

namespace CommunicationWorkerRole
{
    public class QBODocumnetsUploaderWR : WorkerEntryPoint
    {
        QBOAttachmentsUploaderParmeters wrParmeters;

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
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void RunQBODocumnetUploaderWR()
        {
            wrParmeters = new QBOAttachmentsUploaderParmeters();
            wrParmeters.QueueService = new DbQueueService(wrParmeters.QueueName, 0);
            var queueResponse = wrParmeters.QueueService.Receive();

            if (queueResponse != null && queueResponse.MessageId != null)
            {
                try
                {
                    MapQueueResponse(queueResponse);
                    InitializeServices();
                    HandelAPInvoiceAPDNCNDocumnet();
                    wrParmeters.QueueService.Complete();
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
            wrParmeters.Tenant = int.Parse(queueResponse.MessageValues["Tenant"].ToString());
            wrParmeters.DocumentsFilingId = queueResponse.MessageValues["EntityId"].ToString();
            wrParmeters.IsDocumentUploaded = bool.Parse(queueResponse.MessageValues["IsDocumentUploaded"].ToString());
            wrParmeters.IsDocumentDeleted = bool.Parse(queueResponse.MessageValues["IsDocumentDeleted"].ToString());
            wrParmeters.DocumentCode = queueResponse.MessageValues["DocumentCode"].ToString();
        }

        private void InitializeServices()
        {
            wrParmeters.ObjectContext = WebFreightContext.GetContext(wrParmeters.Tenant);
            wrParmeters.ObjectTableRepository = new ObjectTableRepository(wrParmeters.ObjectContext);
            wrParmeters.CommonContext = CommonDataContext.GetContext(wrParmeters.Tenant);
            wrParmeters.APInvoiceRepository = new APInvoiceRepository(wrParmeters.Tenant);
            wrParmeters.Storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            wrParmeters.DocumentsFilingRepository = new DocumentsFilingRepository(wrParmeters.Tenant);
        }

        private void HandelAPInvoiceAPDNCNDocumnet()
        {
            if (string.IsNullOrEmpty(wrParmeters.DocumentsFilingId))
            {
                return;
            }
            DocumentsFilingPM documentsFilingPM = GetDocumentsFilingPM(wrParmeters.DocumentsFilingId, wrParmeters.Tenant);
            if (documentsFilingPM == null)
            {
                return;
            }

            if (wrParmeters.IsDocumentUploaded)
            {
                HandelUploadingDocument(documentsFilingPM);
            }
            else if (wrParmeters.IsDocumentDeleted)
            {
                return;
            }
        }

        private DocumentsFilingPM GetDocumentsFilingPM(string documentsFilingId, int tenant)
        {
            if (string.IsNullOrEmpty(documentsFilingId))
            {
                return null;
            }

            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            return documentsFilingQuery?.GetSinglePM(documentsFilingId, tenant);
        }

        private void HandelUploadingDocument(DocumentsFilingPM documentFiling)
        {          
            this.GetAPInvoice(documentFiling.EntityId);
            if (!IsAPInvoiceSentToQBO())
            {
                return;
            }
            var objectTable = wrParmeters.ObjectTableRepository?.GetSingleObjectTable(documentFiling.ObjectTableId, wrParmeters.Tenant, false);
            if (objectTable == null || objectTable.Name != wrParmeters.ObjectTableName)
            {
                return;
            }
            var documentsFilings = this.GetDocumentFilings(documentFiling);
            this.GetDocumentsFilingsBytes(documentsFilings);
        }

        private List<DocumentsFiling> GetDocumentFilings(DocumentsFilingPM documentFiling)
        {
            var documentsFilings = (from a in wrParmeters.CommonContext.DocumentsFilings.Include("DocumentType")
                                    where a.Tenant == wrParmeters.Tenant && a.EntityId == documentFiling.EntityId
                                    && a.ObjectTableId == documentFiling.ObjectTableId
                                    && a.IsDeleted == false && a.IsTransferdToQBO == false
                                    && (a.DocumentType != null && a.DocumentType.Code == wrParmeters.DocumentCode)
                                    select a).ToList();

            return documentsFilings;
        }

        private void GetDocumentsFilingsBytes(List<DocumentsFiling> documentsFilings)
        {
            if (!IsInvoiceHaveDocumentsFilings(documentsFilings))
            {
                return;
            }
            foreach (DocumentsFiling documentFiling in documentsFilings)
            {
                SendToQBO(documentFiling);
            }
        }
        private void SendToQBO(DocumentsFiling documentFiling)
        {
            Document document = GetDocument(documentFiling.DocumentId);
            var attachmentFileBytes = GetFileData(document);
            this.SendAttachmentQBO(attachmentFileBytes, document, documentFiling);
        }

        private bool IsAPInvoiceSentToQBO()
        {
            if (wrParmeters.APInvoice == null)
            {
                return false;
            }
            if (wrParmeters.APInvoice.TransferStatusCode != "TR")
            {
                return false;
            }
            if(string.IsNullOrEmpty(wrParmeters.APInvoice.ExternalAccountingEntityId))
            {
                return false;
            }
            return true;
        }

        private bool IsInvoiceHaveDocumentsFilings(List<DocumentsFiling> documentsFilings)
        {
            if(documentsFilings == null)
            {
                return false;
            }
            if (!(documentsFilings.Count() >= 1))
            {
                return false;
            }
            return true;
        }

        private void GetAPInvoice(string invoiceId)
        {
            if(string.IsNullOrEmpty(invoiceId))
            {
                return;
            }
            wrParmeters.APInvoice = wrParmeters.APInvoiceRepository.GetSingleAPInvoice(invoiceId, wrParmeters.Tenant);
        }

        private Stream GetFileData(Document document)
        {
            if (document == null)
            {
                return null;
            }
            BlobFileInfo fileInfo = MapBlobFileInfo(document);
            byte[] buffer = wrParmeters.Storageservice.Read(fileInfo);
            Stream stream = new MemoryStream(buffer);
            return stream;
        }

        private Document GetDocument(string documentId)
        {
            Document document = null;
            if (string.IsNullOrEmpty(documentId))
            {
                return document;
            }
            document = (from doc in wrParmeters.CommonContext.Documents
                        where doc.Id == documentId && doc.Tenant == wrParmeters.Tenant
                        select doc).FirstOrDefault();

            return document;
        }

        private BlobFileInfo MapBlobFileInfo(Document document)
        {
            return new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = wrParmeters.Tenant,
                FileSize = document.FileSize,
            };
        }


        private void SendAttachmentQBO(Stream stream, Document document, DocumentsFiling documentFiling)
        {
            ServiceContext serviceContext = GetServiceContext();
            DataService commonServiceQBO = new DataService(serviceContext);
            Attachable attachable = new Attachable();
            attachable.AttachableRef = new AttachableRef[1];
            attachable.AttachableRef[0].EntityRef = new ReferenceType();
            attachable.AttachableRef[0].EntityRef.type = objectNameEnumType.Invoice.ToString();
            attachable.AttachableRef[0].EntityRef.name = objectNameEnumType.Invoice.ToString();
            attachable.AttachableRef[0].EntityRef.Value = wrParmeters.APInvoice.ExternalAccountingEntityId;
            attachable.ContentType = document.Extension;
            attachable.FileName = document.FileName;
            Attachable attachableUploaded = commonServiceQBO.Upload(attachable, stream);
            if(attachableUploaded != null)
            {
                UpdateDocument(documentFiling);
            }

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

            AccountingSettingQuery query = new AccountingSettingQuery(int.Parse(wrParmeters.Tenant.ToString()));
            AccountingSettingPM entityPM = query.GetSingleAccountingSettingPMById(int.Parse(wrParmeters.Tenant.ToString()));
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
            var data = tokenResp.Result;

            if (!String.IsNullOrEmpty(data.Error) || String.IsNullOrEmpty(data.RefreshToken) || String.IsNullOrEmpty(data.AccessToken))
            {
                throw new Exception("Refresh token failed - " + data.Error);
            }

            if (previousRefreshToken != data.RefreshToken)
            {
                entityPM.RefreshToken = data.RefreshToken;
                ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Id);
                AccountingSetting accountingSetting = MyContext.AccountingSettings.Where(p => p.Id == entityPM.Id).FirstOrDefault();
                if (accountingSetting != null)
                {
                    accountingSetting.RefreshToken = data.RefreshToken;
                    MyContext.AccountingSettings.Attach(accountingSetting);
                    MyContext.SetAsModified(accountingSetting);
                    MyContext.SaveChanges();
                }
            }

            return data.AccessToken;
        }

        private void UpdateDocument(DocumentsFiling documentFiling)
        {
            documentFiling.IsTransferdToQBO = true;
            wrParmeters.DocumentsFilingRepository.Update(documentFiling);
            wrParmeters.DocumentsFilingRepository.SubmitChanges();
        }
        private void HandelException(Exception ex)
        {
            ExceptionHandler.HandleException(ex, DateTime.Now, wrParmeters.Tenant, "", "QBO Documnet Uploader code WorkerRole Run Method", "", null);
            wrParmeters.QueueService.CompleteAsFailed();
            Thread.Sleep(10000);
        }
    }

    public class QBOAttachmentsUploaderParmeters {

        public DbQueueService QueueService;
        public int Tenant;
        public string DocumentsFilingId = "";
        public bool IsDocumentUploaded = false;
        public bool IsDocumentDeleted = false;
        public string DocumentCode = "";
        public string ObjectTableName = "APInvoice";
        public string QueueName = "QBODocumnetsUploaderQueue";
        public IWebFreightContext ObjectContext;
        public ObjectTable ObjectTable;
        public APInvoice APInvoice;
        public APInvoiceRepository APInvoiceRepository;
        public ICommonDataContext CommonContext;
        public ObjectTableRepository ObjectTableRepository;
        public IBlobService Storageservice;
        public DocumentsFilingRepository DocumentsFilingRepository;
    }

}
