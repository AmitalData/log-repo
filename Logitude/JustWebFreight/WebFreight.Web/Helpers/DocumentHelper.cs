using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class DocumentHelper
    {

        public DocumentOutPM CreateDocumentOut(string documentTypeId, string entityId, string childEntityId, string childReference, string objectTableId, int tenant, string userId = null)
        {
            try
            {
                ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
                DocumentOutRepository documentOutRepository = new DocumentOutRepository(objectContext);
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(objectContext);
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(documentOutRepository);

                DocumentType documentType = documentTypeRepository.GetSingleDocumentTypes(documentTypeId, tenant);
                string documentTemplateId = null;
                string emailTemplateId = null;

                documentTemplateId = documentType.DocumentTypeDefaultReportTemplateId;
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
                DocumentOut newDocument = new DocumentOut() { EmailTemplateId = emailTemplateId, DocumentTemplateId = documentTemplateId, Tenant = tenant, Issued = false, };
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
                List<Document> documentsList = documentRepository.GetDocuments(tenant.Id).Where(d =>!d.IsEncrypted).ToList();
     
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
                                string cmd = "Update Documents set IsEncrypted=1 , HasFile =1 where id =" + "'"+ document.Id+ "' and tenant ="+ document.Tenant;
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
                        AzureLog.SaveLogsInStorage(data, "P", DateTime.Now, "", "", 0, "", "Encryption Document", null);
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

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }
    }

}
