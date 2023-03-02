using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.Controllers.WorkflowModel.Models;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.Workflow;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WorkflowModel.Extended.FlowEntities
{
    public class FlowShipmentPayableController : ApiController
    {
        public HttpResponseMessage PostByFilterTree(ApiQueryTreeFilters apiQueryTreeFilters)
        {
            int tenant = 0;

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                tenant = authToken.Tenant;

                IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(authToken.Tenant);
                ShipmentPayableRepository shipmentPayableRepository = new ShipmentPayableRepository(shipmentsContext);
                IQueryable<ShipmentPayable> shipmentPayableQuery = shipmentPayableRepository.GetShipmentPayables(authToken.Tenant);

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                TreeFilterQueryArgs treeFilterQueryArgs = new TreeFilterQueryArgs()
                {
                    AdditionalTreeFilter = javaScriptSerializer.Serialize(apiQueryTreeFilters.QueryFilterItem),
                    ObjectTableName = "ShipmentPayable",
                    Tenant = tenant,
                };

                shipmentPayableQuery = new TreeFilterQueryService().Apply(shipmentPayableQuery, treeFilterQueryArgs);
                shipmentPayableQuery = shipmentPayableQuery.OrderBy(string.IsNullOrEmpty(apiQueryTreeFilters.OrderBy) ? "Id" : apiQueryTreeFilters.OrderBy);
                shipmentPayableQuery = shipmentPayableQuery.Skip(0);
                shipmentPayableQuery = shipmentPayableQuery.Take(apiQueryTreeFilters.PageSize);

                var shipmentPayables = shipmentPayableQuery.Select("new { " + apiQueryTreeFilters.ReturnedColumns + " }").ToDynamicList();

                ServiceResponse response = new ServiceResponse();
                response.Result = shipmentPayables;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(ex, tenant));
            }
        }
    }
}