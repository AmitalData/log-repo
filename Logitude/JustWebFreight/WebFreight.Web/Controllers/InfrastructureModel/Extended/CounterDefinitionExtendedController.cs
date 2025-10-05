using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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
    public class CounterDefinitionExtendedController : ApiController
    {

        public HttpResponseMessage GetCustomizedCounterDefinitionsByCounterId(string counterId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                CounterDefinitionQuery entityQuery = new CounterDefinitionQuery(tenant);
                List<CounterDefinitionPM> myResult = entityQuery.GetCustomizedCounterDefinitionsByCounterId(counterId, tenant).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetCounterDefinitionsByCounterName(string counterName)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                CounterDefinitionQuery entityQuery = new CounterDefinitionQuery(tenant);
                List<CounterDefinitionPM> myResult = entityQuery.GetCounterDefinitionsByCounterName(counterName, tenant).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}