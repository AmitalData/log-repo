using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
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
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
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
using System.Text;
using System.Transactions;
using System.Web;
using WebFreight.Web;
using User = Simplog.Data.CommonDataModel.EntityPOCOs.User;
using WebFreight.Web;
using Logitude.XSD.CW_API.ABM;
using Logitude.BL.InvoiceModel.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using DocumentsFiling = Simplog.Data.CommonDataModel.EntityPOCOs.DocumentsFiling;
using Customer = Simplog.Data.CommonDataModel.EntityPOCOs.Customer;
using DocumentType = Simplog.Data.CommonDataModel.EntityPOCOs.DocumentType;
using Contact = Simplog.Data.CommonDataModel.EntityPOCOs.Contact;

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


                //---------------------------------------- islam
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
             // newDocumentFiling.SecurityId = newDocumentFiling.Id + RandomString(10);
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
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        //ve.PropertyName, ve.ErrorMessage);

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

                //return Convert.ToHexString(hashBytes); // .NET 5 +

                //Convert the byte array to hexadecimal string prior to.NET 5

                StringBuilder sb = new System.Text.StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)

                {

                    sb.Append(hashBytes[i].ToString("X2"));

                }

                return sb.ToString();

            }

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
                        //AzureLog.SaveLogsInStorage(data, "P", DateTime.Now, "", "", 0, "", "Encryption Document", null);
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

      
        
        

        public void StartSignPDFInvoice(ARInvoice invocie, int tenant, ARInvoiceRepository repository,string contactEmail, FullAccountingSettingPM accountingSettings)
        {

            try
            {
                 
                IHSMSignFileService HSMSignFileService = ContainerAccessor.Container.Resolve(typeof(IHSMSignFileService), "HSMSignFileService", new ParameterOverride("", 1)) as IHSMSignFileService;
                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);

                DocumentRepository documentRepository = new DocumentRepository(commoncontext);
                DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(commoncontext);
                DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);
                TenantRepository tenantRepository = new TenantRepository(tenant);

                DocumentsFilingPM myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(invocie.Id, tenant).FirstOrDefault();
                Document document = documentRepository.GetSingleDocument(tenant, myDocumentFilings?.DocumentId);
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
                        .SignCustomsRequest(tenant, invocie.Id, filedata, document.FileName, vatNumber, loggedcontact?.Id, accountingSettings);
                    //invocie.IsSigned

                    if (signBytes != null)
                    {
                        storageservice.Write(signBytes, fileInfo);
                        APInvoiceHelper.AddCommunicationLog("D", invocie, signBytes.ToString(), "ARInvoice", invocie?.Id, "signBytes is ok", Tenant);

                        this.HSMSignatureSucceeded(invocie, repository, contactEmail, document, myDocumentFilings.Id);
                       
                        
                    }
                    else
                    {
                        APInvoiceHelper.AddCommunicationLog("F", invocie, signBytes.ToString(), "ARInvoice", invocie?.Id, "signBytes is empty", Tenant);

                        throw new HSMException($"{invocie?.Id},response: Result.Content.ReadAsByteArrayAsync().Result is empty", "Fails");
                    }
                }
                else
                {
                    throw new ArgumentNullException("There is no document");
                }
            }
            catch(HSMException ex)
            {
                APInvoiceHelper.AddCommunicationLog("F", invocie, ex.Message, "ARInvoice", invocie.Id, "HSM Signature Failed", tenant);

                this.HSMSignatureFailed(invocie, ex, repository);
                                 
            }
            catch (Exception ex)
            {
                APInvoiceHelper.AddCommunicationLog("F", invocie, ex.Message, "ARInvoice", invocie.Id, "HSM Signature Failed", tenant);

                //Logger.LogMe($"hSMSignFile({tenant},{invocieId})" + ex.ToString(), true, "CustomsHSMSignWR");
                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "ProccessHSMSign-MarkExportSignTaskAsDone", "", null);


            }
        }

        private void HSMSignatureFailed(ARInvoice invocie,HSMException ex, ARInvoiceRepository repository)
        {
           invocie.IsSigned = "2";//FALID
            repository.Update(invocie);
            repository.SubmitChanges();
             this.CreateEvent("HSMF", invocie, "חתימת החשבונית לא  צלחה"+ ex);
            this.SendEmailAlert("ohad@amital.co.il", "  חתימה בHSM נכשלה", " חתימת החשבונית נכשלה &ensp;&ensp;&ensp; חשבונית מספר"+invocie.InvoiceNumber+ "<br /><br />מצורפת השגיאה "+ex, invocie.Tenant, invocie.Id);
        }


        private void HSMSignatureSucceeded(ARInvoice invocie, ARInvoiceRepository repository,string contactEmail,Document document,string  DocumentFilingId )
        {

            invocie.IsSigned = "1";
            repository.Update(invocie);
            repository.SubmitChanges();
            this.CreateEvent("HSMS", invocie, "החשבונית נחתמה בהצלחה");
         //   this.SendEmailAlert("libby@amital.co.il", "  חתימה בHSM נכשלה", " חתימת החשבונית נכשלה &ensp;&ensp;&ensp; חשבונית מספר" + invocie.InvoiceNumber + "<br /><br />מצורפת השגיאה " );
            this.SendToEmailContact(contactEmail, invocie, document, DocumentFilingId, repository, invocie.Tenant);

        }
        private void CreateEvent(string eventCode,ARInvoice arinvocie, string Notes = null)
        {
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(Tenant);
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = arinvocie.Id,
                Tenant = arinvocie.Tenant,
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


        private void SendToEmailContact(string email, ARInvoice arinvocie,Document document,string DocumentFilingId, ARInvoiceRepository repository,int tenant)
        {
            string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
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
                Document documentInterestReport = new Document();
                string documentInterestReportId = "";
                if (this.isInterestReport && arinvocie.ARInvoiceTypeCode=="IT")
                {
                    documentInterestReportId = this.GetDocumentInterestReportId(arinvocie.Id, tenant);
                  
                }
                string documentId=this.SendHtmlDocument(bytedata, DocumentFilingId, null, tenant, email, "חשבונית חתומה", null, null, userId, arinvocie.Id, LoggingObjectTableId, document.Id+","+ documentInterestReportId, null, null, null);
                if (!string.IsNullOrEmpty(documentId))
                {
                    arinvocie.IsSigned = "3";
                    repository.Update(arinvocie);
                    repository.SubmitChanges();
                }
                else
                {
                    arinvocie.IsSigned = "4";
                    repository.Update(arinvocie);
                    repository.SubmitChanges();
                }
            }
            catch(Exception e)
            {
                arinvocie.IsSigned = "4";
                repository.Update(arinvocie);
                repository.SubmitChanges();
            }
        }
        public string SendHtmlDocument(byte[] htmlData, string internalDocumentId, string externalDocumentId, int tenant, string toEmail, string subject, string cc, string bcc, string userId, string entityId, string objectTableId, string attachments, string entityReference, string from, string replyTo)
        {
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            //ShipmentsContext shipmentsContext = new ShipmentsContext();
            DocumentOutRepository internalDocRep = new DocumentOutRepository(context);
            DocumentRepository documentRep = new DocumentRepository(context);



            //DocumentType documentType = (from d in context.DocumentTypes
            //                             where d.Id == documentTypeId && d.Tenant == tenant
            //                             select d).FirstOrDefault();


            DocumentOut internalDocument = internalDocRep.GetSingleDocumentOut(internalDocumentId, tenant);
            DocumentOutCopyRepository documentoutCopyRep = new DocumentOutCopyRepository(tenant);
            List<DocumentOutCopy> documentOutCopies = null;
            if (internalDocument != null)
            {
                if (internalDocument.DocumentsFiling.DocumentType.IsDocumentOneTimePrintLimited)
                {
                    documentOutCopies = documentoutCopyRep.GetDocumentOutCopyByDocumentOutId(internalDocument.Id, tenant);
                }

                //internalDocument.DocumentId = document.Id;
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

            //if (!WebFreightEntryPoint.UsingAzure)
            //{
            //    try
            //    {
            //        string filePath = Server.MapPath(".");
            //        filePath += "\\UserUploads\\";
            //        filePath += document.Id;
            //        filePath += ".html";

            //        FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            //        BinaryWriter bw = new BinaryWriter(fs);
            //        bw.Write(htmlData);
            //        bw.Close();
            //    }
            //    catch
            //    {
            //    }
            //}
            //else // In Azure
            //{
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
                //string filename = document.Id + ".html";
                // CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));


                //using (Stream memstream = blobfile.OpenWrite())
                //{
                //    memstream.Write(htmlData, 0, htmlData.Length);
                //    memstream.Close();
                //    //memstream.Write(htmlData, 0, htmlData.Length);
                //    //blobfile.UploadFromStream(memstream);
                //}
            }
            catch
            {
            }

            // }



            // Send Html Document by email
            // SendHtmlDocumentByEmail(ToEmail, Subject, CC, filePath);
            //============================

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



            //EventTracer.CreateTraceEvent(new TraceEvent(), "CRCR", currency.Tenant, contact.Id, currency.Id, null, "Currency", null, null, false);

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
                                        string email = HttpContext.Current.User.Identity.Name;
                                        UserRepository userRep = new UserRepository(tenant);
                                        User printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, email, tenant, false);
                                        copy.LastPrintDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                        copy.LastPrintedByUserId = printedBy.Id;
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
            }




            return document.Id;
        }

        public string GetDocumentInterestReportId(string ARInvocieId,int tenant)
        {
            ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
            DocumentRepository documentRepository = new DocumentRepository(commoncontext);
            DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(commoncontext);
            DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);
            InterestReportRepository interestReportRepository = new InterestReportRepository(tenant);
            InterestReport interestReport = interestReportRepository.GetSingleByARInvoiceId(ARInvocieId, tenant);
            DocumentsFilingPM myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(interestReport.Id, tenant).FirstOrDefault();
            return   documentRepository.GetSingleDocument(tenant, myDocumentFilings?.DocumentId)?.Id;


        }
        public void Sign(string documentOutId, int tenant, FullAccountingSettingPM accountingSettings)
        {

            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            ARInvoiceRepository repository = new ARInvoiceRepository(tenant);


            var documentsFiling = objectContext.DocumentsFilings.Where(doc => doc.Id == documentOutId).FirstOrDefault();
            ARInvoice invocie = repository.GetARInvoiceById(tenant, documentsFiling.EntityId).FirstOrDefault();

            if (invocie != null)
            {
                string contactEmail = this.IsSignatureHtmlPresentByBillToId(invocie.BillToId, tenant);
                if (!string.IsNullOrEmpty(contactEmail))
                {
                    this.CreatePdfDoc(documentsFiling, invocie.Id, invocie.Tenant, "ARInvoice");
                    if (this.isInterestReport && invocie.ARInvoiceTypeCode == "IT")
                    {
                        this.CreateDocumentInterestReport(invocie.Tenant, invocie.Id);
                    }
                    this.StartSignPDFInvoice(invocie, invocie.Tenant, repository, contactEmail, accountingSettings);
                }

            }

        }

        private string IsSignatureHtmlPresentByBillToId(string Billto, int tenant)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

            if (string.IsNullOrEmpty(Billto)) return "";
            Customer myCustomer = (from customer in objectContext.Customers
                                   where customer.Id == Billto
                                   // join cardContact in objectContext.CardContacts on card.Id equals cardContact.CardId
                                   select customer).FirstOrDefault();
            string email = "";
            if (myCustomer != null && !string.IsNullOrEmpty(myCustomer.EmailForSendingSingArinvoice))
            {
                email = objectContext.Contacts.Where(contact => contact.Id == myCustomer.EmailForSendingSingArinvoice).FirstOrDefault().Email;
                if (!string.IsNullOrEmpty(email))
                {
                    this.isInterestReport = myCustomer.SendingInterestReport != null ? true : false;
                    return email;
                }

            }
            return email;
        }

        private void CreatePdfDoc(DocumentsFiling documentsFiling, string Id, int tenant, string objectTableName)
        {

            IExportDocumentHelper exportDocumentHelper = ContainerAccessor.Container.Resolve(typeof(IExportDocumentHelper), "ExportDocumentHelper", new ParameterOverride("", 1)) as IExportDocumentHelper;
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
            DocumentTypePM documentTypePM = documentTypeQuery.GetSinglePM(documentsFiling.DocumentTypeId, documentsFiling.Id, tenant);

            string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(tenant);
            var objectTable = objectTableRepository.GetObjectTableByName(objectTableName, tenant, true);

            documentTypePM.DocumentTypeCopies.ForEach(doc =>
            {
                exportDocumentHelper.ExportDocument2Pdf(documentsFiling.DocumentTypeId, Id, objectTable.Id, null, null, documentsFiling.Id, tenant, doc.Id, resolveLoggingUserId);
            });

        }

        public void CreateDocumentInterestReport(int tenant, string arinvocieId)
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
            interestReport = interestReportRepository.GetSingleByARInvoiceId(arinvocieId, tenant);
            DocumentOutPM documentOutPM = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(interestReport.Id, null, documentTypeId, tenant);
            if (documentOutPM == null)
            {
                var LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("InterestReport");
                DocumentHelper documentHelper = new DocumentHelper();
                documentOutPM = documentHelper.CreateDocumentOut(documentTypeId, interestReport.Id, null, interestReport.ReportNumber, LoggingObjectTableId, tenant, null, null);
            }
            var documentsFiling = commoncontext.DocumentsFilings.Where(doc => doc.Id == documentOutPM.Id).FirstOrDefault();
            DocumentsFilingPM myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(interestReport.Id, tenant).FirstOrDefault();
            if (string.IsNullOrEmpty(documentsFiling.DocumentId))
            {
                this.CreatePdfDoc(documentsFiling, interestReport.Id, tenant, "InterestReport");
            }
        }
    }
      
   

}
