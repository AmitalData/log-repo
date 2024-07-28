using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Transactions;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.Validators;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.HybridMapping;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using System.ServiceModel.Description;
using System.Web;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Utils;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DocumentInWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DocumentInWcfService.svc or DocumentInWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DocumentInWcfService : IDocumentInWcfService
    {

        //public static void Configure(ServiceConfiguration config)
        //{

        //    //ServiceEndpoint se = new ServiceEndpoint(new ContractDescription("IDocumentInWcfService"), new BasicHttpBinding(), new EndpointAddress("basic"));
        //    //se.Behaviors.Add(new MyEndpointBehavior());
        //    //config.AddServiceEndpoint(se);

        //    //config.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true  , HttpGetBinding = new    });
        //    //config.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });
        //}
        //public static void Configure(ServiceConfiguration config)
        //{
        //    config.LoadFromConfiguration(ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = @"c:\sharedConfig\MyConfig.config" }, ConfigurationUserLevel.None));
        //}
        //public static void Configure(ServiceConfiguration config)
        //{
        //    // Enable “Add Service Reference” support 
        //    config.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
        //    // set up support for http, https, net.tcp, net.pipe 
        //    config.EnableProtocol(new BasicHttpBinding());
        //    config.EnableProtocol(new BasicHttpBinding());
        //    config.EnableProtocol(new NetTcpBinding());
        //    config.EnableProtocol(new NetNamedPipeBinding());
        //    // add an extra BasicHttpBinding endpoint at http:///basic 
        //    config.AddServiceEndpoint(typeof(IService1), new BasicHttpBinding(), "basic");
        //} 
        public Response Upsert(DocumentsFilingPM documentDataPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(documentDataPM.Tenant);
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    response = UpsertDocumentData(documentDataPM);

                    scope.Complete();

                    return response;
                }


            }

            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }

                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
        }

        public Response UpsertDocumentData(DocumentsFilingPM entityPM)
        {
            Response response = new Response();

            try
            {

                ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(entityPM.Tenant);
                DocumentsFilingService service = new DocumentsFilingService(commonContext, entityPM.Tenant);
                DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(commonContext);
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);
                //DocumentRepository documentRepository = new DocumentRepository(commonContext);
                //DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commonContext);
                //ObjectTabelRepository objectTableRepository = new ObjectTabelRepository(webFreightContext);
                //DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(commonContext);
                //UserRepository userRepository = new UserRepository(commonContext);
                if (entityPM.IsAttachment)
                {
                    entityPM.Code = "111";
                }

                entityPM.IsHybrid = true;


                ClassLevelValidator validationClass = new ClassLevelValidator("DocumentsFiling", entityPM.Tenant) { IsHybrid = true };

                if (string.IsNullOrEmpty(entityPM.Id) && !entityPM.IsAttachment)
                {
                    response.HasError = true;
                    response.ErrorMessage += "Id field is required" + Environment.NewLine;
                }



                //if (entityPM.FileData == null)
                //{
                //    response.HasError = true;
                //    response.ErrorMessage += "FileData field is required" + Environment.NewLine;

                //}

                if (string.IsNullOrEmpty(entityPM.FileExtension))
                {
                    response.HasError = true;
                    response.ErrorMessage += "FileExtension field is required" + Environment.NewLine;

                }


                response = DocumentsFilingHybridMapping.MapEntityToLogitude(entityPM);


                if (!validationClass.IsValid(entityPM, entityPM, null))
                {
                    response.HasError = true;
                    response.ErrorMessage += validationClass.GetErrorMessage(entityPM, null) + Environment.NewLine;
                }

                if (!response.HasError)
                {
                    if (string.IsNullOrEmpty(entityPM.Folder))
                    {
                        entityPM.Folder = "docsin";
                    }

                    entityPM.HasFile = true;
                    entityPM.ReceivedDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    entityPM.Received = true;

                    DocumentsFilingPM documentInPM = documentsFilingQuery.GetSinglePM(entityPM.Id, entityPM.Tenant);

                    if (documentInPM == null)
                    {
                        service.SetChangeSet(entityPM.DocumentsFilingMetaDataValues);
                        service.Create(entityPM, entityPM.FileData, null, entityPM.FileData == null ? true : false);

                        //ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                        //Contact contact = contactRep.GetSingleContact(entityPM.CreatedByUserId, entityPM.Tenant);
                        //if (contact != null)
                        //{
                        //    ActivityLog.SendTotangoContactActivity(contact.Email, "OUTLOOK COONECTION", "Attachments - from Outlook", entityPM.Tenant, false, null);
                        //}

                        commonContext.SaveChanges();
                    }
                    else
                    {
                        entityPM.Id = documentInPM.Id;
                        if (!string.IsNullOrEmpty(documentInPM.DocumentId) && string.IsNullOrEmpty(entityPM.DocumentId))
                        {
                            entityPM.DocumentId = documentInPM.DocumentId;

                        }

                        foreach (DocumentsFilingMetaDataValuePM metadatavalue in documentInPM.DocumentsFilingMetaDataValues)
                        {
                            metadatavalue.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            entityPM.DocumentsFilingMetaDataValues.Add(metadatavalue);
                        }


                        service.SetChangeSet(entityPM.DocumentsFilingMetaDataValues);

                        service.Update(entityPM, entityPM.FileData, null, entityPM.FileData == null ? true : false);

                        commonContext.SaveChanges();


                    }

                    response.Result = entityPM.Id;
                    response.Result2 = entityPM.SecurityId;
                }


                return response;
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }

                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }

        }

        public DocumentsFilingPM GetDocumentDataByExternalId(string externalId, int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }


            try
            {
                response = new Response();

                SecurityUtility.AuthenticationOnTenant(tenant);
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                DocumentsFilingRepository documentInRepository = new DocumentsFilingRepository(commonContext);
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(documentInRepository);


                DocumentsFilingPM documentInPM = documentsFilingQuery.GetSinglePM(externalId, tenant);
                if (documentInPM != null)
                {
                    documentInPM = DocumentsFilingHybridMapping.MapEntityToHybrid(documentInPM);

                    string fileName = documentInPM.DocumentId + "." + documentInPM.FileExtension;

                    //string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), documentInPM.Folder);
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = documentInPM.DocumentId,
                        FolderName = documentInPM.Folder,
                        Extension = documentInPM.FileExtension,
                        Tenant = tenant,
                        FileSize = documentInPM.FileSize,

                    };
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    documentInPM.FileData = storageservice.Read(fileInfo);
                    
                    

                }

                return documentInPM;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }

        public Response UpsertDocumentData(DocumentDataPM documentDataPM, bool batch)
        {
            Response response = new Response();
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(documentDataPM.Tenant);
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ICommonDataContext commonContext = CommonDataContext.GetContext(documentDataPM.Tenant);
                    IWebFreightContext webFreightContext = WebFreightContext.GetContext(documentDataPM.Tenant);
                    DocumentsFilingService service = new DocumentsFilingService(commonContext, documentDataPM.Tenant);
                    DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(commonContext);
                    DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);
                    DocumentRepository documentRepository = new DocumentRepository(commonContext);
                    DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commonContext);
                    ObjectTableRepository objectTableRepository = new ObjectTableRepository(webFreightContext);
                    DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(commonContext);
                    UserRepository userRepository = new UserRepository(commonContext);

                    ObjectTable table = null;
                    ObjectTable childtable = null;

                    //if (string.IsNullOrEmpty(documentDataPM.ExternalCode))
                    //{
                    //    response.HasError = true;
                    //    response.ErrorMessage = "ExternalCode field is required";
                    //    return response;
                    //}

                    if (!string.IsNullOrEmpty(documentDataPM.ObjectTableName))
                    {
                        table = objectTableRepository.GetObjectTableByName(documentDataPM.ObjectTableName, 0, false);
                        if (table == null)
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ObjectTableName field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (!string.IsNullOrEmpty(documentDataPM.ChildObjectTableName))
                    {
                        childtable = objectTableRepository.GetObjectTableByName(documentDataPM.ChildObjectTableName, 0, false);
                        if (childtable == null)
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ChildObjectTableName field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }


                    User user = null;
                    if (!string.IsNullOrEmpty(documentDataPM.UserEmail))
                    {
                        user = userRepository.GetSingleUserByCodeOrEmail(documentDataPM.UserEmail, documentDataPM.UserEmail, documentDataPM.Tenant, false);
                        if (user == null)
                        {
                            response.HasError = true;
                            response.ErrorMessage = "UserEmail field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "UserEmail field is required.";
                        return response;
                    }

                    //if (documentDataPM.FileData == null)
                    //{
                    //    response.HasError = true;
                    //    response.ErrorMessage = "FileData field is required.";
                    //    return response;
                    //}

                    DocumentType documentType = null;
                    if (!string.IsNullOrEmpty(documentDataPM.DocumentTypeId))
                    {
                        documentType = documentTypeRepository.GetSingleDocumentTypeByCode(documentDataPM.DocumentTypeId, documentDataPM.Tenant);
                        if (documentType == null)
                        {
                            response.HasError = true;
                            response.ErrorMessage = "DocumentTypeId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "DocumentTypeId field is required.";
                        return response;
                    }



                    DocumentsFilingPM documentInPM = null;
                    if (!string.IsNullOrEmpty(documentDataPM.ExternalCode))
                    {
                        documentInPM = documentsFilingQuery.GetDocumentsFilingByDocumentCode(documentDataPM.ExternalCode, documentDataPM.Tenant);
                    }
                    if (documentInPM == null)
                    {

                        documentInPM = new DocumentsFilingPM()
                        {
                            DirectionCode = "I",
                            Tenant = documentDataPM.Tenant,
                            Code = documentDataPM.Code,
                            EntityId = documentDataPM.EntityId,
                            ChildEntityId = documentDataPM.ChildEntityId,
                            ChildEntityReference = documentDataPM.ChildEntityReference,
                            DocumentTypeId = documentType.Id,
                            ObjectTableId = table != null ? table.Id : null,
                            ChildObjectTableId = childtable != null ? childtable.Id : null,
                            CreatedByUserId = user.Id,
                            CreateDate = documentDataPM.ReceivedDate != null ? documentDataPM.ReceivedDate.Value : TenantServerConfigration.GetCurrentDateTime(documentDataPM.Tenant),
                            Notes = documentDataPM.Notes,
                            OwnerId = user.Id,
                            UpdatedByUserId = user.Id,
                            UpdateDate = documentDataPM.ReceivedDate != null ? documentDataPM.ReceivedDate.Value : TenantServerConfigration.GetCurrentDateTime(documentDataPM.Tenant),
                            Description = documentDataPM.Description,
                            ExternalEntityName = documentDataPM.ExternalEntityName,
                            ExternalEntityReference = documentDataPM.ExternalEntityReference,
                            EntityReference = documentDataPM.EntityReference,
                            FileExtension = documentDataPM.FileExtension,
                            FileSize = documentDataPM.FileData != null ? Convert.ToInt32(documentDataPM.FileData.Length) : documentDataPM.FileSize,//Convert.ToInt32(documentDataPM.FileData.Length),
                            Folder = "docsin",
                            HasFile = true,
                            FileName = !string.IsNullOrEmpty(documentDataPM.FileName) ? documentDataPM.FileName : documentType.Name,
                            FileData = documentDataPM.FileData,
                            IsHybrid = true,
                            IsAttachment = true,
                            Received = true,
                            ReceivedDate = documentDataPM.ReceivedDate != null ? documentDataPM.ReceivedDate.Value : TenantServerConfigration.GetCurrentDateTime(documentDataPM.Tenant),
                            ReceivedByUserId = user.Id,
                            DocumentId = documentDataPM.DocumentId
                        };


                        service.Create(documentInPM, documentDataPM.FileData, null, documentDataPM.FileData == null ? true : false);

                        commonContext.SaveChanges();


                    }
                    else
                    {
                        documentInPM.UpdateDate = documentDataPM.ReceivedDate != null ? documentDataPM.ReceivedDate.Value : TenantServerConfigration.GetCurrentDateTime(documentDataPM.Tenant);
                        documentInPM.Notes = documentDataPM.Notes;
                        documentInPM.UpdatedByUserId = user.Id;
                        documentInPM.EntityId = documentDataPM.EntityId;
                        documentInPM.ChildEntityId = documentDataPM.ChildEntityId;
                        documentInPM.ChildEntityReference = documentDataPM.ChildEntityReference;
                        documentInPM.ObjectTableId = table != null ? table.Id : null;
                        documentInPM.ChildObjectTableId = childtable != null ? childtable.Id : null;
                        documentInPM.DocumentTypeId = documentType.Id;
                        documentInPM.Description = documentDataPM.Description;

                        documentInPM.ExternalEntityName = documentDataPM.ExternalEntityName;
                        documentInPM.ExternalEntityReference = documentDataPM.ExternalEntityReference;
                        documentInPM.EntityReference = documentDataPM.EntityReference;

                        documentInPM.FileName = !string.IsNullOrEmpty(documentDataPM.FileName) ? documentDataPM.FileName : documentType.Name;
                        documentInPM.FileSize = documentDataPM.FileData != null ? Convert.ToInt32(documentDataPM.FileData.Length) : documentDataPM.FileSize;//Convert.ToInt32(documentDataPM.FileData.Length);
                        documentInPM.FileExtension = documentDataPM.FileExtension;

                        documentInPM.IsHybrid = true;

                        //foreach (DocumentsFilingMetaDataValuePM metadatavalue in documentInPM.DocumentsFilingMetaDataValues)
                        //{
                        //    metadatavalue.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                        //}


                        //foreach (DocumentsFilingMetaDataValuePM metadatavalue in documentDataPM.DocumentsFilingMetaDataValuePMs)
                        //{
                        //    metadatavalue.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        //    documentInPM.DocumentsFilingMetaDataValues.Add(metadatavalue);
                        //}


                        //  service.SetChangeSet(documentInPM.DocumentsFilingMetaDataValues);

                        service.Update(documentInPM, documentDataPM.FileData, null, documentDataPM.FileData == null ? true : false);

                        commonContext.SaveChanges();


                    }

                    scope.Complete();

                    response.Result = documentInPM.Id;
                    response.Result2 = documentInPM.SecurityId;

                    return response;
                }
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }

                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
        }

        public Response GetStorageContainerConnectionString(int tenant)
        {
            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);

                response.Result = DocumentFileUploadHelper.GetTempStorageSasWrite(tenant);
                response.Result2 = DocumentFileUploadHelper.GetStorageEncryptionKey(tenant);

                return response;
            }
            catch (Exception ex)
            {
                return UpdateResponseException(response, ex);
            }
        }

        private Response UpdateResponseException(Response response, Exception ex)
        {
            response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
            response.HasError = true;
            response.ErrorMessage = ex.Message;
            response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                response.ErrorMessage += Environment.NewLine + ex.StackTrace;
            }

            return response;
        }

        public Response UploadDocumentFileDataFromStorage(int tenant, string blobname, string DocumentId = null)
        {
            Response response = new Response();
            if(string.IsNullOrEmpty(blobname))
                return UpdateResponseException(response, new ArgumentException("blobname or DocumentId is null or empty"));

            Logger.LogDebug("UploadDocumentFileDataFromStorage start", "tenant: " + tenant + " blobname: " + blobname + " DocumentId: " + DocumentId);

            SecurityUtility.AuthenticationOnTenant(tenant);
            response = DocumentFileUploadHelper.AddDocumentAndSendToInternalStorage(tenant, blobname, DocumentId);

            Logger.LogDebug("UploadDocumentFileDataFromStorage finish", "tenant: " + tenant + " blobname: " + blobname + " DocumentId: " + DocumentId + " response: " + response.HasError + ", error message: " + response.ErrorMessage);

            return response;
        }

        public Response UploadDocumentFileData(byte[] buffer, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string FileNameWithExtention, string DocumentId)
        {

            Response response = new Response();

            if (string.IsNullOrEmpty(FileNameWithExtention))
            {
                response.HasError = true;
                response.ErrorMessage = "Invalid file name!";
                return response;
            }

            string[] fileParams = FileNameWithExtention.Split('.');
            string finalFileName = FileNameWithExtention.Substring(0, FileNameWithExtention.LastIndexOf('.'));
            string fileextension = fileParams[fileParams.Length - 1];

            if (string.IsNullOrEmpty(fileextension))
            {
                response.HasError = true;
                response.ErrorMessage = "Invalid file extension!";
            }

            if (string.IsNullOrEmpty(finalFileName))
            {
                response.HasError = true;
                response.ErrorMessage = "Invalid file name!";
            }

            if (response.HasError)
            {
                return response;
            }

            DocumentFileUploadHelper documentFileUploadHelper = new DocumentFileUploadHelper(this.GetType().Name);
            return documentFileUploadHelper.UploadDocumentFileData(buffer, fileSize, sentBytes, blockIdsList, bufferNumber, tenant, FileNameWithExtention, DocumentId);

        }

        public DocumentsFilingPM GetDocumentDataByExternalIdWithoutBinaray(string externalId, int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }


            try
            {
                response = new Response();

                SecurityUtility.AuthenticationOnTenant(tenant);
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                DocumentsFilingRepository documentInRepository = new DocumentsFilingRepository(commonContext);
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(documentInRepository);


                DocumentsFilingPM documentInPM = documentsFilingQuery.GetSinglePM(externalId, tenant);
                if (documentInPM != null)
                {
                    documentInPM = DocumentsFilingHybridMapping.MapEntityToHybrid(documentInPM); 
                }

                return documentInPM;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }

        //public Response UploadDocumentFileData(byte[] buffer, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, string DoumentFilingId, int tenant)
        //{
        //    Response response = new Response();

        //    try
        //    {

        //        ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
        //        IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
        //        DocumentsFilingService service = new DocumentsFilingService(commonContext, tenant);
        //        DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(commonContext);
        //        DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);
        //        DocumentRepository docRepository = new DocumentRepository(tenant);
        //        var documentsFiling = documentsFilingQuery.GetSinglePM(DoumentFilingId, tenant);
        //        Document document = docRepository.GetSingleDocument(tenant, documentsFiling.DocumentId);
        //        if (documentsFiling != null)
        //        {
        //            fileName = documentsFiling.FileName;
        //            if (sentBytes < fileSize)
        //            {
        //                fileNameAndExtension = document.Id + "." + document.Extension;//fileNameAndExtension = BuidDocument(tenant, documentsFiling.Id, fileSize);
        //                fileName = fileNameAndExtension;
        //                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileNameAndExtension.ToLower(), "docsin");
        //                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
        //                Logitude.Server.Tools.BlobServiceReference.Response res = storageservice.WriteBlock(buffer, fileSize, sentBytes, blockIdsList, bufferNumber, filePath, fileName.ToLower());
        //                if (res.HasError)
        //                {
        //                    response.HasError = res.HasError;
        //                    response.ErrorMessage = res.ErrorMessage;
        //                }
        //                else
        //                {
        //                    response.Result = "In Progress"; 
        //                }
                       
        //            }
        //            else if (sentBytes == fileSize)
        //            {
        //                fileNameAndExtension = document.Id + "." + document.Extension; //fileNameAndExtension = BuidDocument(tenant, documentsFiling.Id, fileSize);
        //                fileName = fileNameAndExtension;
        //                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileNameAndExtension.ToLower(), "docsin");
        //                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
        //                Logitude.Server.Tools.BlobServiceReference.Response res = storageservice.WriteBlock(buffer, fileSize, sentBytes, blockIdsList, bufferNumber, filePath, fileName.ToLower());
        //                if (res.HasError)
        //                {
        //                    response.HasError = res.HasError;
        //                    response.ErrorMessage = res.ErrorMessage;
        //                }
        //                else
        //                { 
        //                    response.Result = "Done";
        //                }
        //            } 
        //        }
        //        else
        //        {
        //            response.HasError = true;
        //            response.ErrorMessage = "There is no DocumentFiling, Use UpsertDocumentData To Add one then Upload the file again.";
        //        }
        //        return response;
        //    }

        //    catch (System.Data.Entity.Validation.DbEntityValidationException e)
        //    {
        //        string Error = "";
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);

        //                Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
        //            }
        //        }

        //        response.HasError = true;
        //        response.ErrorMessage = Error;

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
        //        response.HasError = true;
        //        response.ErrorMessage = ex.Message;
        //        response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
        //        if (!string.IsNullOrEmpty(ex.StackTrace))
        //        {
        //            response.ErrorMessage += Environment.NewLine + ex.StackTrace;
        //        }
        //        return response;
        //    }

        //}
        //string fileNameAndExtension;
        //private static string fileName;
        //string documentIdAndExtension;
        //private string BuidDocument(int tenant, string externalDocumentId, long fileSize)
        //{
        //    try
        //    {
        //        DocumentsFilingRepository externalDocumentRepository = new DocumentsFilingRepository(tenant);
        //        DocumentRepository docRepository = new DocumentRepository(tenant);

        //        DocumentsFiling externalDocument = externalDocumentRepository.GetSingleDocumentsFiling(externalDocumentId, tenant);
        //        Document document = docRepository.GetSingleDocument(tenant, externalDocument.DocumentId);

        //        string fileextension = fileName.Split('.')[1].ToString();
        //        string finalFileName = externalDocumentId + "." + fileextension;

        //        string realFileName = null;
        //        if (!string.IsNullOrEmpty(fileName))
        //        {
        //            realFileName = fileName.Split('.')[0];
        //        }

        //        //if (document == null)
        //        //{
        //        //    document = new Document()
        //        //    {
        //        //        CreateDate = DateTime.Now,
        //        //        Extension = fileextension,
        //        //        FileSize = Convert.ToInt32(fileSize),
        //        //        Tenant = Convert.ToInt32(externalDocument.Tenant),
        //        //        Id = IdCounter.GetNumber("Document", tenant).ToString(),//externalDocumentId,
        //        //        HasFile = true,
        //        //        Folder = "docsin",
        //        //        FileName = realFileName,
        //        //    };
        //        //    docRepository.Add(document);
        //        //}
        //        //else
        //        //{
        //        //    document.CreateDate = DateTime.Now;
        //        //    document.Extension = fileextension;
        //        //    document.FileSize = Convert.ToInt32(fileSize);
        //        //    document.Tenant = Convert.ToInt32(externalDocument.Tenant);
        //        //    document.HasFile = true;
        //        //    document.Folder = "docsin";
        //        //    document.FileName = realFileName;
        //        //    docRepository.Update(document);
        //        //}

        //        //docRepository.SubmitChanges();
        //        fileNameAndExtension = document.Id + "." + document.Extension;
        //        externalDocument.DocumentId = document.Id;
        //        externalDocumentRepository.Update(externalDocument);
        //        externalDocumentRepository.SubmitChanges();

        //        return fileNameAndExtension;

        //    }

        //    catch (Exception e)
        //    {
        //        string ip = "";
        //        if (HttpContext.Current != null && HttpContext.Current.Request != null)
        //        {
        //            ip = HttpContext.Current.Request.UserHostAddress;
        //        }
        //    }
        //    return documentIdAndExtension;
        //}

        //public DocumentsFilingPM GetDocumentDataByExternalIdWithoutBinaray(string externalId, int tenant, ref Response response)
        //{
        //    if (CacheManager.CacheWrapper == null)
        //    {
        //        CacheManager.CacheWrapper = new MockCacheWrapper();
        //    }
        //    try
        //    {
        //        response = new Response();
        //        SecurityUtility.AuthenticationOnTenant(tenant);
        //        ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
        //        DocumentsFilingRepository documentInRepository = new DocumentsFilingRepository(commonContext);
        //        DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(documentInRepository);
        //        DocumentsFilingPM documentInPM = documentsFilingQuery.GetSinglePM(externalId, tenant);
        //        if (documentInPM != null)
        //        {
        //            documentInPM = DocumentsFilingHybridMapping.MapEntityToHybrid(documentInPM);
        //        }
        //        return documentInPM;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
        //        response.HasError = true;
        //        response.ErrorMessage = ex.Message;
        //        response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
        //        if (!string.IsNullOrEmpty(ex.StackTrace))
        //        {
        //            response.ErrorMessage += Environment.NewLine + ex.StackTrace;
        //        }
        //        return null;
        //    }
        //}
    }
}
