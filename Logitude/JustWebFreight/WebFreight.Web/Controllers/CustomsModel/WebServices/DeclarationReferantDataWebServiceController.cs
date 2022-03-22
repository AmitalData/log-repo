
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Customs.BL.DataContracts;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class DeclarationReferantDataWebServiceController : ApiController
    {

        public HttpResponseMessage GetDeclarationReferantDataDashBoard(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int myTenant = authToken.Tenant;

                DeclarationReferantDataQueryService declarationReferantDataService = new DeclarationReferantDataQueryService(tenant);
                List<DeclarationReferantDataChartingClass> myResult = declarationReferantDataService.GetDeclarationReferantDataDashBoard(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage GetQueriesCounts(string refId, string depId, string transportMode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                ICustomContext context = CustomContext.GetContext(tenant);
                DeclarationReferantDataListQueryService declarationCourierStatusQueryService = new DeclarationReferantDataListQueryService(context);
                var refList = refId != null ? refId.Split(',').ToList() : new List<string>();
                var depList = depId != null ? depId.Split(',').ToList() : new List<string>();
                var counts = declarationCourierStatusQueryService.GetQueriesCounts(tenant, refList, depList, transportMode);
                return Request.CreateResponse(HttpStatusCode.OK, counts);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetAdvancedQueryFilters(int tenant, string loggedcontactid, string queryCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                ICustomContext context = CustomContext.GetContext(authToken.Tenant);

                DeclarationReferantDataQueryService advancedQueryFilterQuery = new DeclarationReferantDataQueryService(tenant);
                var result = advancedQueryFilterQuery.GetAdvancedQueryFilterPMsByTenantAndUserAndQuery(tenant, loggedcontactid, queryCode);

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage insertDeclarationReferantFilters(AdvancedQueryFilterPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                AdvancedQueryFilterService service = new AdvancedQueryFilterService(objectContext, entityPM.Tenant);
                service.Create(entityPM);

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage Put(AdvancedQueryFilterPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                AdvancedQueryFilterService service = new AdvancedQueryFilterService(objectContext, entityPM.Tenant);
                service.Update(entityPM);

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}
