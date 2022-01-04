
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.CRMModel.DomainServices;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.Tools.EntityService;
using System.Web;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Security;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace WebFreight.Web.Controllers.GlobalModel.Extended
{
    public partial class AdvancedQueryFiltersExtendedController: ApiController
    {
        public HttpResponseMessage GetSingleByObjectFieldCodeAndTenant(string objectFieldId, int tenant, string queryCode, string loggedUserId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);


                AdvancedQueryFilterQuery repo = new AdvancedQueryFilterQuery(tenant);
                var advancedQueryFilter = repo.GetSingleByObjectFieldCodeAndTenant(objectFieldId, tenant,queryCode, loggedUserId);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, advancedQueryFilter);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



    }
}