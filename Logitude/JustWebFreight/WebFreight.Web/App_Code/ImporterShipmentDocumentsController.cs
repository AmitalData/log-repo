using Logitude.BL.CommonDataModel.CodePropertiesMapping;
using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.DataContracts;
using Simplog.Server.Infrastructure;
using Simplog.Data.ShipmentsModel.Repositories;

namespace WebFreight.Web.App_Code
{
    public class ImporterShipmentDocumentsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        public bool GetIfNew(string id, int tenant)//, int importertenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("DocumentsFiling", "READ", tenant); 
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            //var Temp = documentsFilingQuery.GetSinglePM(id, tenant);
            //if (Temp != null)
            //{
            //    if (Temp.CustomerDocumentId != null)
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        if (Temp.ForwarderDocumentId != null)
            //        {
            //            documentsFilingQuery = new DocumentsFilingQuery(importertenant);
            //            var doc = documentsFilingQuery.GetSinglePM(Temp.CustomerDocumentId, importertenant);//by forworderid and pass Temp.Id
            //            if (doc != null)
            //            {
            //                return false;
            //            }
            //        }
            //        return true;
            //    }
            //}
            //else
            //{
            //    return true;
            //}
            var DocumentFilingPM = documentsFilingQuery.GetSinglePMByForwarderId(id, tenant);
            if (DocumentFilingPM == null)
            {
                DocumentFilingPM = documentsFilingQuery.GetSinglePM(id, tenant);
                if (DocumentFilingPM != null)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        // POST api/<controller>
        public HttpResponseMessage Post(DocumentsFilingAM EntityAM)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(EntityAM.ImporterTenant);
                //SecurityUtility.CheckContactFeature("DocumentsFiling", "NEW", EntityAM.ImporterTenant);
                APIException Result = null;
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(EntityAM.ImporterTenant);
                var EntityPM = new DocumentsFilingPM() { Tenant = EntityAM.ImporterTenant };
                Result = MapEntityAMToEntityPM(EntityAM, EntityPM);

                //shipmentQuery.GetSinglePMWithoutComposition(Shipment.Id, Shipment.Tenant);

                bool IsNewLog = false;
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(EntityPM.Tenant);
                APILogsService apiLogsService = new APILogsService(webFreightContext, EntityPM.Tenant);
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(EntityPM.Tenant);
                HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(EntityAM.ImporterTenant);
                HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(hybridPartnerRepository);
                HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePMByPartnerTenant(EntityAM.Tenant);

                var aPILogsRepository = new APILogsRepository(webFreightContext);
                APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, EntityPM.Tenant);
                APILogsPM LogPM;
                if (Log == null)
                {
                    IsNewLog = true;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("DocumentsFiling", EntityPM.Tenant, true);
                    LogPM = new APILogsPM()
                    {
                        Id = IdCounter.GetNumber("APILogs", EntityPM.Tenant),
                        CorrelationId = CorrelationId,
                        CreateDate = DateTime.Now,
                        CreateDateUTC = DateTime.UtcNow,
                        Direction = "I",
                        EntityId = EntityPM.Id,
                        LastUpdateDate = DateTime.Now,
                        LastUpdateDateUTC = DateTime.UtcNow,
                        NumberOfRetries = 1,
                        ObjectTableId = Objecttable.Id,
                        ExpirationDate = DateTime.Now.AddDays(90),
                        Refrence = EntityAM.Code,
                        Status = "I",
                        Tenant = EntityPM.Tenant
                    };
                }
                else
                {
                    IsNewLog = false;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("DocumentsFiling", EntityPM.Tenant, true);
                    LogPM = new APILogsPM()
                    {
                        Id = Log.Id,
                        CorrelationId = Log.CorrelationId,
                        CreateDate = Log.CreateDate,
                        CreateDateUTC = Log.CreateDateUTC,
                        Direction = Log.Direction,
                        EntityId = Log.EntityId,
                        LastUpdateDate = Log.LastUpdateDate,
                        LastUpdateDateUTC = Log.LastUpdateDateUTC,
                        NumberOfRetries = Log.NumberOfRetries++,
                        ObjectTableId = Log.ObjectTableId,
                        ExpirationDate = Log.ExpirationDate,
                        Refrence = Log.Refrence,
                        Status = "I",
                        Tenant = Log.Tenant,

                    };
                }
                if (Partner != null)
                {
                    LogPM.PartnerName = Partner.Name + "(" + Partner.Id + ")";
                }
                LogPM.Subject = "Insert Documents To Importer Tenant";
                if (IsNewLog)
                {
                    apiLogsService.Create(LogPM);
                }
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Inserting Document To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(EntityAM), null, null, "");

                // Insert
                try
                {
                    if (EntityAM.FileInfo != null)
                    {
                        var DocId = UploadDocumentByte(EntityAM.FileInfo);
                        return Request.CreateResponse(HttpStatusCode.OK, DocId);
                    }
                    else
                    {
                        if (Result == null)
                        {
                            string systemEmail = "system@tenant" + EntityPM.Tenant + ".com";
                            ContactQuery contactQuery = new ContactQuery(EntityPM.Tenant);
                            var loggedContact = contactQuery.GetContactByNameAndTenant(systemEmail, EntityPM.Tenant, true);
                            if (loggedContact == null)
                            {
                                loggedContact = contactQuery.GetContactByEmailOnly(systemEmail, EntityPM.Tenant);
                            }
                            EntityPM.CreatedByUserId = loggedContact.Id;
                            EntityPM.OwnerId = loggedContact.Id;
                            EntityPM.UpdatedByUserId = loggedContact.Id;
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Inserting Document To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(EntityAM), null, null, "");
                            ICommonDataContext objectContext = CommonDataContext.GetContext(EntityPM.Tenant);
                            DocumentsFilingService documentsFilingService = new DocumentsFilingService(objectContext, EntityPM.Tenant);
                            documentsFilingService.SetChangeSet(EntityPM.DocumentsFilingMetaDataValues);
                            var DocumentFilingPM = documentsFilingQuery.GetDocumentsFilingByDocumentType(EntityPM.DocumentTypeId, EntityPM.ObjectTableId, EntityPM.EntityId, EntityPM.Tenant);
                            if (DocumentFilingPM != null && DocumentFilingPM.IsSharedWithCustomer == false && EntityAM.IsSharedWithCustomer == true)
                            {
                                DocumentFilingPM = null;
                            }
                            if (DocumentFilingPM != null && !DocumentFilingPM.HasFile)
                            {
                                Result = MapEntityAMToEntityPM(EntityAM, DocumentFilingPM);
                                //DocumentService documentsservice = new DocumentService(objectContext, DocumentFilingPM.Tenant);
                                DocumentRepository documentsRepo = new DocumentRepository(objectContext);
                                Document Doc = documentsRepo.GetSingleDocument(DocumentFilingPM.Tenant, DocumentFilingPM.DocumentId);
                                if (Result == null)
                                {
                                    //Doc.Id = IdCounter.GetNumber("Document", DocumentFilingPM.Tenant).ToString();
                                    Doc.FileName = DocumentFilingPM.FileName;
                                    Doc.FileSize = DocumentFilingPM.FileSize;
                                    Doc.HasFile = true;
                                    Doc.Extension = DocumentFilingPM.FileExtension;
                                    documentsRepo.Update(Doc);
                                    documentsRepo.SubmitChanges();
                                    documentsFilingService.Update(DocumentFilingPM, null, null, true);//, DocumentFilingPM.FileData);
                                }
                                else
                                {
                                    var Failmsg = "Inserting Document Faild " + DateTime.Now;
                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(Result), null, "");
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, Result);
                                }
                            }
                            else
                            {
                                documentsFilingService.Create(EntityPM, null, null, true);// EntityPM.FileData);
                            }
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, "Insert Document To Importer Tenant Done Successfully " + DateTime.Now, null, null, null, "");

                            return Request.CreateResponse(HttpStatusCode.OK, EntityPM.Id != null ? EntityPM.Id : DocumentFilingPM.Id);
                        }
                        else
                        {

                            var Failmsg = "Inserting Document Faild " + DateTime.Now;
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(Result), null, "");
                            return Request.CreateResponse(HttpStatusCode.BadRequest, Result);
                        }
                    }


                }
                catch (Exception ex)
                {
                    string ErrorMessage = "";

                    if (ex.GetType().Name == "DbEntityValidationException")
                    {
                        var exception = ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors.FirstOrDefault();
                        if (exception != null)
                        {
                            if (exception.ValidationErrors.FirstOrDefault() != null)
                            {
                                ErrorMessage = exception.ValidationErrors.FirstOrDefault().ErrorMessage;
                            }
                            else
                            {
                                ErrorMessage = ex.Message;
                            }
                        }
                        var apiException = new APIException()
                        {
                            ErrorType = ex.GetType().Name,
                            ErrorMessage = ErrorMessage
                        };
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Inserting Document To Importer Tenant Faild " + DateTime.Now, null, LogitudeXmlSerializer.SerializeObjectToXmlString(apiException), null, "");

                        return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);

                    }
                    else
                    {
                        var apiException = new APIException()
                        {
                            ErrorType = ex.GetType().Name,
                            ErrorMessage = ex.Message
                        };
                        string errorMessage = ex.Message + Environment.NewLine;

                        if (ex.InnerException != null)
                        {

                            errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                        }

                        errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Update Document At Importer Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

                        return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        // PUT api/<controller>/5
        public HttpResponseMessage Put(DocumentsFilingAM EntityAM)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(EntityAM.ImporterTenant);
                //SecurityUtility.CheckContactFeature("DocumentsFiling", "UPDATE", EntityAM.ImporterTenant);
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(EntityAM.ImporterTenant);
                DocumentsFilingPM ImporterDocumentFilingPM = documentsFilingQuery.GetSinglePM(EntityAM.CustomerDocumentId, EntityAM.ImporterTenant);
                if (ImporterDocumentFilingPM == null)
                {
                    ImporterDocumentFilingPM = documentsFilingQuery.GetSinglePMByForwarderId(EntityAM.ForwarderDocumentId, EntityAM.ImporterTenant);
                }
                APIException Result = MapEntityAMToEntityPM(EntityAM, ImporterDocumentFilingPM);

                bool IsNewLog = false;
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(ImporterDocumentFilingPM.Tenant);
                APILogsService apiLogsService = new APILogsService(webFreightContext, ImporterDocumentFilingPM.Tenant);
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(ImporterDocumentFilingPM.Tenant);
                HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(EntityAM.ImporterTenant);
                HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(hybridPartnerRepository);
                HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePMByPartnerTenant(EntityAM.Tenant);

                var aPILogsRepository = new APILogsRepository(webFreightContext);
                APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, ImporterDocumentFilingPM.Tenant);
                APILogsPM LogPM;
                if (Log == null)
                {
                    IsNewLog = true;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("DocumentsFiling", ImporterDocumentFilingPM.Tenant, true);
                    LogPM = new APILogsPM()
                    {
                        Id = IdCounter.GetNumber("APILogs", ImporterDocumentFilingPM.Tenant),
                        CorrelationId = CorrelationId,
                        CreateDate = DateTime.Now,
                        CreateDateUTC = DateTime.UtcNow,
                        Direction = "I",
                        EntityId = ImporterDocumentFilingPM.Id,
                        LastUpdateDate = DateTime.Now,
                        LastUpdateDateUTC = DateTime.UtcNow,
                        NumberOfRetries = 1,
                        ObjectTableId = Objecttable.Id,
                        ExpirationDate = DateTime.Now.AddDays(90),
                        Refrence = EntityAM.Code,
                        Status = "I",
                        Tenant = ImporterDocumentFilingPM.Tenant
                    };
                }
                else
                {
                    IsNewLog = false;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("DocumentsFiling", ImporterDocumentFilingPM.Tenant, true);
                    LogPM = new APILogsPM()
                    {
                        Id = Log.Id,
                        CorrelationId = Log.CorrelationId,
                        CreateDate = Log.CreateDate,
                        CreateDateUTC = Log.CreateDateUTC,
                        Direction = Log.Direction,
                        EntityId = Log.EntityId,
                        LastUpdateDate = Log.LastUpdateDate,
                        LastUpdateDateUTC = Log.LastUpdateDateUTC,
                        NumberOfRetries = Log.NumberOfRetries++,
                        ObjectTableId = Log.ObjectTableId,
                        ExpirationDate = Log.ExpirationDate,
                        Refrence = Log.Refrence,
                        Status = "I",
                        Tenant = Log.Tenant,

                    };
                }
                if (Partner != null)
                {
                    LogPM.PartnerName = Partner.Name;
                }
                LogPM.Subject = "Insert Documents To Importer Tenant";
                if (IsNewLog)
                {
                    apiLogsService.Create(LogPM);
                }
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start updating Document To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(EntityAM), null, null, "");

                try
                {
                    if (EntityAM.FileInfo != null)
                    {
                        var DocId = UploadDocumentByte(EntityAM.FileInfo, ImporterDocumentFilingPM);
                        if (DocId == "Error")
                        {
                            var apiException = new APIException()
                            {
                                ErrorType = "Uploading Document Error",
                                ErrorMessage = "There was an error occured while uploading the document"
                            };
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Update Document At Importer Tenant Faild " + DateTime.Now, null, LogitudeXmlSerializer.SerializeObjectToXmlString(apiException), null, "");

                            return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, DocId);
                        }

                    }
                    else
                    {
                        if (Result == null)
                        {
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Updating Document at Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(EntityAM), null, null, "");

                            ICommonDataContext objectContext = CommonDataContext.GetContext(ImporterDocumentFilingPM.Tenant);
                            DocumentsFilingService documentsFilingService = new DocumentsFilingService(objectContext, ImporterDocumentFilingPM.Tenant);
                            documentsFilingService.SetChangeSet(ImporterDocumentFilingPM.DocumentsFilingMetaDataValues);
                            var DocumentFilingPM = documentsFilingQuery.GetDocumentsFilingByDocumentType(ImporterDocumentFilingPM.DocumentTypeId, ImporterDocumentFilingPM.ObjectTableId, ImporterDocumentFilingPM.EntityId, ImporterDocumentFilingPM.Tenant);

                            if (DocumentFilingPM != null && !DocumentFilingPM.HasFile)
                            {
                                Result = MapEntityAMToEntityPM(EntityAM, DocumentFilingPM);

                                if (Result == null)
                                {
                                    if (DocumentFilingPM.FileData == null)
                                    {
                                        documentsFilingService.Update(DocumentFilingPM, null, null, true);//, null);//
                                    }
                                    else
                                    {
                                        DocumentRepository documentsRepo = new DocumentRepository(objectContext);
                                        Document Doc = documentsRepo.GetSingleDocument(DocumentFilingPM.Tenant, DocumentFilingPM.DocumentId);
                                        Doc.FileName = DocumentFilingPM.FileName;
                                        Doc.FileSize = DocumentFilingPM.FileSize;
                                        Doc.HasFile = true;
                                        Doc.Extension = DocumentFilingPM.FileExtension;
                                        documentsRepo.Update(Doc);
                                        documentsRepo.SubmitChanges();
                                        documentsFilingService.Update(DocumentFilingPM, null, null, true);//, DocumentFilingPM.FileData);
                                    }
                                    DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepo = new DocumentsMetaDataTypeRepository(DocumentFilingPM.Tenant);
                                    var LBF = DocumentsMetaDataTypeRepo.GetSingleDocumentsMetaDataTypeByCode("LBF", DocumentFilingPM.Tenant);
                                    if (DocumentFilingPM.DocumentsFilingMetaDataValues != null && LBF != null)
                                    {
                                        var LBFValue = DocumentFilingPM.DocumentsFilingMetaDataValues.Where(a => a.DocumentsMetaDataTypeId == LBF.Id).FirstOrDefault();
                                        if (LBFValue != null && !string.IsNullOrEmpty(LBFValue.MetaDataValue))
                                        {
                                            var shipmentRepository = new ShipmentRepository(DocumentFilingPM.Tenant);
                                            var MyNumber = LBFValue.MetaDataValue.Substring(1);
                                            var shipmentPM = shipmentRepository.GetSingleShipmentByShipmentNumber(MyNumber, DocumentFilingPM.Tenant);
                                            if (shipmentPM != null)
                                            {
                                                bool IsEntityHasDocs = documentsFilingQuery.IsEntityHasDocs(shipmentPM.Id, DocumentFilingPM.Tenant);
                                                if (IsEntityHasDocs == false)
                                                {
                                                    shipmentPM.IsCancelled = true;
                                                    shipmentRepository.Update(shipmentPM);
                                                    shipmentRepository.SubmitChanges();
                                                }
                                            }

                                          
                                        }
                                    }
                                   
                                }
                                else
                                {
                                    var Failmsg = "Inserting Document Faild " + DateTime.Now;
                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(Result), null, "");
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, Result);
                                }
                            }
                            else
                            {
                                if (ImporterDocumentFilingPM.FileData == null)
                                {
                                    documentsFilingService.Update(ImporterDocumentFilingPM, null,null,true);
                                }
                                else
                                {
                                    documentsFilingService.Update(ImporterDocumentFilingPM, ImporterDocumentFilingPM.FileData, null, true);
                                }
                                DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepo = new DocumentsMetaDataTypeRepository(ImporterDocumentFilingPM.Tenant);
                                var LBF = DocumentsMetaDataTypeRepo.GetSingleDocumentsMetaDataTypeByCode("LBF", ImporterDocumentFilingPM.Tenant);
                                if (ImporterDocumentFilingPM.DocumentsFilingMetaDataValues != null && LBF != null)
                                {
                                    var LBFValue = ImporterDocumentFilingPM.DocumentsFilingMetaDataValues.Where(a => a.DocumentsMetaDataTypeId == LBF.Id).FirstOrDefault();
                                    if (LBFValue != null && !string.IsNullOrEmpty(LBFValue.MetaDataValue))
                                    {
                                        var shipmentRepository = new ShipmentRepository(ImporterDocumentFilingPM.Tenant);
                                        var MyNumber = LBFValue.MetaDataValue.Substring(1);
                                        var shipmentPM = shipmentRepository.GetSingleShipmentByShipmentNumber(MyNumber, ImporterDocumentFilingPM.Tenant);
                                        if (shipmentPM != null)
                                        {
                                            bool IsEntityHasDocs = documentsFilingQuery.IsEntityHasDocs(shipmentPM.Id, ImporterDocumentFilingPM.Tenant);
                                            if (IsEntityHasDocs == false)
                                            {
                                                shipmentPM.IsCancelled = true;
                                                shipmentRepository.Update(shipmentPM);
                                                shipmentRepository.SubmitChanges();
                                            }
                                        }


                                    }
                                }
                            }
                          

                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, "Updating Document To Importer Tenant Done Successfully " + DateTime.Now, null, null, null, "");

                            return Request.CreateResponse(HttpStatusCode.OK, ImporterDocumentFilingPM.Id);
                        }
                        else
                        {

                            var Failmsg = "Inserting Document Faild " + DateTime.Now;
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(Result), null, "");
                            return Request.CreateResponse(HttpStatusCode.BadRequest, Result);
                        }
                    }

                }
                catch (Exception ex)
                {
                    string ErrorMessage = "";

                    if (ex.GetType().Name == "DbEntityValidationException")
                    {
                        var exception = ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors.FirstOrDefault();
                        if (exception != null)
                        {
                            if (exception.ValidationErrors.FirstOrDefault() != null)
                            {
                                ErrorMessage = exception.ValidationErrors.FirstOrDefault().ErrorMessage;
                            }
                            else
                            {
                                ErrorMessage = ex.Message;
                            }
                        }
                        var apiException = new APIException()
                        {
                            ErrorType = ex.GetType().Name,
                            ErrorMessage = ErrorMessage
                        };
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Update Document At Importer Tenant Faild " + DateTime.Now, null, LogitudeXmlSerializer.SerializeObjectToXmlString(apiException), null, "");

                        return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                    }
                    else
                    {
                        var apiException = new APIException()
                        {
                            ErrorType = ex.GetType().Name,
                            ErrorMessage = ex.Message
                        };
                        string errorMessage = ex.Message + Environment.NewLine;

                        if (ex.InnerException != null)
                        {

                            errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                        }

                        errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Update Document At Importer Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

                        return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);

                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private string UploadDocumentByte(FileInformation FileInfo, DocumentsFilingPM documentsFilingPM = null)
        {
            DocumentFileUploadHelper documentFileUploadHelper = new DocumentFileUploadHelper();
            var response = documentFileUploadHelper.UploadDocumentFileData(FileInfo.buffer, FileInfo.FileSize, FileInfo.SentSize, FileInfo.BlockIdsList, FileInfo.BufferNumber, FileInfo.Tenant, FileInfo.FileName, FileInfo.DocumentId, documentsFilingPM);
            if (!response.HasError)
            {
                return response.Result;
            }
            else
            {
                return "Error";
            }

        }

        public HttpResponseMessage Delete(string id)
        {
            try
            {
                var temp = id.Split(',');
                string DocId = temp[0];
                int Tenant = int.Parse(temp[1]);
                SecurityUtility.AuthenticationOnTenant(Tenant);

                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(Tenant);
                DocumentsFilingPM DocumentFilingPM = documentsFilingQuery.GetSinglePM(DocId, Tenant);

                if (DocumentFilingPM != null)
                {
                    bool IsNewLog = false;
                    string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                    IWebFreightContext webFreightContext = WebFreightContext.GetContext(DocumentFilingPM.Tenant);
                    APILogsService apiLogsService = new APILogsService(webFreightContext, DocumentFilingPM.Tenant);
                    ObjectTableRepository objectTabelRepository = new ObjectTableRepository(DocumentFilingPM.Tenant);
                    #region APILogs
                    var aPILogsRepository = new APILogsRepository(webFreightContext);
                    APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, DocumentFilingPM.Tenant);
                    APILogsPM LogPM;
                    if (Log == null)
                    {
                        IsNewLog = true;
                        var Objecttable = objectTabelRepository.GetObjectTableByName("DocumentFiling", DocumentFilingPM.Tenant, true);
                        LogPM = new APILogsPM()
                        {
                            Id = IdCounter.GetNumber("APILogs", DocumentFilingPM.Tenant),
                            CorrelationId = CorrelationId,
                            CreateDate = DateTime.Now,
                            CreateDateUTC = DateTime.UtcNow,
                            Direction = "I",
                            EntityId = DocumentFilingPM.Id,
                            LastUpdateDate = DateTime.Now,
                            LastUpdateDateUTC = DateTime.UtcNow,
                            NumberOfRetries = 1,
                            ObjectTableId = Objecttable.Id,
                            ExpirationDate = DateTime.Now.AddDays(90),
                            Refrence = DocumentFilingPM.Code,
                            Status = "I",
                            Tenant = DocumentFilingPM.Tenant
                        };
                    }
                    else
                    {
                        IsNewLog = false;
                        var Objecttable = objectTabelRepository.GetObjectTableByName("DocumentFiling", DocumentFilingPM.Tenant, true);
                        LogPM = new APILogsPM()
                        {
                            Id = Log.Id,
                            CorrelationId = Log.CorrelationId,
                            CreateDate = Log.CreateDate,
                            CreateDateUTC = Log.CreateDateUTC,
                            Direction = Log.Direction,
                            EntityId = Log.EntityId,
                            LastUpdateDate = Log.LastUpdateDate,
                            LastUpdateDateUTC = Log.LastUpdateDateUTC,
                            NumberOfRetries = Log.NumberOfRetries++,
                            ObjectTableId = Log.ObjectTableId,
                            ExpirationDate = Log.ExpirationDate,
                            Refrence = Log.Refrence,
                            Status = "I",
                            Tenant = Log.Tenant,

                        };
                    }
                    #endregion
                    LogPM.Subject = "Delete Document in Importer Tenant";
                    if (IsNewLog)
                    {
                        apiLogsService.Create(LogPM);
                    }

                    try
                    {
                        var msg = "Start deleting document At Importer Tenant " + DateTime.Now;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, LogPM.Status, 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(DocumentFilingPM), null, null, "");

                        if (DocumentFilingPM != null)
                        {
                            DocumentFilingPM.IsDeleted = true;
                            DocumentFilingPM.DontAddToQueue = true;
                            ContactRepository contactRepository = new ContactRepository(DocumentFilingPM.Tenant);
                            var User = contactRepository.GetSingleContactByEmail("system@tenant" + DocumentFilingPM.Tenant + ".com", DocumentFilingPM.Tenant, true);
                            ICommonDataContext objectContext = CommonDataContext.GetContext(DocumentFilingPM.Tenant);
                            DocumentsFilingService documentsFilingService = new DocumentsFilingService(objectContext, DocumentFilingPM.Tenant);
                            documentsFilingService.Update(DocumentFilingPM, null, User.Id, true);
                            msg = "Document deleted successfully " + DateTime.Now;
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(DocumentFilingPM), null, null, "");

                        }
                        return Request.CreateResponse(HttpStatusCode.OK, "Ok");
                    }
                    catch (Exception ex)
                    {
                        var apiException = new APIException()
                        {
                            ErrorType = ex.GetType().Name,
                            ErrorMessage = ex.Message
                        };
                        string errorMessage = ex.Message + Environment.NewLine;

                        if (ex.InnerException != null)
                        {

                            errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                        }

                        errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Cancle Shipment At Importer Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

                        return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private APIException MapEntityAMToEntityPM(DocumentsFilingAM EntityAM, DocumentsFilingPM EntityPM)
        {
            APIException Responce = new APIException();
            if (!string.IsNullOrEmpty(EntityAM.ObjectTableName))
            {
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(EntityAM.ImporterTenant);
                var Objecttable = objectTabelRepository.GetObjectTableByName(EntityAM.ObjectTableName, EntityAM.ImporterTenant, true);
                EntityPM.ObjectTableId = Objecttable.Id;
            }
            EntityPM.IsDeleted = EntityAM.IsDeleted;
            EntityPM.ExternalEntityReference = EntityAM.ExternalCode;
            if (!string.IsNullOrEmpty(EntityAM.ForwarderDocumentId))
            {
                EntityPM.ForwarderDocumentId = EntityAM.ForwarderDocumentId;
            }
            if (!string.IsNullOrEmpty(EntityAM.DocumentId))
            {
                EntityPM.DocumentId = EntityAM.DocumentId;
            }
            if (EntityAM.IsRequested && !string.IsNullOrEmpty(EntityAM.ForwarderDocumentId))
            {
                EntityPM.Notes = EntityAM.Notes;
            }
            if (!string.IsNullOrEmpty(EntityAM.EntityNumber))
            {
                ShipmentQuery shipmentQuery = new ShipmentQuery(EntityAM.ImporterTenant);
                ShipmentPM Shipment = shipmentQuery.GetSingleShipmentPMByNumber(EntityAM.EntityNumber, EntityAM.ImporterTenant);
                EntityPM.EntityId = Shipment.Id;
            }
            if (!string.IsNullOrEmpty(EntityAM.Description))
            {
                EntityPM.Description = EntityAM.Description;
            }
            if (EntityAM.FileData != null && EntityAM.FileData.Length > 0)
            {
                EntityPM.FileData = EntityAM.FileData;
            }
            if (EntityAM.DocumentsFilingMetaDataValues != null)
            {
                foreach (var item in EntityAM.DocumentsFilingMetaDataValues)
                {
                    
                    var DocMetaDataTypeId = DocumentTypeMetaDataTypePropertiesMapping.GetDocumentTypeMetaDataTypeProperties(EntityAM.ImporterTenant, item.DocumentsMetaDataType);
                    if (!string.IsNullOrEmpty(DocMetaDataTypeId))
                    {
                        var ValuesQuery = new DocumentsFilingMetaDataValueQuery(EntityAM.ImporterTenant);
                        var MyChangeSetOp = item.ChangeSetOp;
                        DocumentsFilingMetaDataValuePM MYValue = null;
                        if (!string.IsNullOrEmpty(EntityAM.CustomerDocumentId))
                        {
                            MYValue = ValuesQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTypeTenant(EntityAM.CustomerDocumentId, DocMetaDataTypeId, EntityAM.ImporterTenant);
                            
                        }
                        if (MYValue != null)
                        {
                            //MyChangeSetOp = ChangeSetOperation.Update;
                            //MYValue.MetaDataValue = item.MetaDataValue;
                            if (EntityPM.DocumentsFilingMetaDataValues != null)
                            {
                                var CurData = EntityPM.DocumentsFilingMetaDataValues.Where(a => a.Id == MYValue.Id).FirstOrDefault();// = new List<DocumentsFilingMetaDataValuePM>(); ;
                                if (CurData != null)
                                {
                                    CurData.ChangeSetOp = ChangeSetOperation.Update;
                                    CurData.MetaDataValue = item.MetaDataValue;
                                    CurData.DocumentsMetaDataTypeId = DocMetaDataTypeId;
                                }
                            }
                            else
                            {
                                EntityPM.DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValuePM>();
                                MYValue.ChangeSetOp = ChangeSetOperation.Update;
                                MYValue.MetaDataValue = item.MetaDataValue;
                                MYValue.DocumentsMetaDataTypeId = DocMetaDataTypeId;
                                EntityPM.DocumentsFilingMetaDataValues.Add(MYValue);
                            }
                        }
                        else
                        {
                            if (EntityPM.DocumentsFilingMetaDataValues == null)
                            {
                                EntityPM.DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValuePM>(); ;
                            }
                            EntityPM.DocumentsFilingMetaDataValues.Add(new DocumentsFilingMetaDataValuePM()
                            {
                                Tenant = EntityAM.ImporterTenant,
                                MetaDataValue = item.MetaDataValue,
                                DocumentsMetaDataTypeId = DocMetaDataTypeId,
                                ChangeSetOp = MyChangeSetOp
                            });
                        } 
                    }
                    else
                    {
                        Responce.ErrorType = "Validation Error";
                        Responce.ErrorMessage = "DocumentTypeMetaDataType field doesn't exist in the database, insert this entity before using it.";
                        return Responce;
                    }

                }

            }
            if (!string.IsNullOrEmpty(EntityAM.Extension))
            {
                EntityPM.FileExtension = EntityAM.Extension;
            }
            if (!string.IsNullOrEmpty(EntityAM.SignersList))
            {
                EntityPM.SignersList = EntityAM.SignersList;
            }
            EntityPM.IsRequested = EntityAM.IsRequested;
            EntityPM.IsDigitallySigned = EntityAM.IsDigitallySigned;
            EntityPM.DirectionCode = "I";
            EntityPM.FileSize = EntityAM.FileSize;
            EntityPM.FileName = EntityAM.FileName;
            EntityPM.DontAddToQueue = true;
            EntityPM.IsSharedWithCustomer = EntityAM.IsSharedWithCustomer;

            if (EntityAM.DocumentType != null)
            {
                var DocumentTypeId = DocumentTypeCodePropertiesMapping.GetDocumentTypeFromDocumentTypeProperties(EntityAM.ImporterTenant, EntityAM.DocumentType);
                if (!string.IsNullOrEmpty(DocumentTypeId))
                {
                    EntityPM.DocumentTypeId = DocumentTypeId;
                }
                else
                {
                    Responce.ErrorType = "Validation Error";
                    Responce.ErrorMessage = "DocumentTypeId field doesn't exist in the database, insert this entity before using it.";
                    return Responce;
                }

            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "DocumentTypeId field is required.";
                return Responce;
            }

            return null;
        }

    }
}