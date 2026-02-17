using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
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

                var documentTypeMetaDataQuery = new DocumentTypeMetaDataQuery(tenant);
                IQueryable<DocumentTypeMetaDataPM> DocumentTypeMetaData = documentTypeMetaDataQuery.GetDocumentTypeMetaDataPMsByDocumentIdTenant(DocumentTypeId, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, DocumentTypeMetaData);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetDocumentMetaDataValuesByDocument(int tenant,string DocumentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

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
    }
}