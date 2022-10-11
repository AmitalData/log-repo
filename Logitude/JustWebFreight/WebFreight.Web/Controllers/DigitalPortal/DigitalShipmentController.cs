using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using System.Data.Entity;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalShipmentController : ApiController
    {
        [HttpGet]
        [Route("DigitalShipment/GetSingle")]
        public HttpResponseMessage GetSingle(string id, string cardId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, id);
                id = shipmentIdAndTenant.Item1;
                var tenant = shipmentIdAndTenant.Item2;
                var shipmentQuery = new ShipmentQuery(tenant);
                var shipmentPM = shipmentQuery.GetSinglePM(id, tenant, cardId);

                if (shipmentPM.CustomerId == cardId || shipmentPM.AgentId == cardId || string.IsNullOrWhiteSpace(cardId))
                {
                    shipmentPM.TimeLineData = shipmentQuery.MapVerticalTimeLine(shipmentPM);
                    PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                    return Request.CreateResponse(HttpStatusCode.OK, shipmentPM);
                }

                throw new AutenticationException("Sorry! you are not authorized to read data!");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [Route("DigitalShipment/GetByFilters")]
        public HttpResponseMessage GetByFilters(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                newFilters.Tenant = authToken.Tenant;
                var shipmentQuery = new ShipmentQuery(authToken.Tenant);
                var entityLists = shipmentQuery.GetByFilters(newFilters);
                var response = new ServiceResponse();

                if (newFilters.GetCount)
                {
                    response.Count = entityLists.Count();
                }

                entityLists = QueryableExtensions.Skip(entityLists, () => newFilters.PageIndex);
                entityLists = QueryableExtensions.Take(entityLists, () => newFilters.PageSize);

                List<DigitalShipmentList> listQuery = entityLists.ToList();

                shipmentQuery.BuildShipmentListWithTimeLine(listQuery, authToken.Tenant);

                response.Result = listQuery;

                var reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route("DigitalShipment/GetShipmentActiveStatuses")]
        public IHttpActionResult GetShipmentActiveStatuses(string cardId)
        {
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);
                int tenant = authToken.Tenant;
                var entityStatusQuery = new EntityStatusQuery(tenant);
                var res = entityStatusQuery.GetDigitalPortalActiveStatuses(tenant)
                                           .Select(a => new EntityStatusList
                                           {
                                               DisplayName = a.Code.Equals("SDLY")
                                                               ? "Out for Delivery"
                                                               : a.Code.Equals("SHOR")
                                                               ? "Created"
                                                               : a.DisplayName,
                                               Id = a.Id,
                                               StatusWeight = a.StatusWeight,
                                               Name = a.Name
                                           })
                                           .OrderBy(a => a.StatusWeight)
                                           .ThenBy(a => a.Name)
                                           .ToList();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpGet]
        [Route("DigitalShipment/GetEntityEvents")]
        public IHttpActionResult GetEntityEvents(string entityId, string objectTableName, string cardType, string cardId)
        {
            var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
            var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, entityId, true, true);
            entityId = shipmentIdAndTenant.Item1;
            var tenant = shipmentIdAndTenant.Item2;
            var result = new List<TraceEventPM>();
            var objectTable = new ObjectTableRepository(tenant).GetObjectTableByName(objectTableName, 0, true);
            if (objectTable != null)
            {
                var traceEventsRepository = new TraceEventRepository(tenant);
                var traceEventQuery = new TraceEventQuery(traceEventsRepository);

                var dataQuery = traceEventQuery.GetTraceEventPMsByTenantByEntityId(tenant, entityId, objectTable.Id);

                var resultQuery = cardType == null
                                     || cardType.Equals("AG", StringComparison.InvariantCultureIgnoreCase)
                                        ? dataQuery.Where(d => d.IsAgentView)
                                        : dataQuery.Where(d => d.IsCustomerView);

                return Ok(resultQuery.OrderByDescending(s => s.EventDateTime).ToList());
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("DigitalShipment/GetShipmentRoutingLegs")]
        public IHttpActionResult GetShipmentRoutingLegs(string shipmentId, string cardId)
        {
            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, shipmentId);
                shipmentId = shipmentIdAndTenant.Item1;
                var tenant = shipmentIdAndTenant.Item2;
                var shipmentQuery = new ShipmentQuery(tenant);
                var routingLegs = shipmentQuery.GetDigitalShipmentRoutingLegs(shipmentId, tenant);
                return Ok(routingLegs);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpPut]
        [Route("DigitalShipment/PutShipmentDigitalField")]
        public IHttpActionResult PutShipmentDigitalField(ShipmentDigitalArchivedArgs archivedrgs)
        {
            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(archivedrgs.CardId, archivedrgs.ShipmentId);
                var shipmentId = shipmentIdAndTenant.Item1;
                var tenant = shipmentIdAndTenant.Item2;
                ShipmentDigitalFieldRepository shipmentDigitalFieldRepository = new ShipmentDigitalFieldRepository(tenant);
                var shipmentDigitalField = shipmentDigitalFieldRepository.GetSingleShipmentDigitalFields(shipmentId, tenant);
                shipmentDigitalField.IsCustomerArchived = archivedrgs.IsCustomerArchived;
                shipmentDigitalFieldRepository.Update(shipmentDigitalField);
                shipmentDigitalFieldRepository.SubmitChanges();
                return Ok(shipmentDigitalField);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpGet]
        [Route("DigitalShipment/GetShipmentDigitalTransports")]
        public IHttpActionResult GetShipmentDigitalTransports(string cardId)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                var shipmentQuery = new ShipmentQuery(authToken.Tenant);
                var routingLegs = shipmentQuery.GetTransportModesWithSubTypes(authToken.Tenant);
                return Ok(routingLegs);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }
    }
}