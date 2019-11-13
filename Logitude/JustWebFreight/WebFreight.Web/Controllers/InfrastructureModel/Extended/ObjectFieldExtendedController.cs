using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
                ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(authToken.Tenant);
                List<ObjectFieldPM> objectFieldPMs =!string.IsNullOrEmpty(entityAutomationIds)? objectFieldQuery.GetEntityAuomationAllowedinAutomationConditionsObjectFieldPMsByEntityTableIds(entityAutomationIds.Split(',').ToList(), authToken.Tenant):null;
                return Request.CreateResponse(HttpStatusCode.OK, objectFieldPMs);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}