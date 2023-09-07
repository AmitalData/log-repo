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

namespace Logitude.BL.Helpers
{
    public class DocumentHelper
    {

        private readonly bool IsAutomation;
        private int Tenant;

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
                newDocumentFiling.SecurityId = newDocumentFiling.Id + RandomString(10);
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

      
        
        

        public void StartSignPDFInvoice(ARInvoice invocie, int tenant, ARInvoiceRepository repository)
        {

            try
            {
                IHSMSignFileService HSMSignFileService = ContainerAccessor.Container.Resolve(typeof(IHSMSignFileService), "HSMSignFileService", new ParameterOverride("", 1)) as IHSMSignFileService;


                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                DocumentRepository documentRepository = new DocumentRepository(commoncontext);
                DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(commoncontext);
                DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);
                TenantRepository tenantRepository = new TenantRepository();

                DocumentsFilingPM myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(invocie.Id, tenant).FirstOrDefault();
                Document document = documentRepository.GetSingleDocument(tenant, myDocumentFilings?.DocumentId);
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
                        .SignCustomsRequest(tenant, invocie.Id, filedata, document.FileName, vatNumber);
                    //invocie.IsSigned
                    if (signBytes != null)
                    {
                        storageservice.Write(signBytes, fileInfo);
                        this.HSMSignatureSucceeded(invocie, repository);
                       
                        
                    }
                }
                else
                {
                    throw new ArgumentNullException("There is no document");
                }
            }
            catch(HSMException ex)
            {
                this.HSMSignatureFailed(invocie, ex, repository);
                                 
            }
            catch (Exception ex)
            {
                //Logger.LogMe($"hSMSignFile({tenant},{invocieId})" + ex.ToString(), true, "CustomsHSMSignWR");
              //  ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "ProccessHSMSign-MarkExportSignTaskAsDone", "", null);


            }
        }

        private void HSMSignatureFailed(ARInvoice invocie,HSMException ex, ARInvoiceRepository repository)
        {
           invocie.IsSigned = "2";//FALID
            repository.Update(invocie);
            repository.SubmitChanges();
             this.CreateEvent("HSMF", invocie, "חתימת החשבונית לא  צלחה");
            this.SendEmailAlert("libby@amital.co.il","  חתימה בHSM נכשלה", " חתימת החשבונית נכשלה &ensp;&ensp;&ensp; חשבונית מספר"+invocie.InvoiceNumber+ "<br /><br />מצורפת השגיאה "+ex);
        }


        private void HSMSignatureSucceeded(ARInvoice invocie, ARInvoiceRepository repository)
        {
            invocie.IsSigned = "1";
            repository.Update(invocie);
            repository.SubmitChanges();
            this.CreateEvent("HSMS", invocie, "החשבונית נחתמה בהצלחה");
            this.SendEmailAlert("libby@amital.co.il", "  חתימה בHSM נכשלה", " חתימת החשבונית נכשלה &ensp;&ensp;&ensp; חשבונית מספר" + invocie.InvoiceNumber + "<br /><br />מצורפת השגיאה " );
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

        private void SendEmailAlert(string email, string subject, string body)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(Tenant);
            ContactRepository contactRepository = new ContactRepository(commonContext);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, Tenant);

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
                Tenant = Tenant,
            };
            Communications.AddEmailCommunicationLogQueue(emailParams, Tenant);
        }
    }
      
   

}
