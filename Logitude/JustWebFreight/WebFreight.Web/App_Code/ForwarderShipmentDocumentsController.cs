using Logitude.BL.CommonDataModel.CodePropertiesMapping;
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
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class ForwarderShipmentDocumentsController : ApiController
    {
        public bool GetIfNew(string id, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("DocumentsFiling", "READ", tenant); 
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            var DocumentFilingPM = documentsFilingQuery.GetSinglePMByCustomerId(id, tenant);
            if (DocumentFilingPM == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public HttpResponseMessage Post(DocumentsFilingPM EntityPM)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(EntityPM.Tenant);
                //SecurityUtility.CheckContactFeature("DocumentsFiling", "NEW", EntityPM.Tenant);
                bool IsNewLog = false;
                APIException Result = null;
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(EntityPM.Tenant);
                APILogsService apiLogsService = new APILogsService(webFreightContext, EntityPM.Tenant);
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(EntityPM.Tenant);
                var NewEntityPM = new DocumentsFilingPM() { Tenant = EntityPM.Tenant };
                Result = MapNewEntityPMToEntityPM(EntityPM, NewEntityPM);
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
                        Refrence = EntityPM.Code,
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
                LogPM.Subject = "Insert Documents To Forwarder Tenant";
                if (IsNewLog)
                {
                    apiLogsService.Create(LogPM);
                }
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Inserting Document To Forwarder Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(EntityPM), null, null, "");
                // Insert
                try
                {
                    if (EntityPM.buffer != null)
                    {
                        var DocId = "";
                        var Res = UploadDocumentByte(EntityPM.buffer, (long)EntityPM.FileSize, EntityPM.SentSize, EntityPM.BlockIdsList, EntityPM.BufferNumber, EntityPM.Tenant, EntityPM.FullFileName, EntityPM.DocumentId);
                        if (Res.HasError)
                        {
                            var apiException = new APIException()
                            {
                                ErrorType = "Uploading Document Error",
                                ErrorMessage = "There was an error occured while uploading the document " + Res.ErrorMessage
                            };
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Update Document At Forwarder Tenant Faild " + DateTime.Now, null, LogitudeXmlSerializer.SerializeObjectToXmlString(apiException), null, "");

                            return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, Res.Result);
                        }
                    }
                    else
                    {
                        //APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Inserting Document To Forwarder Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(EntityPM), null, null, "");
                        if (Result == null)
                        {
                            ShipmentPM ForwarderShipment = null;
                            ShipmentQuery shipmentQuery = new ShipmentQuery(NewEntityPM.Tenant);
                            ICommonDataContext objectContext = CommonDataContext.GetContext(NewEntityPM.Tenant);
                            ContactQuery contactQuery = new ContactQuery(NewEntityPM.Tenant);
                            var loggedContact = contactQuery.GetContactByNameAndTenant("system@tenant" + EntityPM.Tenant + ".com", NewEntityPM.Tenant, true);
                            if (loggedContact == null)
                            {
                                loggedContact = contactQuery.GetContactByEmailOnly("system@tenant" + EntityPM.Tenant + ".com", NewEntityPM.Tenant);
                            }
                            NewEntityPM.CreatedByUserId = loggedContact.Id;
                            NewEntityPM.OwnerId = loggedContact.Id;
                            NewEntityPM.UpdatedByUserId = loggedContact.Id;
                            //ForwarderShipment = shipmentQuery.GetSingleShipmentPMByNumber(EntityPM.EntityId, EntityPM.Tenant);
                            //DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(EntityPM.Tenant);
                            //var DocType = documentTypeQuery.GetSinglePMByCodeAndTenant(EntityPM.DocumentTypeCode, EntityPM.Tenant);
                            //DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(objectContext);
                            //foreach (var item in EntityPM.DocumentsFilingMetaDataValues)
                            //{
                            //    var DocFilingType = DocumentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode(item.DocumentsMetaDataTypeId, EntityPM.Tenant);
                            //    item.DocumentsMetaDataTypeId = DocFilingType.Id;
                            //}
                            //ContactRepository contactRepository = new ContactRepository(EntityPM.Tenant);
                            //Contact User = contactRepository.GetSingleContactByEmail("system@tenant" + EntityPM.Tenant + ".com", EntityPM.Tenant, true);
                            //UserRepository userRepository = new UserRepository(EntityPM.Tenant);
                            //User User = userRepository.GetSingleUserByEmail("system@tenant" + EntityPM.Tenant + ".com", EntityPM.Tenant, true);
                            //EntityPM.EntityId = ForwarderShipment.Id;
                            //EntityPM.DocumentTypeId = DocType.Id;
                            //EntityPM.CreatedByUserId = User.Id;
                            //EntityPM.UpdatedByUserId = User.Id;
                            //EntityPM.OwnerId = User.Id;
                            //EntityPM.ReceivedByUserId = User.Id;
                            //EntityPM.DeletedByUserId = null;
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Inserting Document To Forwarder Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(NewEntityPM), null, null, "");

                            DocumentsFilingService documentsFilingService = new DocumentsFilingService(objectContext, NewEntityPM.Tenant);
                            documentsFilingService.SetChangeSet(NewEntityPM.DocumentsFilingMetaDataValues);
                            documentsFilingService.Create(NewEntityPM, null, null, true);
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, "Insert Document To Forwarder Tenant Done Successfully " + DateTime.Now, null, null, null, "");

                            return Request.CreateResponse(HttpStatusCode.OK, NewEntityPM.Id);
                        }
                        else
                        {
                            var Failmsg = "Insert Document Faild " + DateTime.Now;
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
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Inserting Document To Forwarder Tenant Faild " + DateTime.Now, null, LogitudeXmlSerializer.SerializeObjectToXmlString(apiException), null, "");

                        return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);

                    }
                    else
                    {

                        string errorMessage = ex.Message + Environment.NewLine;

                        if (ex.InnerException != null)
                        {

                            errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                        }

                        errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                        var apiException = new APIException()
                        {
                            ErrorType = ex.GetType().Name,
                            ErrorMessage = ex.Message
                        };
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Insert Document At Forwarder Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

                        return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message + Environment.NewLine;

                if (ex.InnerException != null)
                {
                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
                }

                errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                var apiException = new APIException()
                {
                    ErrorType = ex.GetType().Name,
                    ErrorMessage = errorMessage
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
            }

        }

        private Response UploadDocumentByte(byte[] buffer, long FileSize, long SentSize, string[] BlockIdsList, int BufferNumber, int Tenant, string FileName, string DocumentId)
        {
            DocumentFileUploadHelper documentFileUploadHelper = new DocumentFileUploadHelper();
            var response = documentFileUploadHelper.UploadDocumentFileData(buffer, FileSize, SentSize, BlockIdsList, BufferNumber, Tenant, FileName, DocumentId);
            return response;
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(DocumentsFilingPM EntityPM)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(EntityPM.Tenant);
                //SecurityUtility.CheckContactFeature("DocumentsFiling", "UPDATE", EntityPM.Tenant);
                bool IsNewLog = false;
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(EntityPM.Tenant);
                DocumentsFilingPM DocumentFilingPM = documentsFilingQuery.GetSinglePMByCustomerId(EntityPM.CustomerDocumentId, EntityPM.Tenant);
                APIException Result = MapNewEntityPMToEntityPM(EntityPM, DocumentFilingPM);

                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(EntityPM.Tenant);
                APILogsService apiLogsService = new APILogsService(webFreightContext, EntityPM.Tenant);
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(EntityPM.Tenant);
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
                        //Refrence = Shipment.ShipmentNumber,
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
                LogPM.Subject = "Update Documents To Forwarder Tenant";
                if (IsNewLog)
                {
                    apiLogsService.Create(LogPM);
                }
                try
                {

                    if (EntityPM.buffer != null)
                    {
                        var Res = UploadDocumentByte(EntityPM.buffer, (long)EntityPM.FileSize, EntityPM.SentSize, EntityPM.BlockIdsList, EntityPM.BufferNumber, EntityPM.Tenant, EntityPM.FullFileName, EntityPM.DocumentId);
                        if (Res.HasError)
                        {
                            var apiException = new APIException()
                            {
                                ErrorType = "Uploading Document Error",
                                ErrorMessage = "There was an error occured while uploading the document" + Res.ErrorMessage
                            };
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Update Document At Forwarder Tenant Faild " + DateTime.Now, null, LogitudeXmlSerializer.SerializeObjectToXmlString(apiException), null, "");

                            return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, Res.Result);
                        }

                    }
                    else
                    {
                        if (Result == null)
                        {
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Updating Document at Forwarder Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(EntityPM), null, null, "");

                            //ShipmentPM ForwarderShipment = null;
                            //ShipmentQuery shipmentQuery = new ShipmentQuery(EntityPM.Tenant);
                            //ForwarderShipment = shipmentQuery.GetSingleShipmentPMByNumber(EntityPM.EntityId, EntityPM.Tenant); // todo: Ask About How To get Forwarder Tenant
                            //EntityPM.EntityId = ForwarderShipment.Id;
                            //ContactRepository contactRepository = new ContactRepository(EntityPM.Tenant);
                            //Contact User = contactRepository.GetSingleContactByEmail("system@tenant" + EntityPM.Tenant + ".com", EntityPM.Tenant, true);
                            //UserRepository userRepository = new UserRepository(EntityPM.Tenant);
                            //User User = userRepository.GetSingleUserByEmail("system@tenant" + EntityPM.Tenant + ".com", EntityPM.Tenant, true);

                            //EntityPM.CreatedByUserId = User.Id;
                            //EntityPM.UpdatedByUserId = User.Id;
                            //EntityPM.OwnerId = User.Id;
                            //EntityPM.ReceivedByUserId = User.Id;
                            //EntityPM.DeletedByUserId = null;
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Updating Document To Forwarder Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(DocumentFilingPM), null, null, "");

                            ICommonDataContext objectContext = CommonDataContext.GetContext(DocumentFilingPM.Tenant);
                            //DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(objectContext);
                            //foreach (var item in EntityPM.DocumentsFilingMetaDataValues)
                            //{
                            //    var DocFilingType = DocumentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode(item.DocumentsMetaDataTypeId, EntityPM.Tenant);
                            //    item.DocumentsMetaDataTypeId = DocFilingType.Id;
                            //}
                            DocumentsFilingService documentsFilingService = new DocumentsFilingService(objectContext, DocumentFilingPM.Tenant);
                            documentsFilingService.SetChangeSet(DocumentFilingPM.DocumentsFilingMetaDataValues);
                            if (DocumentFilingPM.FileData == null)
                            {
                                documentsFilingService.Update(DocumentFilingPM, null, null, true);
                            }
                            else
                            {
                                documentsFilingService.Update(DocumentFilingPM, DocumentFilingPM.FileData, null, true);
                            }
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, "Updating Document To Forwarder Tenant Done Successfully " + DateTime.Now, null, null, null, "");

                            return Request.CreateResponse(HttpStatusCode.OK, DocumentFilingPM.Id);
                        }
                        else
                        {
                            var Failmsg = "Updating Document Faild " + DateTime.Now;
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
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Update Document At Forwarder Tenant Faild " + DateTime.Now, null, LogitudeXmlSerializer.SerializeObjectToXmlString(apiException), null, "");

                        return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                    }
                    else
                    {

                        string errorMessage = ex.Message + Environment.NewLine;

                        if (ex.InnerException != null)
                        {

                            errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                        }

                        errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                        var apiException = new APIException()
                        {
                            ErrorType = ex.GetType().Name,
                            ErrorMessage = errorMessage
                        };
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Update Document At Forwarder Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

                        return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message + Environment.NewLine;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                }

                errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                var apiException = new APIException()
                {
                    ErrorType = ex.GetType().Name,
                    ErrorMessage = errorMessage
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        private APIException MapNewEntityPMToEntityPM(DocumentsFilingPM newEntityPM, DocumentsFilingPM EntityPM)
        {
            APIException Responce = new APIException();
            if (!string.IsNullOrEmpty(newEntityPM.ObjectTableName))
            {
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(newEntityPM.Tenant);
                var Objecttable = objectTabelRepository.GetObjectTableByName(newEntityPM.ObjectTableName, newEntityPM.Tenant, true);
                EntityPM.ObjectTableId = Objecttable.Id;
            }
            EntityPM.IsDeleted = newEntityPM.IsDeleted;
            EntityPM.Notes = newEntityPM.Notes;
            if (!string.IsNullOrEmpty(newEntityPM.ForwarderDocumentId))
            {
                EntityPM.ForwarderDocumentId = newEntityPM.ForwarderDocumentId;
            }
            if (!string.IsNullOrEmpty(newEntityPM.CustomerDocumentId))
            {
                EntityPM.CustomerDocumentId = newEntityPM.CustomerDocumentId;

            }
            if (!string.IsNullOrEmpty(newEntityPM.EntityId))
            {
                ShipmentQuery shipmentQuery = new ShipmentQuery(newEntityPM.Tenant);
                ShipmentPM Shipment = shipmentQuery.GetSingleShipmentPMByNumber(newEntityPM.EntityId, newEntityPM.Tenant);
                if (Shipment != null)
                {
                    EntityPM.EntityId = Shipment.Id;
                }
                else
                {
                    EntityPM.EntityId = null;
                }
            }
            if (!string.IsNullOrEmpty(newEntityPM.Description))
            {
                EntityPM.Description = newEntityPM.Description;
            }
            if (newEntityPM.FileData != null && newEntityPM.FileData.Length > 0)
            {
                EntityPM.FileData = newEntityPM.FileData;
            }
            if (newEntityPM.DocumentsFilingMetaDataValues != null)
            {
                foreach (var item in newEntityPM.DocumentsFilingMetaDataValues)
                {
                    var DocMetaDataTypeId = DocumentTypeMetaDataTypePropertiesMapping.GetDocumentTypeMetaDataTypeProperties(newEntityPM.Tenant, new CodeProperties() { Code = item.DocumentsMetaDataTypeId });
                    if (!string.IsNullOrEmpty(DocMetaDataTypeId))
                    {
                        if (EntityPM.DocumentsFilingMetaDataValues == null)
                        {
                            EntityPM.DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValuePM>(); ;
                        }
                        EntityPM.DocumentsFilingMetaDataValues.Add(new DocumentsFilingMetaDataValuePM()
                        {
                            Tenant = newEntityPM.Tenant,
                            MetaDataValue = item.MetaDataValue,
                            DocumentsMetaDataTypeId = DocMetaDataTypeId,
                            ChangeSetOp = item.ChangeSetOp
                        });
                    }
                    else
                    {
                        Responce.ErrorType = "Validation Error";
                        Responce.ErrorMessage = "DocumentTypeMetaDataType field doesn't exist in the database, insert this entity before using it.";
                        return Responce;
                    }

                }

            }
            if (!string.IsNullOrEmpty(newEntityPM.FileExtension))
            {
                EntityPM.FileExtension = newEntityPM.FileExtension;
            }
            if (!string.IsNullOrEmpty(newEntityPM.SignersList))
            {
                EntityPM.SignersList = newEntityPM.SignersList;
            }

            EntityPM.IsDigitallySigned = newEntityPM.IsDigitallySigned;
            EntityPM.DocumentId = newEntityPM.DocumentId;
            EntityPM.DirectionCode = "I";
            EntityPM.FileSize = newEntityPM.FileSize;
            EntityPM.FileName = newEntityPM.FileName;
            EntityPM.DontAddToQueue = true;
            EntityPM.IsSharedWithCustomer = newEntityPM.IsSharedWithCustomer;
            EntityPM.IsSharedWithForwarder = newEntityPM.IsSharedWithForwarder;
            EntityPM.CustomerTenantNumber = newEntityPM.CustomerTenantNumber; 

            if (newEntityPM.DocumentTypeCode != null)
            {
                var DocumentTypeId = DocumentTypeCodePropertiesMapping.GetDocumentTypeFromDocumentTypeProperties(newEntityPM.Tenant, new CodeProperties() { Code = newEntityPM.DocumentTypeCode });
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