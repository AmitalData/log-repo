using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Counters;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Global
{
    public partial class QueriesController : ApiController
    {

        //public HttpResponseMessage GetAdvancedQueryFiltersByTenant(int tenant,string loggedcontactid)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

        //        AdvancedQueryFilterQuery advancedQueryFilterQuery = new AdvancedQueryFilterQuery(tenant);
        //        var result = advancedQueryFilterQuery.GetAdvancedQueryFilterPMsByTenantAndUser(tenant, loggedcontactid);
        //        var temp = result.Where(a => a.QueryId == "1-3887");

        //        return Request.CreateResponse(HttpStatusCode.OK, result);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }

        //}

        public HttpResponseMessage Post(QueryPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("", entityPM.Tenant, authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                QueryService service = new QueryService(objectContext, entityPM.Tenant);
                entityPM.Id = IdCounter.GetNumber("Query", entityPM.Tenant).ToString();
                entityPM.Code = entityPM.Id;
                if(entityPM.UserId==null)
                  entityPM.UniqueCode = entityPM.ObjectTableName + '.' + entityPM.Code;
                else
                  entityPM.UniqueCode = entityPM.ObjectTableName +'.'+ entityPM.UserId + '.' + entityPM.Code;
                service.Create(entityPM);

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Put(QueryPM entityPM)
        { 
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("", entityPM.Tenant, authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                QueryService service = new QueryService(objectContext, entityPM.Tenant);
                //entityPM.OriginalQueryId = null;
                service.Update(entityPM);
                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Delete(string UniqueCode, string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);
                QueryRepository queryRepository = new QueryRepository(objectContext);
                SharedUserQueryRepository sharedUserQueryRepository = new SharedUserQueryRepository(objectContext);
                QueryColumnRepository queryColumnRepository = new QueryColumnRepository(objectContext);
                AdvancedQueryFilterRepository advancedQueryFilterRepository = new AdvancedQueryFilterRepository(objectContext);

                Query myQuery = queryRepository.GetSingleQueryByUniqueCode(UniqueCode);
                List<SharedUserQuery> sharedUserQueries = sharedUserQueryRepository.GetAllByQueryCode(UniqueCode);
                List<QueryColumn> queryColumns = null;
                List<AdvancedQueryFilter> advancedQueryFilters = null;

                if (myQuery != null)
                {
                    if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(myQuery.SharedByUserId) && myQuery.SharedByUserId != userId)
                    {
                        queryColumns = queryColumnRepository.GetQueryColumnsByQueryCodeAndUser(tenant, myQuery.SharedByUserId, UniqueCode);
                        advancedQueryFilters = advancedQueryFilterRepository.GetAdvancedQueryFiltersByTenantAndUserAndQuery(tenant, myQuery.SharedByUserId, UniqueCode);
                    }

                    else
                    {
                        queryColumns = queryColumnRepository.GetQueryColumnsByQueryCode(tenant, UniqueCode);
                        advancedQueryFilters = advancedQueryFilterRepository.GetAdvancedQueryFiltersByTenantAndAndQuery(tenant, UniqueCode);
                    }

                    foreach (QueryColumn column in queryColumns)
                    {
                        queryColumnRepository.Remove(column);
                    }

                    foreach (AdvancedQueryFilter filter in advancedQueryFilters)
                    {
                        advancedQueryFilterRepository.Remove(filter);
                    }
                    
                    foreach (SharedUserQuery item in sharedUserQueries)
                    {
                        sharedUserQueryRepository.Remove(item);
                    }

                    queryRepository.Remove(myQuery);
                    objectContext.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, myQuery);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingle(string UniqueCode)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                QueryQuery queryQuery = new QueryQuery(authToken.Tenant);
                QueryPM queryPM = queryQuery.GetSingleQueryPM(UniqueCode, authToken.Tenant);
                
                return Request.CreateResponse(HttpStatusCode.OK, queryPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetAllSystemViewsByObjectTable(string objectTableName)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                QueryQuery queryQuery = new QueryQuery(authToken.Tenant);
                List<QueryPM> queriesPM = queryQuery.GetQueryPMsByTenant(authToken.Tenant).Where(query => query.ObjectTableName == objectTableName && query.SystemLevel).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, queriesPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}