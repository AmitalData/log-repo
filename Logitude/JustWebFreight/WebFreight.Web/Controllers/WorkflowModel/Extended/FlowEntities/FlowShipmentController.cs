using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery;
using Marvin.JsonPatch;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http;
using System.Transactions;
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
    public class FlowShipmentController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            int tenant = 0;

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                tenant = authToken.Tenant;

                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(id, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, shipmentPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(ex, tenant));
            }
        }

        public HttpResponseMessage PostByFilterTree(ApiQueryTreeFilters apiQueryTreeFilters)
        {
            int tenant = 0;

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                tenant = authToken.Tenant;

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                //IQueryable<ShipmentList> shipmentsQuery = shipmentQuery.GetAllShipmentListTenant(tenant);
                var shipmentsQuery = shipmentRepository.GetShipmentViewsByTenant(tenant);

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                TreeFilterQueryArgs treeFilterQueryArgs = new TreeFilterQueryArgs()
                {
                    AdditionalTreeFilter = javaScriptSerializer.Serialize(apiQueryTreeFilters.QueryFilterItem),
                    ObjectTableName = "Shipment",
                    Tenant = tenant,
                };

                shipmentsQuery = new TreeFilterQueryService().Apply(shipmentsQuery, treeFilterQueryArgs);
                shipmentsQuery = shipmentsQuery.OrderBy(string.IsNullOrEmpty(apiQueryTreeFilters.OrderBy) ? "Id" : apiQueryTreeFilters.OrderBy);
                shipmentsQuery = shipmentsQuery.Skip(0);
                shipmentsQuery = shipmentsQuery.Take(apiQueryTreeFilters.PageSize);

                var shipments = shipmentsQuery.Select("new { " + apiQueryTreeFilters.ReturnedColumns + " }").ToDynamicList();

                ServiceResponse response = new ServiceResponse();
                response.Result = shipments;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(ex, tenant));
            }
        }

        public HttpResponseMessage Post(ShipmentPM entityPM)
        {
            int tenant = 0;

            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        tenant = authToken.Tenant;

                        IShipmentsContext objectContext = ShipmentsContext.GetContext(entityPM.Tenant);
                        ShipmentService service = new ShipmentService(objectContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                        service.Create();

                        IShipmentsContext updatedEntityContext = ShipmentsContext.GetContext(tenant);
                        ShipmentRepository updatedEntityRepository = new ShipmentRepository(updatedEntityContext);
                        ShipmentQuery updatedShipmentQuery = new ShipmentQuery(updatedEntityRepository);
                        entityPM = updatedShipmentQuery.GetSinglePM(entityPM.Id, entityPM.Tenant);

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(ex, tenant));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage Patch(string id, JsonPatchDocument<ShipmentPM> shipmentJsonPatch)
        {
            int tenant = 0;

            try
            {
                using (TransactionScope transactionScope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    tenant = authToken.Tenant;

                    IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
                    ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
                    ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                    ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(id, tenant);

                    if (shipmentPM == null) { throw new Exception("Cannot find the shipment"); }

                    SecurityUtility.AuthenticationOnEntityTenant("Shipment", shipmentPM.Tenant, tenant);

                    shipmentJsonPatch.ApplyTo(shipmentPM);
                    //shipmentPM = new JsonPatchApplier<ShipmentPM>(shipmentJsonPatch, shipmentPM).Apply();

                    ShipmentService shipmentService = new ShipmentService(shipmentsContext, shipmentPM, SecurityUtility.GetAuthenticatedUser());
                    shipmentService.Update(true, isPatchUpdate: true);

                    ShipmentPM updatedShipmentPM = shipmentQuery.GetSinglePM(shipmentPM.Id, shipmentPM.Tenant);

                    transactionScope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, updatedShipmentPM);
                }
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(exception, tenant));
            }
        }
    }
}