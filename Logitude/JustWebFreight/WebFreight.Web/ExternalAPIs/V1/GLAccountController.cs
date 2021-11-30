using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class GLAccountController: ApiController
    {


        public HttpResponseMessage GetSingleGLAccount(string id, string DisplayNumber, string InternalNumber)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
				SecurityUtility.AuthenticateAPICall(authToken.Tenant);
				GLAccountQueryService Service = new GLAccountQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = new GLAccount();
                if (!string.IsNullOrEmpty(id))
                {
                     Result = Service.GetGLAccountById(id, tenant);
                }
                else if (!string.IsNullOrEmpty(DisplayNumber)){
                    Result = Service.GetGLAccountByDisplayNumber(DisplayNumber, tenant);
                }

                else if (!string.IsNullOrEmpty(InternalNumber))
                {
                    Result = Service.GetGLAccountByInternalNumber(InternalNumber, tenant);
                }
                GLAccountQueryService mappingService = new GLAccountQueryService(tenant);
                Result.Cards = mappingService.GetGLAccountCards(Result);
                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(GLAccount entity)
        {
            GLAccount oldEntity = entity;

            if (ModelState.IsValid)
            {
                try
                {

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        int tenant = entity.Tenant;
						SecurityUtility.AuthenticateAPICall(authToken.Tenant);
						if (entity != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<GLAccount>(LogitudeXmlSerializer.SerializeObjectToXmlString(entity));
                        }

                        IAccountingContext MyContext = AccountingContext.GetContext(entity.Tenant);
                        GLAccountQueryService mappingService = new GLAccountQueryService(entity.Tenant);
                        GLAccountPM entityPM = mappingService.GLAccountDataMappingAndValidatin(entity, entity.Tenant);
                       // mappingService.SetInvoiceLinesEntityId(entityPM, entity.Tenant);

                      
                        entityPM.Tenant = entity.Tenant;







                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        GLAccountUpdateService service = new GLAccountUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        service.Update(entityPM,true);

                        entity = mappingService.GLAccountDataMappingAndValidatin(entityPM, entity.Tenant);
                        APIHelper.AddCommunicationLog("D", oldEntity, entity, "GLAccount", entityPM.Id, "GLAccount API", entity.Tenant);

                        scope.Complete();


                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "GLAccount", null, "GLAccount API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }

            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "GLAccount", null, "GLAccount API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(GLAccount entity)
        {
            GLAccount oldEntity = entity;

            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {

                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        int tenant = authToken.Tenant;
						SecurityUtility.AuthenticateAPICall(authToken.Tenant);
						if (entity != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<GLAccount>(LogitudeXmlSerializer.SerializeObjectToXmlString(entity));
                        }

                        IAccountingContext MyContext = AccountingContext.GetContext(tenant);
                        GLAccountQueryService mappingService = new GLAccountQueryService(tenant);
                        GLAccountPM entityPM = mappingService.GLAccountDataMappingAndValidatinForExternalAPI(entity, tenant);

                        if (entity.Parent != null)
                        {
                            mappingService.CheckParentCurrency(entityPM);
                        }

                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        GLAccountUpdateService service = new GLAccountUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        service.Update(entityPM, true);

                        APIHelper.AddCommunicationLog("D", oldEntity, entity, "GLAccount", entityPM.Id, "GLAccount API", authToken.Tenant);

                        scope.Complete();


                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "GLAccount", null, "GLAccount API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "GLAccount", null, "GLAccount API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }


    }
}