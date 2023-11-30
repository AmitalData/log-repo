using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
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
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class TipsVisibilityController : ApiController
    {
        public HttpResponseMessage GetTipsVisibilities(int tenant, string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                TipsVisibilityQuery tipQuery = new TipsVisibilityQuery(tenant);
                List<TipsVisibilityPM> tipsVisibilityLists = tipQuery.GetTipsVisibilities(tenant, userId);

                return Request.CreateResponse(HttpStatusCode.OK, tipsVisibilityLists);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSingleTipsVisibilityPM(string id , int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                TipsVisibilityQuery tipQuery = new TipsVisibilityQuery();
                TipsVisibilityPM tipsVisibilityPM = tipQuery.GetSingleTipsVisibilityPM(id,tenant);

                return Request.CreateResponse(HttpStatusCode.OK, tipsVisibilityPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage Post(TipsVisibilityPM newEntity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("TipsVisibility", newEntity.Tenant, authToken.Tenant);

                        IWebFreightContext objectContext = WebFreightContext.GetContext(newEntity.Tenant);
                        TipsVisibilityService service = new TipsVisibilityService(objectContext, newEntity.Tenant);
                        TipsVisibilityRepository tipsVisibilityRepository = new TipsVisibilityRepository(objectContext);
                        bool exists = tipsVisibilityRepository.GetSingleTipsVisibility(newEntity.Id, newEntity.Tenant) != null ? true : false;

                        if (!exists)   service.Create(newEntity);
                      
                        else
                        {
                            throw new Exception("This Entity Already exists!");
                        }
                    

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, newEntity);
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

        public HttpResponseMessage Put(TipsVisibilityPM currentEntity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("TipsVisibility", currentEntity.Tenant, authToken.Tenant);


                        IWebFreightContext objectContext = WebFreightContext.GetContext(currentEntity.Tenant);
                        TipsVisibilityService service = new TipsVisibilityService(objectContext, currentEntity.Tenant);
                        service.Update(currentEntity);

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, currentEntity);
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
        

    }
}