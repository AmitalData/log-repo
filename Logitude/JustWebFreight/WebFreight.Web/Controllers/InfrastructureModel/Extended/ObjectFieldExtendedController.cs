using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class ObjectFieldExtendedController : ApiController
    {
        public HttpResponseMessage GetEntityAuomationAllowedinAutomationConditionsObjectFieldPMsByEntityTableIds(string entityAutomationIds , int tenant)
        {
            try
            {
               
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(authToken.Tenant);
                List<ObjectFieldPM> objectFieldPMs =!string.IsNullOrEmpty(entityAutomationIds)? objectFieldQuery.GetEntityAuomationAllowedinAutomationConditionsObjectFieldPMsByEntityTableIds(entityAutomationIds.Split(',').ToList(), authToken.Tenant):null;
                return Request.CreateResponse(HttpStatusCode.OK, objectFieldPMs);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDefaultAdditionalFiltersById(string objectFieldId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("ObjectField", authToken.Tenant, tenant);

                IWebFreightContext webFreightContext = WebFreightContext.GetContext(authToken.Tenant);
                ObjectFieldService objectFieldService = new ObjectFieldService(webFreightContext, authToken.Tenant);
                var defaultAdditionalTreeFilters = objectFieldService.GetDefaultAdditionalFiltersByIdAndTenant(objectFieldId, authToken.Tenant);
                
                return Request.CreateResponse(HttpStatusCode.OK, defaultAdditionalTreeFilters);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetObjectFieldByName(string objectFieldName,string querySection)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(authToken.Tenant);
                var objectFieldPMs = objectFieldQuery.GetObjectFieldByName(objectFieldName, querySection);
                return Request.CreateResponse(HttpStatusCode.OK, objectFieldPMs);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSingleQuery(string UniqueCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                QueryQuery queryQuery = new QueryQuery(authToken.Tenant);
                QueryPM queryPM = queryQuery.GetSingleQueryPM(UniqueCode);

                return Request.CreateResponse(HttpStatusCode.OK, queryPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

    }
}