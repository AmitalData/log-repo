using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mapping;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Enums;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Transactions;
using System.Web;
using WebFreight.Web;
using WebFreight.Web;
using Contact = Simplog.Data.CommonDataModel.EntityPOCOs.Contact;
using Customer = Simplog.Data.CommonDataModel.EntityPOCOs.Customer;
using DocumentsFiling = Simplog.Data.CommonDataModel.EntityPOCOs.DocumentsFiling;
using DocumentType = Simplog.Data.CommonDataModel.EntityPOCOs.DocumentType;
using User = Simplog.Data.CommonDataModel.EntityPOCOs.User;

namespace Logitude.BL.Helpers
{
    public class DocumentHelper
    {

        private readonly bool IsAutomation;
        private int Tenant;
        public bool isInterestReport = false;

        public DocumentHelper(bool isAutomation = false )
        {
            IsAutomation = isAutomation;
        }

        public DocumentOutPM CreateDocumentOut(string documentTypeId, string entityId, string childEntityId, string childReference, string objectTableId, int tenant, string userId = null , string documentTemplateId = null)
        {
            this.Tenant = tenant;
            try
            {
                ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
                DocumentOutRepository documentOutRepository = new DocumentOutRepository(objectContext);
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(objectContext);
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(documentOutRepository);

                DocumentType documentType = documentTypeRepository.GetSingleDocumentTypes(documentTypeId, tenant);
                string emailTemplateId = null;

                if (string.IsNullOrEmpty(documentTemplateId))
                {
                    documentTemplateId = documentType.DocumentTypeDefaultReportTemplateId;
                }

                emailTemplateId = documentType.DocumentTypeDefaultHTMLTemplateId;


                //---------------------------------------- 
                DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(objectContext);

                if (string.IsNullOrEmpty(userId))
                {
                    string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
                    UserRepository userRepository = new UserRepository(tenant);
                    User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);

                    if (loggedUser != null)
                    {
                        userId = loggedUser.Id;
                    }
                }

                DocumentsFiling newDocumentFiling = new DocumentsFiling() { DocumentTypeId = documentTypeId, EntityId = entityId, Tenant = tenant, ObjectTableId = objectTableId, ChildEntityId = childEntityId, ChildEntityReference = childReference, DirectionCode = "O" };

                newDocumentFiling.Id = IdCounter.GetNumber("Document", tenant).ToString();
                string com_id = newDocumentFiling.Id;        // Length = 30
                string com_md5 = CreateMD5(com_id); // Length = 32 
                string com_short = newDocumentFiling.Id.Substring(0, 8);
                newDocumentFiling.SecurityId = com_short + com_md5; // Length = 40
                newDocumentFiling.Code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
                newDocumentFiling.CreatedByUserId = userId;
                newDocumentFiling.OwnerId = userId;
                newDocumentFiling.UpdatedByUserId = userId;
                newDocumentFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                newDocumentFiling.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                newDocumentFiling.SearchFields = newDocumentFiling.Code + "," + newDocumentFiling.DirectionCode;
                documentsFilingRepository.Add(newDocumentFiling);

                //----------------------------------------
                DocumentOut newDocument = CreateDocumentOutInstance(userId, documentTemplateId, emailTemplateId);
                newDocument.Id = newDocumentFiling.Id;
                documentOutRepository.Add(newDocument);

                objectContext.SaveChanges();

                DocumentOutPM docPM = documentOutQuery.GetSinglePM(newDocument.Id, newDocument.Tenant);
                return docPM;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }


                string authenticateduser = "";

                try
                {
                    authenticateduser = Security.SecurityUtility.GetAuthenticatedUser();
                }

                catch
                {
                    authenticateduser = "UnKnown";
                }
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(new Exception(Error), DateTime.Now, 0, "", authenticateduser, "", ip);
                throw new Exception(Error);
            }
        }

        private static string CreateMD5(string input)

        {

            // Use input string to calculate MD5 hash

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())

            {

                byte[] inputBytes = System.Text.Encoding.Unicode.GetBytes(input);

                byte[] hashBytes = md5.ComputeHash(inputBytes);

                //Convert the byte array to hexadecimal string prior to.NET 5

                StringBuilder sb = new System.Text.StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)

                {

                    sb.Append(hashBytes[i].ToString("X2"));

                }

                return sb.ToString();

            }

        }

        public (DocumentOutPM document, bool isSign) PutCreateDocumentOut(CreateDocumentOutArgs createDocumentOutArgs, string userId = null)
        {
            bool isSign = false;
            DocumentOutQuery documentOutQuery = new DocumentOutQuery(createDocumentOutArgs.Tenant);
            DocumentOutPM documentOutPM = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(createDocumentOutArgs.EntityId, createDocumentOutArgs.ChildEntityId, createDocumentOutArgs.DocumentTypeId, createDocumentOutArgs.Tenant);
            if (documentOutPM == null)
            {
                documentOutPM = CreateDocumentOut(createDocumentOutArgs.DocumentTypeId, createDocumentOutArgs.EntityId, createDocumentOutArgs.ChildEntityId, createDocumentOutArgs.ChildReference, createDocumentOutArgs.ObjectTableId, createDocumentOutArgs.Tenant, userId, createDocumentOutArgs.DocumentTypeTemplateId);
            }
            if (documentOutPM != null)
            { 
                if (createDocumentOutArgs.SignHSM)
                {
                    IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
                    FullAccountingSettingPM accountingSettings = query.GetFullAccountingSettingByTenant(createDocumentOutArgs.Tenant);
                    if (accountingSettings.AccountingActivated && !string.IsNullOrEmpty(accountingSettings.HSM) && !string.IsNullOrEmpty(accountingSettings.HSMaddress) && !string.IsNullOrEmpty(accountingSettings.HSMtoken))
                        isSign = Sign(documentOutPM.Id, createDocumentOutArgs.Tenant, accountingSettings);

                }

            }
            return (documentOutPM, isSign);
        }

        private DocumentOut CreateDocumentOutInstance(string userId, string documentTemplateId, string emailTemplateId)
        {
            DocumentOut documentOut = new DocumentOut() { EmailTemplateId = emailTemplateId, DocumentTemplateId = documentTemplateId, Tenant = Tenant, Issued = false };
            if (!IsAutomation) return documentOut;
            documentOut.Issued = true;
            documentOut.IssuedDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
            documentOut.IssuedByUserId = userId;
            return documentOut;
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public void EncryptionDocument()
        {
            TenantQuery tenantQuery = new TenantQuery(0);
            string strConnString = GetConnection(0);
            List<TenantList> tenantList = tenantQuery.GetAllTenantLists().ToList();
            AesFunction aesFunction = new AesFunction();
            foreach (TenantList tenant in tenantList)
            {
                DocumentRepository documentRepository = new DocumentRepository(0);
                List<Document> documentsList = documentRepository.GetDocuments(tenant.Id).Where(d => !d.IsEncrypted).ToList();

                foreach (Document document in documentsList)
                {
                    try
                    {
                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                        BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = document.Id,
                            FolderName = document.Folder,
                            Extension = document.Extension,
                            Tenant = document.Tenant,
                            IsDecrypted = true,
                            AesKey = tenant.StorageEncryptionKey,
                        };
                        byte[] fileData = storageservice.Read(fileInfo);

                        if (fileData != null)
                        {
                            fileInfo.IsEncrypted = true;
                            storageservice.Write(fileData, fileInfo);

                            using (SqlConnection cn = new SqlConnection(strConnString))
                            {
                                string cmd = "Update Documents set IsEncrypted=1 , HasFile =1 where id =" + "'" + document.Id + "' and tenant =" + document.Tenant;
                                SqlCommand sqlCommand = new SqlCommand(cmd, cn);
                                cn.Open();
                                sqlCommand.ExecuteNonQuery();
                                cn.Close();
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        string data = "@ Tenant : " + tenant.Id + "@ DocumentId : " + document.Id + "@ Exception : " + ex.Message;
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "EncryptionDocument:Tenant = " + tenant.Id + "@DocumentId=" + document.Id, null, null);
                    }
                }


            }
        }

        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }

        
        

        public void RetrySignature(string documentId, int tenant)
        {
             

            try
            {
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
                DocumentOutPM documentOutPM = documentOutQuery.GetSinglePM(documentId, tenant);
                if (documentOutPM != null)
                {

                    IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
                    FullAccountingSettingPM accountingSettings = query.GetFullAccountingSettingByTenant(tenant);
                    DocumentHelper DocumentHelper = new DocumentHelper();
                    if (accountingSettings.AccountingActivated && !string.IsNullOrEmpty(accountingSettings.HSM) && !string.IsNullOrEmpty(accountingSettings.HSMaddress) && !string.IsNullOrEmpty(accountingSettings.HSMtoken))
                        Sign(documentOutPM.Id, tenant, accountingSettings);

                }
            }
           
            catch (Exception ex)
            {

                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "ProccessHSMSign-MarkExportSignTaskAsDone", "", null);


            }
        }



        public void StartSignPDFInvoice(ARInvoice invoice, int tenant, ARInvoiceRepository repository,string contactEmail, FullAccountingSettingPM accountingSettings)
        {

            try
            {
                 
                IHSMSignFileService HSMSignFileService = ContainerAccessor.Container.Resolve(typeof(IHSMSignFileService), "HSMSignFileService", new ParameterOverride("", 1)) as IHSMSignFileService;
                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);

                DocumentRepository documentRepository = new DocumentRepository(commoncontext);
                DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(commoncontext);
                DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);
                DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(tenant);


               TenantRepository tenantRepository = new TenantRepository(tenant);
                DocumentsFilingPM myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(invoice.Id, tenant).FirstOrDefault();
                DocumentOutCopyPM documentOutCopyPM = documentOutCopyQuery.GetDocumentOutCopiesForDocumentOutAndType(myDocumentFilings.Id, tenant, "999G");
                Document document = documentRepository.GetSingleDocument(tenant, documentOutCopyPM?.DocumentId);
                ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
                  var vatNumber = tenantRepository.GetSingleByTenant(tenant).VatNumber;

                if (document != null)
                {
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

                    byte[] signBytes = HSMSignFileService
                        .SignCustomsRequest(tenant, invoice.Id, filedata, document.FileName, vatNumber, loggedcontact?.Id, accountingSettings);

                    if (signBytes != null)
                    {
                        storageservice.Write(signBytes, fileInfo);
                        APInvoiceHelper.AddCommunicationLog("D", invoice, signBytes.ToString(), "ARInvoice", invoice?.Id, "signBytes is ok", Tenant);

                        this.HSMSignatureSucceeded(invoice, repository, contactEmail, document, myDocumentFilings.Id, accountingSettings);
                       
                        
                    }
                    else
                    {
                        APInvoiceHelper.AddCommunicationLog("F", invoice, signBytes.ToString(), "ARInvoice", invoice?.Id, "signBytes is empty", Tenant);

                        throw new HSMException($"{invoice?.Id},response: Result.Content.ReadAsByteArrayAsync().Result is empty", "Fails");
                    }
                }
                else
                {
                    throw new ArgumentNullException("There is no document");
                }
            }
            catch(HSMException ex)
            {
                APInvoiceHelper.AddCommunicationLog("F", invoice, ex.Message, "ARInvoice", invoice.Id, "HSM Signature Failed", tenant);

                this.HSMSignatureFailed(invoice, ex, repository);
                                 
            }
            catch (Exception ex)
            {
                APInvoiceHelper.AddCommunicationLog("F", invoice, ex.Message, "ARInvoice", invoice.Id, "HSM Signature Failed", tenant);

                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "ProccessHSMSign-MarkExportSignTaskAsDone", "", null);


            }
        }



        public bool CheckPDFInvoiceInStorage_Inner(ARInvoice invoice, int tenant, ARInvoiceRepository repository, string contactEmail, FullAccountingSettingPM accountingSettings)
        {
            bool rv = false;
             

            try
            {

                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);

                DocumentRepository documentRepository = new DocumentRepository(commoncontext);
                DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(commoncontext);
                DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);
                DocumentOutCopyQuery DocumentOutCopyQuery = new DocumentOutCopyQuery(tenant);


                TenantRepository tenantRepository = new TenantRepository(tenant);
                DocumentsFilingPM myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(invoice.Id, tenant).FirstOrDefault();
                DocumentOutCopyPM documentOutCopyPM = DocumentOutCopyQuery.GetDocumentOutCopiesForDocumentOutAndType(myDocumentFilings.Id, tenant, "999G");
                Document document = documentRepository.GetSingleDocument(tenant, documentOutCopyPM?.DocumentId);
                ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
                var vatNumber = tenantRepository.GetSingleByTenant(tenant).VatNumber;

                if (document != null)
                {
                    Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = tenant,
                        FileSize = document.FileSize,
                    };

                    Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;

                    bool fileExists = storageservice.FileExists(fileInfo);
                    return fileExists;
                }
                else
                {
                    throw new ArgumentNullException("There is no document");
                }
                
            }
            catch (Exception ex)
            {
                APInvoiceHelper.AddCommunicationLog("F", invoice, ex.Message, "ARInvoice", invoice.Id, "Check PDF Invoice In Storage Failed", tenant);

                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "CheckPDFInvoiceInStorage", "", null);
                return false;

            }
        }

        private void HSMSignatureFailed(ARInvoice invoice, HSMException ex, ARInvoiceRepository repository)
        {
            invoice.IsSigned = ARInvoiceSignedStatusValues.SigningFailed;
            repository.Update(invoice);
            repository.SubmitChanges();
             this.CreateEvent("HSMF", invoice, "חתימת החשבונית לא  צלחה"+ ex);
            this.SendEmailAlert("ohad@amital.co.il", "  חתימה בHSM נכשלה", " חתימת החשבונית נכשלה &ensp;&ensp;&ensp; חשבונית מספר"+ invoice.InvoiceNumber+ "<br /><br />מצורפת השגיאה "+ex, invoice.Tenant, invoice.Id);
        }


        private void HSMSignatureSucceeded(ARInvoice invoice, ARInvoiceRepository repository,string contactEmail,Document document,string  DocumentFilingId ,FullAccountingSettingPM accountingSettings)
        {

            invoice.IsSigned = ARInvoiceSignedStatusValues.SignedButNotYetSent;
            repository.Update(invoice);
            repository.SubmitChanges();
            this.CreateEvent("HSMS", invoice, DocumentFilingId + "החשבונית נחתמה בהצלחה :");
            this.SendToEmailContact(contactEmail, invoice, document, DocumentFilingId, repository, invoice.Tenant, accountingSettings);

        }
        public void SendSignInterestInvoices(string[] selectedList ,int tenant)
        {

            ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            ARInvoiceRepository repository = new ARInvoiceRepository();
            DocumentRepository documentRepository = new DocumentRepository(commoncontext);
            DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(commoncontext);
            DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);
            DocumentOutCopyQuery DocumentOutCopyQuery = new DocumentOutCopyQuery(tenant);
            IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
            FullAccountingSettingPM accountingSettings = query.GetFullAccountingSettingByTenant(tenant);


            List<string> failedItems = new List<string>();
            try
            {
                foreach (var item in selectedList)
                {
                    try
                    {
                        ARInvoice invoice = repository.GetSingleARInvoice(item, tenant);
                        bool isPrinted = invoice.IsPrinted;

                        if (invoice.IsPrinted)
                        {
                            bool isPDFExist = this.CheckPDFInvoiceInStorage_Inner(invoice, tenant, repository, null, accountingSettings);
                            if (!isPDFExist)
                            {
                                isPrinted = false;
                            }
                        }

                        if (!isPrinted)
                        {
                            PrintInterestInvoice(tenant, invoice, invoiceContext);
                        }

                        else if (invoice.IsSigned == ARInvoiceSignedStatusValues.NotSigned || invoice.IsSigned == ARInvoiceSignedStatusValues.SigningFailed)
                        {
                            SignInterestInvoice(tenant, invoice, invoiceContext);
                        }
                        else if (invoice.IsSigned == ARInvoiceSignedStatusValues.SignedButNotYetSent || invoice.IsSigned == ARInvoiceSignedStatusValues.SignedButSendingByEmailFailed)
                        {
                            DocumentsFilingPM myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(invoice?.Id, tenant).FirstOrDefault();
                            DocumentOutCopyPM documentOutCopyPM = DocumentOutCopyQuery.GetDocumentOutCopiesForDocumentOutAndType(myDocumentFilings?.Id, tenant, "999G");
                            Document document = documentRepository.GetSingleDocument(tenant, documentOutCopyPM?.DocumentId);
                            string contactEmail = this.IsSignatureHtmlPresentByBillToId(invoice?.BillToId, tenant);
                            if (!string.IsNullOrEmpty(contactEmail))
                                this.SendToEmailContact(contactEmail, invoice, document, myDocumentFilings?.Id, repository, tenant, accountingSettings);
                        }
                    }
                    catch (Exception ex)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError($"SendSignInterestInvoices  ARInvoiceId :{item}   , Err:{ex} ");
                        failedItems.Add(item);
                    }
                 }
             }
            catch (Exception e)
            {

                throw e;
            }
                    
        }

        public void PrintInterestInvoice(int tenant, ARInvoice aRInvoice, IInvoiceContext invoiceContext)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(tenant);
                    InterestReport interestReport = aRInvoiceQuery.GetInterestReport(aRInvoice);
                    ARInvoicePM aRInvoicePM = aRInvoiceQuery.GetSinglePM(aRInvoice.Id, tenant);
                    if (aRInvoicePM == null || interestReport == null) throw new ArgumentNullException("There is no ARInvoice or InterestReport");

                    ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, tenant);
                    invoiceService.BuildDocumentsForNewInvoiceLite(aRInvoicePM, interestReport);
                    invoiceService.SignInvoice(aRInvoicePM, tenant);
                    NetCommonHelper.Logger.DevLog.Instance.WriteTrace("PrintInterestInvoice aRInvoicePM.Id=" + aRInvoicePM.Id);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                   NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, "Error in PrintInterestInvoice aRInvoice.Id=" + aRInvoice.Id);
                }

            }
        }


        public void SignInterestInvoice(int tenant, ARInvoice aRInvoice, IInvoiceContext invoiceContext)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(tenant);
                    ARInvoicePM aRInvoicePM = aRInvoiceQuery.GetSinglePM(aRInvoice.Id, tenant);
                    if (aRInvoicePM == null) throw new ArgumentNullException("There is no ARInvoice");

                    ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, tenant);
                    invoiceService.SignInvoice(aRInvoicePM, tenant);
                    NetCommonHelper.Logger.DevLog.Instance.WriteTrace("PrintInterestInvoice aRInvoicePM.Id=" + aRInvoicePM.Id);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, "Error in PrintInterestInvoice aRInvoice.Id=" + aRInvoice.Id);
                }

            }
        }

        private void CreateEvent(string eventCode,ARInvoice arinvoice, string Notes = null)
        {
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(Tenant);
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = arinvoice.Id,
                Tenant = arinvoice.Tenant,
                UserId = loggedContact.Id,
                ObjectTableName = "ARInvoice",
                EventTypeCode = eventCode,
                Notes = Notes,
            });
        }

        private void SendEmailAlert(string email, string subject, string body,int tenant,string entityId)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRepository = new ContactRepository(commonContext);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);

            StringBuilder HtmlTemplate = new StringBuilder();
            HtmlTemplate.Append("<p style='text-align:left'>");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append(body);
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Thank you");

            if (loggedContact == null) return;

            EmailCommunicationParams emailParams = new EmailCommunicationParams()
            {

                To = email,
                Subject = subject,
                EmailBody = HtmlTemplate.ToString(),
                LoggingUserId = loggedContact.Id,
                Tenant = tenant,
                LoggingEntityId= entityId,
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("ARInvoice")
,

            };
            Communications.AddEmailCommunicationLogQueue(emailParams, tenant);
        }


        private string GetContactEmailByContactId(string loggedContactId, int tenant, ContactQuery contactQuery)
        {
            string contactEmail = string.Empty;
            if (!string.IsNullOrEmpty(loggedContactId))
            {
                if (contactQuery == null) contactQuery = new ContactQuery(tenant);
                contactEmail = contactQuery.GetContactEmailById(loggedContactId, tenant);
                if (contactEmail == null && tenant != 0) contactEmail = contactQuery.GetContactEmailById(loggedContactId, 0);
            }
            return contactEmail;
        }

        public void SendToEmailContactOuter(string email, ARInvoice arinvoice, Document document, string DocumentFilingId, ARInvoiceRepository repository, int tenant, bool isInterestReport)
        {
            this.isInterestReport = isInterestReport;
            IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
            FullAccountingSettingPM accountingSettings = query.GetFullAccountingSettingByTenant(tenant);
            this.SendToEmailContact(email, arinvoice, document, DocumentFilingId, repository, tenant, accountingSettings);
        }

        private void SendToEmailContact(string email, ARInvoice arinvoice,Document document,string DocumentFilingId, ARInvoiceRepository repository,int tenant,FullAccountingSettingPM accountingSettings)
        {
            string loggedUserEmail = null;
            try
            {
                 loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
            }
            catch (Exception ex)
            {
                if (loggedUserEmail == null)
                {
                    ContactRepository contactRepository = new ContactRepository(tenant);
                    Contact contact = contactRepository.GetContactByUserTypeAndTenant("S", tenant);
                    loggedUserEmail = contact.Email;
                }
            }
            UserRepository userRepository = new UserRepository(Tenant);
            User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);
              System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
             EncodedHtmlHelper encodedHtmlHelper = new EncodedHtmlHelper();
            string htmlstring = "";
            string userId = null;
            string LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("ARInvoice");
            string htmlString = "<html><head><meta http- equiv='Content- Type' content= 'text/html; charset = iso-8859-1' > <style type='text/css' style= 'display: none; '></style></head><body>";
           
            htmlString += "</body></html>";
             
            byte[] bytedata = Encoding.UTF8.GetBytes(htmlString);

            if (loggedUser != null)
            {
                userId = loggedUser.Id;
            }
            try {
                 string subject = accountingSettings?.InvoiceNotes?? "חשבונית חתומה";
                Document documentInterestReport = new Document();
                string documentInterestReportId = "";
                if (this.isInterestReport && arinvoice.ARInvoiceTypeCode=="IT")
                {
                    documentInterestReportId = this.GetDocumentInterestReportId(arinvoice.Id, tenant);
                    subject = accountingSettings?.InterestInvoiceNotes;
                }

                string tenantDotCom = "system@tenant" + tenant + ".com";
                string xxxDotCom = "unifreight@xxxxxxx.com";
                string system = "SYSTEM";

                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                ContactRepository contactRepository = new ContactRepository(commonDataContext);
                ContactQuery contactQuery = new ContactQuery(contactRepository);
                string fromEmail = GetContactEmailByContactId(arinvoice?.IssuedByUserId, tenant, contactQuery);    
                if (String.IsNullOrWhiteSpace(fromEmail) || fromEmail.ToLowerInvariant() == tenantDotCom)
                {
                    fromEmail = GetContactEmailByContactId(arinvoice?.UpdatedByUserId, tenant, contactQuery);
                    if (String.IsNullOrWhiteSpace(fromEmail) || fromEmail.ToLowerInvariant() == tenantDotCom)
                    {

                        fromEmail = contactQuery.GetRealEmailByEnglishName(system, tenant, xxxDotCom);
                    }
                }
                string documentId=this.SendHtmlDocument(bytedata, DocumentFilingId, null, tenant, email, subject += " " + arinvoice.InvoiceNumber, null, null, userId, arinvoice.Id, LoggingObjectTableId, document.Id+","+ documentInterestReportId, null, fromEmail, null,loggedUserEmail);
                if (!string.IsNullOrEmpty(documentId))
                {
                    arinvoice.IsSigned = ARInvoiceSignedStatusValues.SignedAndSentByEmail;
                    repository.Update(arinvoice);
                    repository.SubmitChanges();
                }
                else
                {
                    string mess = "Error while sending signed invoice " + arinvoice.InvoiceNumber;
                    APInvoiceHelper.AddCommunicationLog("F", arinvoice, mess, "ARInvoice", arinvoice.Id, "Send Html Document Failed", tenant);
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(mess);
                    arinvoice.IsSigned = ARInvoiceSignedStatusValues.SignedButSendingByEmailFailed;
                    repository.Update(arinvoice);
                    repository.SubmitChanges();
                }
            }
            catch(Exception e)
            {
                arinvoice.IsSigned = ARInvoiceSignedStatusValues.SignedButSendingByEmailFailed;
                repository.Update(arinvoice);
                repository.SubmitChanges();
                APInvoiceHelper.AddCommunicationLog("F", arinvoice, e.Message, "ARInvoice", arinvoice.Id, "Send Invoice Failed", tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e, "General Exception in DocumentHelper");
            }
        }


        public string SendHtmlDocument(byte[] htmlData, string internalDocumentId, string externalDocumentId, int tenant, string toEmail, string subject, string cc, string bcc, string userId, string entityId, string objectTableId, string attachments, string entityReference, string from, string replyTo,string loggedUserEmail)
        {
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            DocumentOutRepository internalDocRep = new DocumentOutRepository(context);
            DocumentRepository documentRep = new DocumentRepository(context);



            DocumentOut internalDocument = internalDocRep.GetSingleDocumentOut(internalDocumentId, tenant);
            DocumentOutCopyRepository documentoutCopyRep = new DocumentOutCopyRepository(tenant);
            List<DocumentOutCopy> documentOutCopies = null;
            if (internalDocument != null)
            {
                if (internalDocument.DocumentsFiling.DocumentType.IsDocumentOneTimePrintLimited)
                {
                    documentOutCopies = documentoutCopyRep.GetDocumentOutCopyByDocumentOutId(internalDocument.Id, tenant);
                }

                internalDocument.IssuedDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                internalDocument.IssuedByUserId = userId;
                internalDocument.Issued = true;
                internalDocument.DocumentsFiling.UpdatedByUserId = userId;
                internalDocument.DocumentsFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                internalDocRep.Update(internalDocument);
            }
            Simplog.Data.CommonDataModel.EntityPOCOs.Document document = new Simplog.Data.CommonDataModel.EntityPOCOs.Document()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                Extension = "html",
                FileSize = Convert.ToInt32(htmlData.Length),
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant).ToString(),
                Folder = "docsout",
                HasFile = true,
            };
            documentRep.Add(document);
            documentRep.SubmitChanges();
            internalDocRep.SubmitChanges();


            try
            {
                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(document.Id + ".html", document.Folder);

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = htmlData.Length,


                };
                storageservice.Write(htmlData, fileInfo);

            }
            catch
            {
            }



            // Save Html to CommunicationLog

            CommunicationLog log = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                InOut = "O",
                To = toEmail,
                CC = cc,
                BCC = bcc,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                DocumentId = document.Id,
                CreatedByUserId = userId,
                EntityId = entityId,
                ObjectTableId = objectTableId,
                DocumentOutId = internalDocumentId,
                DocumentsFilingId = externalDocumentId,
                EntityReference = entityReference,
                Subject = subject,
                Tenant = tenant,
                LastStatusDateUTC = TenantServerConfigration.GetCurrentDateTime(tenant),
                CreateDateUTC = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationLogTypeCode = "E",
                CommunicationStatusTypeCode = "W",


            };

            if (!string.IsNullOrEmpty(from) && !string.IsNullOrWhiteSpace(from))
            {
                log.From = from;
            }
            if (!string.IsNullOrEmpty(replyTo) && !string.IsNullOrWhiteSpace(replyTo))
            {
                log.ReplyToList = replyTo;
            }




            context.CommunicationLogs.Add(log);
            try
            {
                context.SaveChanges();
            }
            catch (Exception eeee)
            { }


            if (attachments != null)
            {
                string[] attachmentsArray = attachments.Split(',');
                if (attachmentsArray.Count() != 0)
                {
                    foreach (string docId in attachmentsArray)
                    {
                        if (!String.IsNullOrEmpty(docId))
                        {
                            if (internalDocument != null)
                            {
                                DocumentOutCopy copy = null;
                                DocumentType documentType = internalDocument.DocumentsFiling.DocumentType;
                                if (documentOutCopies == null)
                                {
                                    copy = documentoutCopyRep.GetSingleDocumentOutCopy(docId);
                                }
                                if (copy != null)
                                {
                                    DocumentTypeCopyRepository documentTypeCopyRep = new DocumentTypeCopyRepository(context);
                                    DocumentTypeCopy typeCopy = documentTypeCopyRep.GetSingleDocumentTypeCopy(copy.DocumentTypeCopyId);
                                    DocumentTypeRepository documentTypeRep = new DocumentTypeRepository(context);
                                    documentType = documentTypeRep.GetSingleDocumentTypes(typeCopy.DocumentTypeId, tenant);

                                }

                                if (documentType.IsDocumentOneTimePrintLimited)
                                {
                                    if (copy == null)
                                    {
                                        copy = documentOutCopies.Where(d => d.Id == docId).FirstOrDefault();
                                    }
                                    if (copy != null && copy.DocumentTypeCopyId == documentType.LimitedPrintCopyId)
                                    {
                                        UserRepository userRep = new UserRepository(tenant);
                                        User printedBy = userRep.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, false);
                                        copy.LastPrintDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                        copy.LastPrintedByUserId = printedBy?.Id;
                                        documentoutCopyRep.Update(copy);
                                        documentoutCopyRep.SubmitChanges();
                                    }

                                }
                            }

                            CommunicationAttachment attachment = new CommunicationAttachment()
                            {
                                Id = IdCounter.GetNumber("CommunicationAttachment", tenant).ToString(),
                                CommunicationLogId = log.Id,
                                DocumentId = docId,
                                Tenant = tenant,
                            };

                            context.CommunicationAttachments.Add(attachment);
                        }
                    }

                    context.SaveChanges();
                }
            }
            try
            {
                DbQueueService queueservice = new DbQueueService("EmailQueue", tenant);
                Dictionary<string, string> emailQueueMessage = new Dictionary<string, string>() { { "CommunicationLogId", log.Id }, { "Tenant", tenant.ToString() } };
                
                queueservice.Send(emailQueueMessage, tenant);
            }
            catch (Exception ex)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "web role", null, ip);
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
            }




            return document.Id;
        }

        public string GetDocumentInterestReportId(string ARInvoiceId,int tenant)
        {
            ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
            DocumentRepository documentRepository = new DocumentRepository(commoncontext);
            DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(commoncontext);
            DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);
            InterestReportRepository interestReportRepository = new InterestReportRepository(tenant);
            InterestReport interestReport = interestReportRepository.GetSingleByARInvoiceId(ARInvoiceId, tenant);
            DocumentsFilingPM myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(interestReport.Id, tenant).FirstOrDefault();
            return   documentRepository.GetSingleDocument(tenant, myDocumentFilings?.DocumentId)?.Id;


        }
        public bool Sign(string documentOutId, int tenant, FullAccountingSettingPM accountingSettings)
        {

            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            ARInvoiceRepository repository = new ARInvoiceRepository(tenant);


            var documentsFiling = objectContext.DocumentsFilings.Where(doc => doc.Id == documentOutId).FirstOrDefault();
            ARInvoice invoice = repository.GetARInvoiceById(tenant, documentsFiling.EntityId).FirstOrDefault();

            if (invoice != null)
            {
                string contactEmail = this.IsSignatureHtmlPresentByBillToId(invoice.BillToId, tenant);
                if (!string.IsNullOrEmpty(contactEmail))
                {
                    this.CreatePdfDoc(documentsFiling, invoice.Id, invoice.Tenant, "ARInvoice", true);
                    if (this.isInterestReport && invoice.ARInvoiceTypeCode == "IT")
                    {
                        this.CreateDocumentInterestReport(invoice.Tenant, invoice.Id ,null);
                    }
                    this.StartSignPDFInvoice(invoice, invoice.Tenant, repository, contactEmail, accountingSettings);
                    return true;
                }
                return false;

            }
            return false;

        }

        public bool CheckPDFInvoiceInStorage_Outer(ARInvoice invoice, int tenant, ARInvoiceRepository repository)
        {
            IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
            FullAccountingSettingPM accountingSettings = query.GetFullAccountingSettingByTenant(tenant);
            string contactEmail = this.IsSignatureHtmlPresentByBillToId(invoice.BillToId, tenant);
            return this.CheckPDFInvoiceInStorage_Inner(invoice, tenant, repository, contactEmail, accountingSettings);
        }

        public bool CheckPDFInvoiceInStorage(string documentOutId, int tenant, FullAccountingSettingPM accountingSettings , string loggedContactId)
        {

            bool rv = false;

            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            ARInvoiceRepository repository = new ARInvoiceRepository(invoiceContext);


 

            var documentsFiling = objectContext.DocumentsFilings.Where(doc => doc.Id == documentOutId).FirstOrDefault();
            if (documentsFiling != null)
            {
                 ARInvoice invoice = repository.GetARInvoiceById(tenant, documentsFiling.EntityId).FirstOrDefault();

                if (invoice != null)
                {
                    string contactEmail = this.IsSignatureHtmlPresentByBillToId(invoice.BillToId, tenant);
                    if (!string.IsNullOrEmpty(contactEmail))
                    {
                        this.CreatePdfDoc(documentsFiling, invoice.Id, invoice.Tenant, "ARInvoice", true);
                        if (this.isInterestReport && invoice.ARInvoiceTypeCode == "IT")
                        {
                            this.CreateDocumentInterestReport(invoice.Tenant, invoice.Id, loggedContactId);
                        }
                        rv = this.CheckPDFInvoiceInStorage_Inner(invoice, invoice.Tenant, repository, contactEmail, accountingSettings);
                    }

                }
            }
            return rv;

        }

        private string IsSignatureHtmlPresentByBillToId(string Billto, int tenant)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

            if (string.IsNullOrEmpty(Billto)) return "";
            Simplog.Data.CommonDataModel.EntityPOCOs.Card myCard = (from card in objectContext.Cards
                                   where card.Id == Billto
                                   select card).FirstOrDefault();
            string email = "";
            if (myCard != null && !string.IsNullOrEmpty(myCard.EmailForSendingSingArinvoice))
            {
               email = objectContext.Contacts.Where(contact => contact.Id == myCard.EmailForSendingSingArinvoice).FirstOrDefault().Email;
            if (!string.IsNullOrEmpty(email))
               {
                    this.isInterestReport = myCard.SendingInterestReport != null ? true : false;
                   return email;
              }

            }
            return email;
        }

        private void CreatePdfDoc(DocumentsFiling documentsFiling, string Id, int tenant, string objectTableName, bool IsDigitalSign = false)
        {

            IExportDocumentHelper exportDocumentHelper = ContainerAccessor.Container.Resolve(typeof(IExportDocumentHelper), "ExportDocumentHelper", new ParameterOverride("", 1)) as IExportDocumentHelper;
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
            DocumentTypePM documentTypePM = documentTypeQuery.GetSinglePM(documentsFiling.DocumentTypeId, documentsFiling.Id, tenant);

            string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(tenant);
            var objectTable = objectTableRepository.GetObjectTableByName(objectTableName, tenant, true);


            string documentFileName = TranslateTextsClass.Translate(objectTableName, tenant, true);

            if (IsDigitalSign)
            {
                var doc = documentTypePM.DocumentTypeCopies.FirstOrDefault();
                string fileName = DetermineDocumentName(objectTableName, tenant, doc.Id);
                exportDocumentHelper.ExportDocument2Pdf(documentsFiling.DocumentTypeId, Id, objectTable.Id, null, null, documentsFiling.Id, tenant, doc.Id, resolveLoggingUserId, documentFileName);
            }
            else
            {
                bool multipleDocuments = documentTypePM.DocumentTypeCopies?.Count > 1;
                documentTypePM.DocumentTypeCopies.ForEach(doc =>
                {
                    string fileName = DetermineDocumentName(objectTableName, tenant, doc.Id, multipleDocuments);

                    exportDocumentHelper.ExportDocument2Pdf(documentsFiling.DocumentTypeId, Id, objectTable.Id, null, null, documentsFiling.Id, tenant, doc.Id, resolveLoggingUserId, fileName);
                });
            }
        }

        public static string DetermineDocumentName(string objectTableName, int tenant, string documentTypeCopyId, bool multipleDocuments = false)
        {
            try
            {
                string documentFileName = TranslateTextsClass.Translate(objectTableName, tenant, true);
                string fileName = documentFileName;
                if (multipleDocuments)
                {
                    DocumentTypeCopyRepository documentTypeCopyRep = new DocumentTypeCopyRepository(tenant);
                    DocumentTypeCopy documentTypeCopy = documentTypeCopyRep.GetSingleDocumentTypeCopy(documentTypeCopyId);
                    if (!string.IsNullOrEmpty(documentTypeCopy?.Name))
                    {
                        fileName = $"{documentFileName} ({documentTypeCopy.Name})";
                    }
                }
                return fileName;
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteWarning($"failed determine file name (documentTypeCopyId: {documentTypeCopyId}), error: {ex.Message}");
                return objectTableName;
            }
        }

        public void CreateDocumentInterestReport(int tenant, string arinvoiceId ,string loggedContactId)
        {
            ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
            DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
            DocumentRepository documentRepository = new DocumentRepository(commoncontext);
            DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(commoncontext);
            DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);
            DocumentTypeQueryService DocumentTypeService = new DocumentTypeQueryService(tenant);
            InterestReportRepository interestReportRepository = new InterestReportRepository(tenant);
            InterestReport interestReport = new InterestReport();

            string documentTypeId = DocumentTypeService.GetDocumentTypeByCode("ITDT", tenant).Id;
            interestReport = interestReportRepository.GetSingleByARInvoiceId(arinvoiceId, tenant);
            DocumentOutPM documentOutPM = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(interestReport.Id, null, documentTypeId, tenant);
            if (documentOutPM == null)
            {
                var LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("InterestReport");
                DocumentHelper documentHelper = new DocumentHelper();
                documentOutPM = documentHelper.CreateDocumentOut(documentTypeId, interestReport.Id, null, interestReport.ReportNumber, LoggingObjectTableId, tenant, loggedContactId, null);
            }
            var documentsFiling = commoncontext.DocumentsFilings.Where(doc => doc.Id == documentOutPM.Id).FirstOrDefault();
            DocumentsFilingPM myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(interestReport.Id, tenant).FirstOrDefault();
            if (string.IsNullOrEmpty(documentsFiling.DocumentId))
            {
                this.CreatePdfDoc(documentsFiling, interestReport.Id, tenant, "InterestReport");
            }
        }
    }

    public class CreateDocumentOutArgs
    {
        public string DocumentTypeId { get; set; }
        public string EntityId { get; set; }
        public string ChildEntityId { get; set; }
        public string ChildReference { get; set; }
        public string ObjectTableId { get; set; }
        public int Tenant { get; set; }
        public string DocumentTypeTemplateId { get; set; }

        public bool SignHSM { get; set; }
    }

}
