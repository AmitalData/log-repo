using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class DocumentTypeMetaDataController : ApiController
    {
        public HttpResponseMessage GetDocumentTypeMetaDataByDocumentTypeId(string DocumentTypeId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                var documentTypeMetaDataQuery = new DocumentTypeMetaDataQuery(tenant);
                IQueryable<DocumentTypeMetaDataPM> DocumentTypeMetaData = documentTypeMetaDataQuery.GetDocumentTypeMetaDataPMsByDocumentIdTenant(DocumentTypeId, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, DocumentTypeMetaData);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetDocumentMetaDataValuesByDocument(int tenant, string DocumentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                var documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                IQueryable<DocumentsFilingMetaDataValuePM> DocumentMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(DocumentId, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, DocumentMetaDataValues);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetDocumentsMetaDataTypeByCode(string Code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                var documentTypeMetaDataRepo = new DocumentsMetaDataTypeRepository(authToken.Tenant);
                DocumentsMetaDataType myDocumentsMetaDataType = documentTypeMetaDataRepo.GetSingleDocumentsMetaDataTypeByCode(Code, authToken.Tenant);


                return Request.CreateResponse(HttpStatusCode.OK, myDocumentsMetaDataType);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetDocumentsFilingMetaDataValueByFilingIdAndCode(string documentsFilingId, string code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                var myDocumentsFilingService = new DocumentsFilingService(MyContext, authToken.Tenant);
                DocumentsFilingMetaDataValuePM MyDocumentMetaDataValues = myDocumentsFilingService.GetDocumentsFilingMetaDataValueByFilingIdAndCode(documentsFilingId, code);

                return Request.CreateResponse(HttpStatusCode.OK, MyDocumentMetaDataValues);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage Put(DocumentTypeMetaDataPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                        string entityName = "DocumentTypeMetaData" + entityPM.Id + entityPM.Tenant;
                        string entityPmName = "DocumentTypeMetaDataPM" + entityPM.Id + entityPM.Tenant;
                        CacheManager.CacheWrapper.Invalidate(entityName);
                        CacheManager.CacheWrapper.Invalidate(entityPmName);

                        ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                        DocumentTypeMetaDataService service = new DocumentTypeMetaDataService(MyContext, entityPM.Tenant);

                        service.Update(entityPM);

                        TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "DocumentsMetaDataType");

                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
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
        
        public HttpResponseMessage Delete(string id, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                new DocumentTypeMetaDataService(tenant).Delete(id);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}