using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;


namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class ObjectTableRuleFieldsController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ObjectTableRuleFieldQuery objectTableRuleFieldQuery = new ObjectTableRuleFieldQuery(authToken.Tenant);
                ObjectTableRuleFieldPM objectTableRuleFieldPM = objectTableRuleFieldQuery.GetSinglePM(id, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, objectTableRuleFieldPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage Post(ObjectTableRuleFieldPM entityPM)
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
                        SecurityUtility.AuthenticationOnEntityTenant("ObjectTableRuleField", entityPM.Tenant, authToken.Tenant);

                        IWebFreightContext MyContext = WebFreightContext.GetContext(entityPM.Tenant);
                        ObjectTableRuleFieldService service = new ObjectTableRuleFieldService(MyContext, entityPM.Tenant);
                        service.Create(entityPM);

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


        public HttpResponseMessage Put(ObjectTableRuleFieldPM entityPM)
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
                        SecurityUtility.AuthenticationOnEntityTenant("ObjectTableRuleField", entityPM.Tenant, authToken.Tenant);

                        string entityName = "ObjectTableRuleField" + entityPM.Id + entityPM.Tenant;
                        string entityPmName = "ObjectTableRuleFieldPM" + entityPM.Id + entityPM.Tenant;
                        if (CacheManager.CacheWrapper.Get(entityName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityName);
                        }
                        if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityPmName);
                        }

                        IWebFreightContext MyContext = WebFreightContext.GetContext(entityPM.Tenant);
                        ObjectTableRuleFieldService service = new ObjectTableRuleFieldService(MyContext, entityPM.Tenant);
                        service.Update(entityPM);

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


        public HttpResponseMessage PutRuleFieldsList(List<ObjectTableRuleFieldPM> fuleFieldsPMList)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    IWebFreightContext ObjectContext = WebFreightContext.GetContext(authToken.Tenant);
                    ObjectTableRuleFieldRepository objectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(ObjectContext);

                    ObjectTableRuleFieldService service = new ObjectTableRuleFieldService(ObjectContext, authToken.Tenant);
                    foreach (ObjectTableRuleFieldPM entityPM in fuleFieldsPMList)
                    {
                        SecurityUtility.AuthenticationOnEntityTenant("ObjectTableRuleField", entityPM.Tenant, authToken.Tenant);



                        switch (entityPM.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                service.Create(entityPM);
                                break;
                            case ChangeSetOperation.Update:
                                service.Update(entityPM);
                                break;
                            case ChangeSetOperation.Delete:
                                ObjectTableRuleField objectTableRuleField = objectTableRuleFieldRepository.GetSingleObjectTableRuleField(entityPM.Id, entityPM.Tenant);
                                objectTableRuleFieldRepository.Remove(objectTableRuleField);
                                break;
                        }

                    }

                    ObjectContext.SaveChanges();
                    scope.Complete();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetObjectTableRuleFieldPMsByTenant(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                ObjectTableRuleFieldQuery objectTableRuleFieldQuery = new ObjectTableRuleFieldQuery(tenant);
                List<ObjectTableRuleFieldPM> result = objectTableRuleFieldQuery.GetObjectTableRuleFieldPMsByTenant(tenant).ToList();
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