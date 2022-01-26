using Logitude.BL.CommonDataModel.APIDataContract;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class DocumentsFilingController : ApiController
    {

        public HttpResponseMessage Post(Logitude.BL.CommonDataModel.APIDataContract.ApiV1.DocumentsFiling entity)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        if (authToken == null)
                        {
                            throw new AutenticationException("Sorry! this user is not authorized!");
                        }
						SecurityUtility.AuthenticateAPICall(authToken.Tenant);
						SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                        DocumentsFilingQueryService mappingService = new DocumentsFilingQueryService(authToken.Tenant);

                        DocumentsFilingPM entityPM = mappingService.DocumentsFilingCustomDataMappingAndValidating(entity, authToken.Tenant, true);
                        entityPM.IsUoloadedField = true;
                        entityPM.DocumentTypeCode = entity.DocumentType != null ? entity.DocumentType.Code : null;
                        DocumentsFilingService service = new DocumentsFilingService(MyContext, authToken.Tenant);
                        service.Create(entityPM, null, null, true);
                        if (!string.IsNullOrEmpty(entityPM.DocumentId) && !string.IsNullOrEmpty(entityPM.FileName))
                        {
                            DocumentRepository documentRepository = new DocumentRepository(MyContext);
                            Document document = documentRepository.GetSingleDocument(authToken.Tenant, entityPM.DocumentId);
                            if (!document.HasFile) throw new Exception("this document's file has been deleted");
                            document.FileName = entityPM.FileName;
                            documentRepository.Update(document);
                            documentRepository.SubmitChanges();
                        }

                        var result = mappingService.GetDocumentsFilingById(entityPM.Id, authToken.Tenant);
                        APIHelper.AddCommunicationLog("D",  entity, result, "DocumentsFiling", entityPM.Id, "Documents Filing API", authToken.Tenant);

                        scope.Complete();

                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F",  entity, apiExceptionResult.Exception, "DocumentsFiling", null, "Documents Filing API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "DocumentsFiling", null, "Documents Filing API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }


        public HttpResponseMessage Put(Logitude.BL.CommonDataModel.APIDataContract.ApiV1.DocumentsFiling entity)
        {
          
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        if (authToken == null)
                        {
                            throw new AutenticationException("Sorry! this user is not authorized!");
                        }
                      
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                        ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                        DocumentsFilingQueryService mappingService = new DocumentsFilingQueryService(authToken.Tenant);

                        DocumentsFilingPM entityPM = mappingService.DocumentsFilingCustomDataMappingAndValidating(entity, authToken.Tenant, false);

                        DocumentsFilingService service = new DocumentsFilingService(MyContext, authToken.Tenant);
                        service.Update(entityPM, null, null, true);

                        var result = mappingService.GetDocumentsFilingById(entityPM.Id, authToken.Tenant);

                        APIHelper.AddCommunicationLog("D", entity, result, "DocumentsFiling", entityPM.Id, "Documents Filing API", authToken.Tenant);

                        scope.Complete();

                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "DocumentsFiling", null, "Documents Filing API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "DocumentsFiling", null, "Documents Filing API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

    }
}
