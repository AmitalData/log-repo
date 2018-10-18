using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class EntityChangeExtendedController : ApiController
    {
        public HttpResponseMessage GetEntityChangePMsByEntityIdAndObjectTable(string entityId, string objectTableId, int tenant)
        {
            try
            {
                EntityChangeQuery entityChangeQuery = new EntityChangeQuery(tenant);
                List<EntityChangePM> entityChangePMLists = entityChangeQuery.GetEntityChangePMsByEntityIdAndObjectTable(entityId, objectTableId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, entityChangePMLists);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetEntityChangeAutomationsSummaryByEntityChangeId(string entitychangeId, string objectTableName, int tenant)
        {
            try
            {
                EntityChangeAutomationHelper entityChangeAutomationHelper = new EntityChangeAutomationHelper();
                EntityChangeAutomationsSummary entityChangeAutomationsSummary = entityChangeAutomationHelper.GetEntityChangeAutomationsSummary(entitychangeId, objectTableName, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, entityChangeAutomationsSummary);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
  
    }
}