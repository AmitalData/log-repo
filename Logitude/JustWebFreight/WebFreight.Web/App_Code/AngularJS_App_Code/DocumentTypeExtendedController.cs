using Logitude.BL.CommonDataModel.DataContracts;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class DocumentTypeExtendedController : ApiController
    {
        public HttpResponseMessage GetDocumentTypesByTenant(int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                DocumentTypeCopyQuery documentTypeCopyQuery = new DocumentTypeCopyQuery(tenant);
                DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);

                List<DocumentTypePM> documentTypes = documentTypeQuery.GetDocumentTypePMsByTenant(tenant).ToList();

                foreach (DocumentTypePM doc in documentTypes)
                {
                    doc.DocumentTypeTemplates = documentTypeTemplateQuery.GetDocumentTypeTemplatesByDocumentTypeId(doc.Id, tenant).ToList();
                    doc.DocumentTypeCopies = documentTypeCopyQuery.GetDocumentTypeCopiesByDocumentType(doc.Id, null, tenant);
                }

                return Request.CreateResponse(HttpStatusCode.OK, documentTypes);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentTypesByObjectTableAndTenant(string objectTableid, int tenant)
        {
            try
            {
                Authentication();
                SecurityUtility.AuthenticationOnTenant(tenant); 
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                List<DocumentTypePM> documentTypes = documentTypeQuery.GetDocumentTypePMsByObjectTableAndTenant(objectTableid, tenant);
             
                return Request.CreateResponse(HttpStatusCode.OK, documentTypes);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetSinglePMWithOutInclude(string id, int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                DocumentTypePM documentType = documentTypeQuery.GetSinglePMWithOutInclude(id, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, documentType);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        



        public HttpResponseMessage GetDocumentTypesPMByObjectTableIdForDocumentPremissions(string objectTableid, int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                List<DocumentTypePM> documentTypes = documentTypeQuery.GetDocumentTypesPMByObjectTableIdForDocumentPremissions(objectTableid, tenant).ToList();
               
                return Request.CreateResponse(HttpStatusCode.OK, documentTypes);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetShareDocumentByObjectTableAndEntityIdAndshipmentLevel(string entityId, string agentId, string agentReference, string objectTableId, string shipmentLevelCode, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);

                List<ShipmentShareDocumentsData> shareDocumentClassLists = documentTypeQuery.GetShareDocumentByObjectTableAndEntityIdAndshipmentLevel(entityId, agentId, agentReference , objectTableId, shipmentLevelCode, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, shareDocumentClassLists);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetFollowUpDocumentTypeByEntityId(string entityId, string objectTableName, int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                List<DocumentTypePM> documentTypes = documentTypeQuery.GetFollowUpDocumentTypeByEntityId( entityId,  objectTableName,  tenant);
                return Request.CreateResponse(HttpStatusCode.OK, documentTypes);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentTypesListByObjectTableAndTenant(int tenant, string objectTableid, string s)
        {
            try
            {
                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                List<DocumentTypeList> documentTypes = documentTypeQuery.GetDocumentTypeListsByObjectTableAndTenant(objectTableid, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, documentTypes);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentTypeListsByEnityIdAndTenant(string transportModeId, string shipmentLevelCode, string objecttableId, int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                List<DocumentTypeList> documentTypes = documentTypeQuery.GetDocumentTypeListsByEnityIdAndTenant(transportModeId, shipmentLevelCode, objecttableId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, documentTypes);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


   

        public HttpResponseMessage GetDocumentTypesByEnityIdAndtransportModeId(string transportModeId, string shipmentLevelCode, string objecttableId, int tenant, string childrenObjectTableIds)
        {
            try
            {

                if (!string.IsNullOrEmpty(childrenObjectTableIds) && childrenObjectTableIds.Contains("undefined"))
                {
                    childrenObjectTableIds = childrenObjectTableIds.Replace("undefined,", "");
                }

                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                List<DocumentTypePM> documentTypes = documentTypeQuery.GetDocumentTypePMsByEnityIdAndTenant(transportModeId, shipmentLevelCode, objecttableId, tenant, childrenObjectTableIds);
                return Request.CreateResponse(HttpStatusCode.OK, documentTypes);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentTypesByTenantAndTransportMode(int tenant, string transportMode)
        {
            try
            {
                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, documentTypeQuery.GetDocumentTypesByTenantAndTransportMode(tenant, transportMode));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentTypeByCode(string code, int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                DocumentTypePM result = documentTypeQuery.GetSinglePMByCodeAndTenant(code, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }




        public HttpResponseMessage GetDocumentTypeListByCode(int tenant, string code, bool s)
        {
            try
            {
                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                DocumentTypeList result = documentTypeQuery.GetSingleListByCodeAndTenant(code, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentTypeListById(string id, int tenant, bool s)
        {

            try
            {
                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                DocumentTypeList result = documentTypeQuery.GetDocumentTypeListById(id, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetDocumentTypesSearch(string name, int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                List<DocumentTypePM> result = documentTypeQuery.GetDocumentTypePMsByTenant(tenant).Where(d => d.Name.StartsWith(name) && d.Tenant == tenant).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDoesDocumentTypeCodeExist(int tenant, string code, int x)
        {
            try
            {
                Authentication();
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                bool a = (documentTypeRepository.GetDocumentTypes(tenant).Where(d => d.Code == code && d.Tenant == tenant)).Any();
                return Request.CreateResponse(HttpStatusCode.OK, a);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingleDocumentType(string id, string documentOutId, int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                DocumentTypePM documentTypePM = documentTypeQuery.GetSinglePM(id, documentOutId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, documentTypePM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetDocumentTypeCopiesByDocumentTypeId(string id, string documentOutId, int tenant)
        {
            try
            {
                Authentication();

                DocumentTypeCopyQuery documentTypeCopyQuery = new DocumentTypeCopyQuery(tenant);
                List<DocumentTypeCopyPM> documentTypeCopies = documentTypeCopyQuery.GetDocumentTypeCopiesByDocumentType(id, documentOutId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, documentTypeCopies);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage GetSingleDocumentTypeList(string id, int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                DocumentType documentType = documentTypeRepository.GetSingleDocumentTypes(id, tenant);
                DocumentTypeList documentTypeList = null;
                if (documentType != null)
                {
                    List<DocumentType> singleEntityList = new List<DocumentType>();
                    singleEntityList.Add(documentType);

                    DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);
                    IQueryable<DocumentType> iQueryable = singleEntityList.AsQueryable();
                    IQueryable<DocumentTypeList> iQueryableEntityList = documentTypeQuery.GetIQueryableEntityList(iQueryable);
                    documentTypeList = iQueryableEntityList.FirstOrDefault();

                    ObjectTablePM pm = ObjectTableQuery.GetSingleObjectTableById(documentTypeList.ObjectTableId, documentTypeList.Tenant);

                    if (pm != null)
                    {
                        documentTypeList.ObjectTableName = pm.Name;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, documentTypeList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentTypeLists(int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);

                IQueryable<DocumentType> documentTypes = documentTypeRepository.GetDocumentTypes(tenant);
                IQueryable<DocumentTypeList> query2 = documentTypeQuery.GetIQueryableEntityList(documentTypes);
                return Request.CreateResponse(HttpStatusCode.OK, query2);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [Query(HasSideEffects = true)]
        public HttpResponseMessage GetDocumentTypeFilters(byte[] xmlFilters, int tenant)
        {
            try
            {
                Authentication();

                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);

                MemoryStream memorystream = new MemoryStream(xmlFilters);
                XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
                QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
                GenericFilter filter = new GenericFilter();
                GenericSort sortClass = new GenericSort();
                IQueryable<DocumentType> documentTypes = documentTypeRepository.GetDocumentTypes(tenant);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                documentTypes = filter.GetFilteredQuery<DocumentType>(nonListQueryOperation, documentTypes);
                int skippedDocumentTypes = queryOperations.PageIndex;

                IQueryable<DocumentTypeList> query2 = documentTypeQuery.GetIQueryableEntityList(documentTypes);

                query2 = filter.GetFilteredQuery<DocumentTypeList>(listQueryOperation, query2);

                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    PropertyInfo propInfo = typeof(DocumentTypeList).GetProperty(queryOperations.SortByColumnName);
                    List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("DocumentType", tenant).ToList();

                    ObjectField objectField = (from a in shipmentObjectFields
                                               where a.FieldName == queryOperations.SortByColumnName
                                               select a).FirstOrDefault();
                    if (objectField != null)
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "text":
                            case "ntext":
                                {
                                    query2 = sortClass.GetSorterQuery<DocumentTypeList, string>(queryOperations, query2);
                                    break;
                                }
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<DocumentTypeList, double>(queryOperations, query2);
                                    break;
                                }
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<DocumentTypeList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<DocumentTypeList, int>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<DocumentTypeList, bool>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderByDescending(d => d.Code);
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    query2 = query2.OrderByDescending(d => d.Code);
                }

                query2 = query2.Skip(skippedDocumentTypes);
                query2 = query2.Take(queryOperations.PageSize);


                List<DocumentTypeList> objectsList = query2.ToList();
                foreach (var list in objectsList)
                {
                    if (!string.IsNullOrEmpty(list.ObjectTableId))
                    {
                        ObjectTablePM pm = ObjectTableQuery.GetSingleObjectTableById(list.ObjectTableId, list.Tenant);
                        list.ObjectTableName = pm.Name;
                    }

                }

                query2 = objectsList.AsQueryable();

                return Request.CreateResponse(HttpStatusCode.OK, query2);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentTypeFiltersCount(byte[] xmlFilters, int tenant, bool iscount)
        {
            try
            {
                Authentication();
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);

                MemoryStream memorystream = new MemoryStream(xmlFilters);
                XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
                QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
                GenericFilter filter = new GenericFilter();
                GenericSort sortClass = new GenericSort();
                IQueryable<DocumentType> documentTypes = documentTypeRepository.GetDocumentTypes(tenant);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
                documentTypes = filter.GetFilteredQuery<DocumentType>(nonListQueryOperation, documentTypes);

                int skippeddocumentTypes = queryOperations.PageIndex;
                IQueryable<DocumentTypeList> query2 = documentTypeQuery.GetIQueryableEntityList(documentTypes);
                query2 = filter.GetFilteredQuery<DocumentTypeList>(listQueryOperation, query2);
                int count = query2.Count();
                return Request.CreateResponse(HttpStatusCode.OK, count);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostDocumentType(DocumentTypePM entity)
        {
            Authentication();
            try
            {
                ICommonDataContext objectContext = CommonDataContext.GetContext(entity.Tenant);
                DocumentTypeService service = new DocumentTypeService(objectContext, entity.Tenant);
                service.Create(entity);

                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }


            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutDocumentType(DocumentTypePM currentEntity)
        {
            Authentication();
            try
            {
                ICommonDataContext objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(objectContext);
                DocumentTypeCopyRepository documentTypeCopyRepository = new DocumentTypeCopyRepository(objectContext);

                DocumentType docType = documentTypeRepository.GetSingleDocumentTypes(currentEntity.Id, currentEntity.Tenant);
                if (currentEntity.Name != docType.Name)
                {
                    if (currentEntity.DocumentTypeCopies.Count == 1)
                    {
                        DocumentTypeCopyPM copyPM = currentEntity.DocumentTypeCopies.FirstOrDefault() as DocumentTypeCopyPM;
                        DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopy(copyPM.Id);
                        documentTypeCopy.Name = currentEntity.Name;
                        copyPM.Name = currentEntity.Name;

                        documentTypeCopyRepository.Update(documentTypeCopy);
                    }
                }
                foreach (DocumentTypeCopyPM item in currentEntity.DocumentTypeCopies)
                {
                    DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopy(item.Id);
                   documentTypeCopy =  MapDocumentTypeCopyDocumentTypeCopyPM(documentTypeCopy,item);
                   documentTypeCopyRepository.Update(documentTypeCopy);
                }
                documentTypeCopyRepository.SubmitChanges();
                DocumentTypeService service = new DocumentTypeService(objectContext, currentEntity.Tenant);
                service.Update(currentEntity, new List<DocumentTypeCopyPM>());



                TableLastUpdateClass.UpdateTableHistory(currentEntity.Tenant, "DocumentType");

                return Request.CreateResponse(HttpStatusCode.OK, currentEntity);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public DocumentTypeCopy MapDocumentTypeCopyDocumentTypeCopyPM(DocumentTypeCopy poco , DocumentTypeCopyPM pm)
        {  
            poco.Code = pm.Code;
            poco.DocumentTypeId = pm.DocumentTypeId;
            poco.InActive = pm.InActive;
            poco.IndexOrder = pm.IndexOrder;
            poco.IsSelectedByDefault = pm.IsSelectedByDefault;
            poco.Name = pm.Name;
            poco.Tenant = pm.Tenant;

          
            return poco;
        }

        public HttpResponseMessage DeleteDocumentType(DocumentTypePM entity)
        {
            try
            {
                ICommonDataContext objectContext = CommonDataContext.GetContext(entity.Tenant);
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(objectContext);
                DocumentType docType = documentTypeRepository.GetSingleDocumentTypes(entity.Id, entity.Tenant);
                documentTypeRepository.Remove(docType);
                return Request.CreateResponse(HttpStatusCode.OK, entity);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutUpdateDocumentTypePMLists(List<DocumentTypePM>documentTypePMists)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                        ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                        DocumentTypeService service = new DocumentTypeService(MyContext, authToken.Tenant);

                        foreach (DocumentTypePM documentTypePM in documentTypePMists)
                        {
                            service.Update(documentTypePM, false);
                        }


                        return Request.CreateResponse(HttpStatusCode.OK, documentTypePMists);
                   
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


        public HttpResponseMessage GetDocumentTypeListsByObjectTableIdForAutomations(string objectTableId, int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);

                List<DocumentTypeList> documentTypes = documentTypeQuery.GetDocumentTypeListsByObjectTableIdForAutomations(objectTableId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, documentTypes);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage getTop5DocumentTypesPMsByObjectTableAndTenant(int tenant, string objectTableId)
        {
            try
            {
                Authentication();
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);

                List<DocumentTypePM> documentTypes = documentTypeQuery.GetTop5DocumentTypePMsByObjectTableId(objectTableId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, documentTypes);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }




        public HttpResponseMessage GetDocumentTypeCopyLists(string objectTableId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("DocumentType", "READ", authToken.Tenant);
                DocumentTypeCopyQuery documentTypeQuery = new DocumentTypeCopyQuery(authToken.Tenant);
                List<DocumentTypeCopyList> documentTypeCopyLists = documentTypeQuery.GetDocumentTypeCopiesByObjectTableId(objectTableId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, documentTypeCopyLists);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }






        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("DocumentType", "READ", authToken.Tenant);
        }


    }
}