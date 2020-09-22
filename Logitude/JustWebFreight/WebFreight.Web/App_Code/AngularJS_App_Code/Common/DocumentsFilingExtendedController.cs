using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Common
{
    public class DocumentsFilingExtendedController : ApiController
    {




        public HttpResponseMessage GetDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable(string entityId, string childEntityId, string objectTableId, string directionCode, int tenant, bool withDocuments)
        {
            Authentication();
            if (childEntityId == "null") childEntityId = "";
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            List<DocumentsFilingPM> myResult = documentsFilingQuery.GetDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable(entityId, childEntityId, objectTableId, directionCode, tenant);
            if (withDocuments)
            {
                myResult = myResult.Where(d => d.DocumentId != null && d.HasFile == true).ToList();
            }
            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }

        
        public HttpResponseMessage GetQuoationDocumentsFilingByQuoteIdAndObjectTableIdAndDocumentTypeCode(string entityId, string objectTableId, string documentTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(authToken.Tenant);
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(authToken.Tenant);
                var documentTypeId = documentTypeQuery.GetDocumentTypeIdByCode(documentTypeCode, authToken.Tenant);
                DocumentsFilingPM myResult = documentsFilingQuery.GetDocumentsFilingPMByEntityIdAndObjectTableIdAndDocumentTypeId(entityId, objectTableId, documentTypeId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetDocumentsFilingsByEntityIdAndObjectTable(string entityId, string childEntityId, string objectTableId, string directionCode, int tenant, bool withDocuments)
        {
            Authentication();

            if (childEntityId == "null") childEntityId = "";
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            List<DocumentsFilingPM> myResult = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityId, childEntityId, objectTableId, directionCode, tenant);

            if (withDocuments)
            {
                myResult = myResult.Where(d => d.DocumentId != null && d.HasFile == true).ToList();
            }
            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }




        public HttpResponseMessage GetDocumentsFilingsByEntityIdAndObjectTableAndDirectionCode(string entityId, string childEntityId, string objectTableId, string directionCode, int tenant, bool withDocuments)
        {
            Authentication();

            if (childEntityId == "null") childEntityId = "";
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            List<DocumentsFilingPM> myResult = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTableAndDirectionCode(entityId, childEntityId, objectTableId, directionCode, tenant);

            if (withDocuments)
            {
                myResult = myResult.Where(d => d.DocumentId != null && d.HasFile == true).ToList();
            }
            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }

        

        public HttpResponseMessage GetAllDocumentsFilingsByEntityIdAndObjectTable(string entityId, string objectTableId, string directionCode, int tenant)
        {
            Authentication();

            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            List<DocumentsFilingPM> myResult = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityId, "", objectTableId, directionCode, tenant);

            //if (withDocuments)
            //{
            //    myResult = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityId, childEntityId, objectTableId, directionCode, tenant).Where(d => d.DocumentId != null && d.HasFile == true).ToList();
            //}
            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }
        public HttpResponseMessage GetRequestedDocumentsFilingsByEntityIdAndObjectTable(string entityId, string objectTableId, string directionCode, int tenant)
        {
            Authentication();

            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            List<DocumentsFilingPM> myResult = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityId, "", objectTableId, directionCode, tenant).Where(d => d.HasFile == false && d.IsRequested == true).ToList();

            //if (withDocuments)
            //{
            //    myResult = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityId, childEntityId, objectTableId, directionCode, tenant).Where(d => d.DocumentId != null && d.HasFile == true).ToList();
            //}
            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }

        public HttpResponseMessage GetSingleDocumentsFilingByChild(string documentTypeId, string paymentNumber, int tenant)
        {
            Authentication();

            if (paymentNumber == "null") paymentNumber = "";

            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            DocumentsFilingPM myResult = documentsFilingQuery.GetDocumentsFilingByChild(documentTypeId, paymentNumber, tenant);

            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }

        public HttpResponseMessage GetCreateDocumentsFiling(string documentTypeId, string entityId, string childEntityId, string childReference, string objectTableId, string directionCode, int tenant, string externalEntityName =null, string externalEntityReference =null, string entityNumber=null)
        {

            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
            UserRepository userRepository = new UserRepository(tenant);

            string loggedUserEmail = "";
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationTokenRepository tokenRep = new AuthenticationTokenRepository(MyContext);
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            if (authToken != null)
            {
                loggedUserEmail = authToken.Email;
            }


            User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);
            DocumentsFilingPM newDocument = new DocumentsFilingPM() { DocumentTypeId = documentTypeId, EntityId = entityId, Tenant = tenant, ObjectTableId = objectTableId, ChildEntityId = childEntityId, ChildEntityReference = childReference, DirectionCode = directionCode };
            newDocument.SearchFields = newDocument.Code + "," + newDocument.DirectionCode + "," + loggedUser.Contact.EnglishName + "," + loggedUser.Contact.LocalName;
            newDocument.Code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
            newDocument.CreatedByUserId = loggedUser.Id;
            newDocument.OwnerId = loggedUser.Id;
            newDocument.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            newDocument.UpdatedByUserId = loggedUser.Id;
            newDocument.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            newDocument.ExternalEntityName = externalEntityName;
            newDocument.ExternalEntityReference = externalEntityReference;
            newDocument.EntityNumber = entityNumber;
            // documentsFilingRepository.Add(newDocument);
            //documentsFilingRepository.SubmitChanges();

            DocumentsFilingService service = new DocumentsFilingService(MyContext, tenant);
            service.Create(newDocument, null);

            DocumentsFilingPM extDocPM = documentsFilingQuery.GetSinglePM(newDocument.Id, newDocument.Tenant);
            return Request.CreateResponse(HttpStatusCode.OK, extDocPM);

        }
        public HttpResponseMessage GetCreateDocumentShipmentEvent( string entityId,  string notes, string eventCode)
        {
            string token = System.Web.HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            UserRepository userRepository = new UserRepository(authToken.Tenant);
            User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, authToken.Email, authToken.Tenant, true);
            if (!string.IsNullOrEmpty(notes))
            {
                if (notes.Contains('.')) notes = notes.Split('.')[0];
            }
            EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = authToken.Tenant,
                    EventTypeCode = eventCode,
                    UserId = loggedUser.Id,
                    EntityId = entityId,
                    ObjectTableName = "Shipment",
                    Notes = notes,
                });
        
            return Request.CreateResponse(HttpStatusCode.OK, true);

        }
      
        public HttpResponseMessage GetDocumentsFilingsById(string id)
        {
            Authentication();


            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            DocumentsFilingPM myResult = documentsFilingQuery.GetSinglePM(id, tenant);

            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }

        public HttpResponseMessage GetFileSizeAndUnit(int? fileBytes)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            double Byte = 1024;
            string FileSize = "";
            double fileSizeDouble;
            if (fileBytes == null)
            {
                fileBytes = 0;
            }

            double.TryParse(fileBytes.Value.ToString(), out fileSizeDouble);
            if (fileBytes < Byte)
            {
                FileSize = string.Format("{0:0.00}", fileSizeDouble) + " B";
            }
            else if (fileBytes >= Byte && fileBytes < Byte * Byte)
            {
                double result = fileSizeDouble / Byte;
                FileSize = string.Format("{0:0.00}", result) + " KB";
            }
            else if (fileBytes >= Byte * Byte && fileBytes < Byte * Byte * Byte)
            {
                double result = fileSizeDouble / (Byte * Byte);
                FileSize = string.Format("{0:0.00}", result) + " MB";
            }
            else if (fileBytes >= Byte * Byte * Byte && fileBytes < Byte * Byte * Byte * Byte)
            {
                double result = fileSizeDouble / (Byte * Byte * Byte);
                FileSize = string.Format("{0:0.00}", result) + " GB";
            }
            return Request.CreateResponse(HttpStatusCode.OK, FileSize);

        }


        public HttpResponseMessage GetDocumentById(string documentId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            // SecurityUtility.CheckContactFeature("Document", "READ", authToken.Tenant);

            DocumentRepository documentRepository = new DocumentRepository(tenant);
            Document document = documentRepository.GetSingleDocument(tenant, documentId);
            return Request.CreateResponse(HttpStatusCode.OK, document);

        }

        public HttpResponseMessage GetDocumentsFilingByDocumentType(string documentTypeId, string objectTableId, string entityId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            var result = documentsFilingQuery.GetDocumentsFilingByDocumentType(documentTypeId, objectTableId, entityId, tenant);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
       private static   int tenant;
        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            tenant = authToken.Tenant;
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
           
        }

        public HttpResponseMessage Post(DocumentsFilingPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //using (TransactionScope scope = TransactionFactory.GetTransaction())
                    //{
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("DocumentsFiling", "NEW", authToken.Tenant);

                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                    string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
                    UserRepository userRepository = new UserRepository(entityPM.Tenant);
                    User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, entityPM.Tenant, true);

                    entityPM.Code = CodeCounter.GetNumber("DocumentsFiling", entityPM.Tenant).ToString();
                    entityPM.SearchFields = entityPM.Code + "," + entityPM.DirectionCode + "," + loggedUser.Contact.EnglishName + "," + loggedUser.Contact.LocalName;
                    entityPM.CreatedByUserId = loggedUser.Id;
                    entityPM.OwnerId = loggedUser.Id;
                    entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    entityPM.UpdatedByUserId = loggedUser.Id;
                    entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    DocumentsFilingService service = new DocumentsFilingService(MyContext, entityPM.Tenant);
                    service.Create(entityPM);

                    //ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
                    // ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("DocumentsFiling", 0, true);
                    //string email = HttpContext.Current.User.Identity.Name;
                    // ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                    //Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
                    //if (loggedContact != null)
                    //{
                    //    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
                    //}

                    //scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    //}
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


        public HttpResponseMessage Put(DocumentsFilingPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //using (TransactionScope scope = TransactionFactory.GetTransaction())
                    //{
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("DocumentsFiling", "UPDATE", authToken.Tenant);

                    string entityName = "DocumentsFiling" + entityPM.Id + entityPM.Tenant;
                    string entityPmName = "DocumentsFilingPM" + entityPM.Id + entityPM.Tenant;
                    if (CacheManager.CacheWrapper.Get(entityName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityName);
                    }
                    if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityPmName);
                    }

                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                    DocumentsFilingService service = new DocumentsFilingService(MyContext, entityPM.Tenant);
                    service.Update(entityPM, true);

                    //ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
                    //ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("DocumentsFiling", 0, true);
                    //string email = HttpContext.Current.User.Identity.Name;
                    //ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                    //Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
                    //if (loggedContact != null)
                    //{
                    //   ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
                    //}


                    //scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    //}
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage GetIsEntityHasSharedDocs(string entityId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            // SecurityUtility.CheckContactFeature("Document", "READ", authToken.Tenant);

            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            var result = documentsFilingQuery.IsEntityHasSharedDocs(entityId, tenant);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        public HttpResponseMessage GetLogBoxConnectedDocs(string SourceEntityId, string DestEntityId, string ObjectTableId, int tenant)
        {
            try
            {


                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                List<DocumentsFilingPM> myResult = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(SourceEntityId, "", ObjectTableId, "I", tenant);
                DocumentsFilingRepository DocsRepo = new DocumentsFilingRepository(tenant);
                ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
                DocumentsFilingService DocsService = new DocumentsFilingService(MyContext, tenant);
                foreach (var item in myResult)
                {
                    item.EntityId = DestEntityId;
                    DocsService.Update(item, true);
                }

                return Request.CreateResponse(HttpStatusCode.OK, DestEntityId);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



        }

        public HttpResponseMessage GetShareDocumentsWithAgent([FromUri] List<string> Ids)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("DocumentsFiling", "UPDATE", tenant);

                ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);

                string EntityId = "";
                DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepo = new DocumentsMetaDataTypeRepository(tenant);
                var LBO = DocumentsMetaDataTypeRepo.GetSingleDocumentsMetaDataTypeByCode("LBO",tenant);
                var LBF = DocumentsMetaDataTypeRepo.GetSingleDocumentsMetaDataTypeByCode("LBF", tenant);
                //DocumentsFilingMetaDataValueRepository DocumentsFilingMetaDataValueRepo = new DocumentsFilingMetaDataValueRepository(tenant);
                foreach (var item in Ids)
                {

                    DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                    var entityPM = documentsFilingQuery.GetSinglePM(item, tenant);
                    DocumentsFilingService service = new DocumentsFilingService(MyContext, entityPM.Tenant);
                    entityPM.IsSharedWithForwarder = true;
                    entityPM.DontAddToQueue = false;
                    EntityId = entityPM.EntityId;
                    if (entityPM.DocumentsFilingMetaDataValues == null)
                    {
                        entityPM.DocumentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValuePM>();
                    }
                    ShipmentRepository ShipmentRepo = new ShipmentRepository(tenant);
                    var myshipment = ShipmentRepo.GetSingleShipment(entityPM.EntityId, tenant);
                    if (LBF != null)
                    { 
                        DocumentsFilingMetaDataValuePM value1 = new DocumentsFilingMetaDataValuePM();
                        value1.ChangeSetOp = ChangeSetOperation.Insert;
                        value1.DocumentsFilingId = entityPM.Id;
                        value1.MetaDataValue = myshipment.TransportModeId + myshipment.ShipmentNumber;
                        value1.DocumentsMetaDataTypeId = LBF.Id;
                        value1.Tenant = entityPM.Tenant;
                        entityPM.DocumentsFilingMetaDataValues.Add(value1);
                    }
                    if (LBO != null)
                    { 
                        DocumentsFilingMetaDataValuePM value2 = new DocumentsFilingMetaDataValuePM();
                        value2.ChangeSetOp = ChangeSetOperation.Insert;
                        value2.DocumentsFilingId = entityPM.Id;
                        value2.MetaDataValue = myshipment.CustomerReference1;
                        value2.DocumentsMetaDataTypeId = LBO.Id;
                        value2.Tenant = entityPM.Tenant;
                        entityPM.DocumentsFilingMetaDataValues.Add(value2);
                    }
                    service.Update(entityPM, true); 

                }
                var shipmentAdditionalCloudDataRepository = new ShipmentAdditionalCloudDataRepository(tenant);
              
                var shipmentPM = shipmentAdditionalCloudDataRepository.GetSingleShipmentAdditionalCloudData(EntityId, tenant);
                shipmentPM.DocsSentToAgent = true;

                shipmentAdditionalCloudDataRepository.Update(shipmentPM);
                shipmentAdditionalCloudDataRepository.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }




        }

        public HttpResponseMessage GetDocumentsFilingsByCode(string Code)
        {
            Authentication();


            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            DocumentsFilingPM myResult = documentsFilingQuery.GetSinglePMByCode(Code, tenant);

            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }

    }
}