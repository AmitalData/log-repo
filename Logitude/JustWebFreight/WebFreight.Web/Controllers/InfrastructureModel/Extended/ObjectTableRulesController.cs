using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
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
    public class ObjectTableRulesController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ObjectTableRuleQuery objectTableRuleQuery = new ObjectTableRuleQuery(authToken.Tenant);
                ObjectTableRulePM objectTableRulePM = objectTableRuleQuery.GetSinglePM(id, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, objectTableRulePM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage Post(ObjectTableRulePM entityPM)
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
                        SecurityUtility.AuthenticationOnEntityTenant("ObjectTableRule", entityPM.Tenant, authToken.Tenant);

                        IWebFreightContext MyContext = WebFreightContext.GetContext(entityPM.Tenant);
                        ObjectTableRuleService service = new ObjectTableRuleService(MyContext, authToken.Tenant);
                        service.Create(entityPM);

                        ClearRulesCache(entityPM);

                        scope.Complete();
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


        public HttpResponseMessage Put(ObjectTableRulePM entityPM)
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

                        SecurityUtility.AuthenticationOnEntityTenant("ObjectTableRule", entityPM.Tenant, authToken.Tenant);


                        IWebFreightContext MyContext = WebFreightContext.GetContext(entityPM.Tenant);
                        ObjectTableRuleService service = new ObjectTableRuleService(MyContext, authToken.Tenant);
                        service.Update(entityPM, entityPM.RuleConditionFields);

                        ClearRulesCache(entityPM);

                        scope.Complete();
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

        public HttpResponseMessage GetRestoredDefaultRule(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                IWebFreightContext MyContext = WebFreightContext.GetContext(authToken.Tenant);
                ObjectTableRuleService service = new ObjectTableRuleService(MyContext, authToken.Tenant);
                service.Delete(id);

                

                return Request.CreateResponse(HttpStatusCode.OK, true);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        private static void ClearRulesCache(ObjectTableRulePM entityPM)
        {
            string entityName = "ObjectTableRule" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "ObjectTableRulePM" + entityPM.Id + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            string pmslistName = "objecttablerulepmstenant" + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(pmslistName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(pmslistName);
            }
        }

        public HttpResponseMessage GetObjectTableRulePMsByTenant(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ObjectTableRuleQuery objectTableRuleQuery = new ObjectTableRuleQuery(tenant);
                List<ObjectTableRulePM> result = objectTableRuleQuery.GetObjectTableRulePMsByTenant(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

    }
}